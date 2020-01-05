using System.Collections.Generic;
using System.IO;
using Capabilities.Util;
using Sim;
using Input = Sim.Input;

namespace Capabilities
{
    public class MemorySaveFactory : CapabilityFactory
    {
        private readonly Memory memory;
        
        public MemorySaveFactory(Memory memory)
        {
            this.memory = memory;
        }
        
        public override Capability Create(StringReader genome)
        {
            return new MemorySave(memory);
        } 
    }
    
    public class MemorySave : Capability
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
                Data = new []{input.Data[0]},
                Energy = -EnergyCost,
            };
        }
    }

}