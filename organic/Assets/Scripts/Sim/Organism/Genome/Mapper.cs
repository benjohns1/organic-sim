using System;
using System.Collections.Generic;
using System.Linq;

namespace Sim.Organism.Genome
{
    public class Mapper
    {
        public List<char> Dialect { get; }
        private readonly List<CapabilityFactory> capabilityFactories;
        public int GeneLength { get; }

        public Mapper(IEnumerable<char> dialect, IEnumerable<CapabilityFactory> capabilityFactories)
        {
            Dialect = dialect.ToList();
            this.capabilityFactories = capabilityFactories.ToList();
            
            GeneLength = (int) Math.Ceiling(Math.Log(this.capabilityFactories.Count, Dialect.Count));
        }

        public Dictionary<string, CapabilityFactory> Generate()
        {
            var map = new Dictionary<string, CapabilityFactory>();
            var genes = GenerateGenes().ToArray();
            for (var i = 0; i < capabilityFactories.Count; i++)
            {
                map.Add(genes[i], capabilityFactories[i]);
            }
            for (var j = capabilityFactories.Count; j < genes.Length; j++)
            {
                map.Add(genes[j], null);
            }

            return map;
        }

        private IEnumerable<string> GenerateGenes()
        {
            List<string> GenerateGenesRecursive(List<string> genes, string gene, int depth)
            {
                if (depth == GeneLength)
                {
                    genes.Add(gene);
                    return genes;
                }
                depth++;

                return Dialect.Aggregate(genes, (current, token) => GenerateGenesRecursive(current, $"{gene}{token}", depth));
            }

            return GenerateGenesRecursive(new List<string>(), "", 0);
        }
    }
}