using System;
using System.Collections.Generic;
using System.Linq;

namespace Sim.Organism
{
    public class GeneFactory
    {
        private readonly Dictionary<char, InteractionFactory> interactionFactories;
        private readonly char geneSeparator;
        private readonly char layerSeparator;
        private readonly char[] dialect;
        private readonly Random random;

        public GeneFactory(Dictionary<char, InteractionFactory> interactionFactories, char geneSeparator, char layerSeparator)
        {
            this.interactionFactories = interactionFactories;
            this.geneSeparator = geneSeparator;
            this.layerSeparator = layerSeparator;
            var keys = interactionFactories.Keys.ToList();
            keys.AddRange(new char[] {this.geneSeparator, this.layerSeparator});
            dialect = keys.ToArray();
            random = new Random();
        }

        public string Mutate(string sequence)
        {
            var dnaSequence = sequence;
            
            const float deleteRate = 0.1f;
            var deleteCount = (int) (dnaSequence.Length * deleteRate);
            for (var i = 0; i < deleteCount; i++)
            {
                var index = random.Next(0, dnaSequence.Length);
                dnaSequence = dnaSequence.Remove(index, 1);
            }

            const float addRate = 0.1f;
            var addCount = (int) (dnaSequence.Length * addRate);
            for (var i = 0; i < addCount; i++)
            {
                var newElement = random.Next(0, dialect.Length);
                var index = random.Next(0, dnaSequence.Length);
                dnaSequence = dnaSequence.Insert(index, dialect[newElement].ToString());
            }

            return dnaSequence;
        }
        
        public Interaction[][] ParseGenome(string dnaSequence)
        {
            var dnaLayers = dnaSequence.Split(layerSeparator);
            return dnaLayers.Select(dnaLayer => dnaLayer.Split(geneSeparator)).Select(dnaGenes => dnaGenes.Select(CreateInteraction).Where(interaction => interaction != null).ToArray()).ToArray();
        }

        private Interaction CreateInteraction(string gene)
        {
            if (gene.Length == 0)
            {
                return null;
            }
            
            var first = gene.Substring(0, 1).ToCharArray()[0];
            var parameters = gene.Substring(1);
            return interactionFactories.TryGetValue(first, out var interactionFactory)
                ? interactionFactory.Create(parameters)
                : null;
        }
    }
}