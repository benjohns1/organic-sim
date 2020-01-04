using Interactions.Util;
using Sim;
using Input = Sim.Input;

namespace Interactions
{
    public class MemorySaveFactory : InteractionFactory
    {
        private Memory memory;
        
        public MemorySaveFactory(Memory memory)
        {
            this.memory = memory;
        }
        
        public override Interaction Create(string dnaParameters)
        {
            return new MemorySave(this.memory);
        } 
    }
    
    public class MemorySave : Interaction
    {
        private readonly Memory memory;
        private const long EnergyCost = 1;

        public MemorySave(Memory memory)
        {
            this.memory = memory;
        }

        public override Output Run(Input input)
        {
            memory.Data = input.Data[0];
            return new Output
            {
                Data = input.Data[0],
                Energy = -EnergyCost,
            };
        }
    }

}