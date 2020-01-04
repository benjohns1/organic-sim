using Interactions.Util;
using Sim;
using Input = Sim.Input;

namespace Interactions
{
    public class MemoryLoadFactory : InteractionFactory
    {
        private Memory memory;
        
        public MemoryLoadFactory(Memory memory)
        {
            this.memory = memory;
        }
        
        public override Interaction Create(string dnaParameters)
        {
            return new MemoryLoad(memory);
        } 
    }
    
    public class MemoryLoad : Interaction
    {
        private readonly Memory memory;
        private const long EnergyCost = 1;

        public MemoryLoad(Memory memory)
        {
            this.memory = memory;
        }

        public override int GetInputCount()
        {
            return 0;
        }

        public override Output Run(Input input)
        {
            return new Output
            {
                Data = memory.Data,
                Energy = -EnergyCost,
            };
        }
    }

}