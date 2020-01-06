using System.IO;
using Sim.Organism.Genome;
using Input = Sim.Organism.Genome.Input;

namespace Capabilities
{
    public class RandomFactory : CapabilityFactory
    {
        private readonly int? seed;

        public RandomFactory(int? seed)
        {
            this.seed = seed;
            HumanReadableName = $"{GetType().Name}({seed})";
        }

        public override string HumanReadableName { get; }

        public override Capability Create(string gene, StringReader genome)
        {
            var hr = new HumanReadable(HumanReadableName, gene);
            return new Random(hr, seed);
        } 
    }
    
    public class Random : Capability
    {
        private readonly System.Random random;
        
        public Random(HumanReadable hr, int? seed)
        {
            HumanReadable = hr;
            random = seed == null ? new System.Random() : new System.Random((int)seed);
        }

        public override int InputCount => 0;

        public override Output Run(Input input)
        {
            return new Output
            {
                Data = new []{RandomUlong()},
                Energy = -1,
            };
        }

        private ulong RandomUlong()
        {
            var buffer = new byte[8];
            random.NextBytes(buffer);
            ulong value = 0;
            for (var offset = 0; offset < 8; offset++)
            {
                value <<= 8;
                value |= buffer[offset];
            }

            return value;
        }
    }

}