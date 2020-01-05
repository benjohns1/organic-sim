using System.Collections.Generic;
using System.IO;
using Sim;
using Input = Sim.Input;

namespace Capabilities
{
    public class RandomFactory : CapabilityFactory
    {
        private readonly int? seed;

        public RandomFactory(int? seed)
        {
            this.seed = seed;
        }

        public override Capability Create(StringReader genome)
        {
            return new Random(seed);
        } 
    }
    
    public class Random : Capability
    {
        private readonly System.Random random;
        
        public Random(int? seed)
        {
            random = seed == null ? new System.Random() : new System.Random((int)seed);
        }
        
        public override int GetInputCount()
        {
            return 0;
        }
        
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