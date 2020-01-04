using Sim;
using Input = Sim.Input;

namespace Interactions
{
    public class RandomFactory : InteractionFactory
    {
        public override Interaction Create(string dnaParameters)
        {
            return new Random();
        } 
    }
    
    public class Random : Interaction
    {
        private readonly int? seed = null;
        private readonly System.Random random;
        
        public Random()
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
                Data = RandomUlong(),
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