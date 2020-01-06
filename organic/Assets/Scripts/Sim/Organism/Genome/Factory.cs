using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
            map = genomeMapper.GeneMap;
            dialect = genomeMapper.Dialect;
            geneLength = genomeMapper.GeneLength;
            
            this.mutationRates = mutationRates;
            random = seed == null ? new Random() : new Random((int) seed);
        }

        private static float FloatThreshold(float val)
        {
            return int.MaxValue * Math.Max(0f, Math.Min(1f, val));
        }

        public string Mutate(string genome)
        {
            var mutated = new StringBuilder();
            
            var deleteThreshold = FloatThreshold(mutationRates.DeleteBase);
            var addThreshold = FloatThreshold(mutationRates.AddBase);
            foreach (var currentBase in genome)
            {
                if (random.Next(0, int.MaxValue) > deleteThreshold)
                {
                    mutated.Append(currentBase);
                }
                
                if (random.Next(0, int.MaxValue) > addThreshold)
                {
                    continue;
                }
                
                var newBase = dialect[random.Next(0, dialect.Count)];
                mutated.Append(newBase);
            }

            return mutated.ToString();
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

            return capabilityFactory.Create(gene, reader);
        }
    }
}