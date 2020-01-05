using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sim.Organism.Genome;

namespace Sim.Organism.Genome
{
    public struct MutationRates
    {
        public float DeleteBase;
        public float AddBase;
    }
    
    public class Factory
    {
        private readonly Dictionary<string, CapabilityFactory> map;
        private readonly List<char> dialect;
        private readonly Random random;
        private readonly MutationRates mutationRates;
        private readonly int geneLength;

        public Factory(Mapper genomeMapper, MutationRates mutationRates, int? seed)
        {
            map = genomeMapper.Generate();
            dialect = genomeMapper.Dialect;
            geneLength = genomeMapper.GeneLength;
            
            this.mutationRates = mutationRates;
            random = seed == null ? new Random() : new Random((int) seed);
        }

        public string Mutate(string genome)
        {
            var deleteCount = (int) (genome.Length * mutationRates.DeleteBase);
            for (var i = 0; i < deleteCount; i++)
            {
                var index = random.Next(0, dialect.Count);
                genome = genome.Remove(index, 1);
            }

            var addCount = (int) (genome.Length * mutationRates.AddBase);
            for (var i = 0; i < addCount; i++)
            {
                var newElement = random.Next(0, dialect.Count);
                var index = random.Next(0, dialect.Count);
                genome = genome.Insert(index, dialect[newElement].ToString());
            }

            return genome;
        }
        
        public IEnumerable<Capability> ParseGenome(string genome)
        {
            var capabilities = new List<Capability>();
            using (var reader = new StringReader(genome))
            {
                var buffer = new char[geneLength];
                while (reader.Read(buffer, 0, geneLength) == geneLength)
                {
                    var gene = new string(buffer);
                    var capability = CreateCapabilityFromGene(gene, reader);
                    if (capability != null)
                    {
                        capabilities.Add(capability);   
                    }
                }
            }

            return capabilities;
        }

        private Capability CreateCapabilityFromGene(string gene, StringReader reader)
        {
            var found = map.TryGetValue(gene, out var capabilityFactory);
            if (capabilityFactory == null)
            {
                return null;
            }

            if (!found)
            {
                throw new Exception($"no gene '{gene}' found in genome map");
            }

            return capabilityFactory.Create(reader);
        }
    }
}