using System.Collections.Generic;
using System.IO;
using Capabilities.Util;
using Sim;
using Input = Sim.Input;

namespace Capabilities
{
    public class MemoryLoadFactory : CapabilityFactory
    {
        private readonly Memory memory;
        
        public MemoryLoadFactory(Memory memory)
        {
            this.memory = memory;
        }
        
        public override Capability Create(StringReader genome)
        {
            return new MemoryLoad(memory);
        } 
    }
    
    public class MemoryLoad : Capability
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
                Data = new []{memory.Data},
                Energy = -EnergyCost,
            };
        }
    }

}