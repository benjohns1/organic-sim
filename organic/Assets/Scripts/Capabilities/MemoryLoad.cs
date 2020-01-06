using System.Collections.Generic;
using System.IO;
using Capabilities.Util;
using Sim;
using Sim.Organism.Genome;
using Input = Sim.Organism.Genome.Input;

namespace Capabilities
{
    public class MemoryLoadFactory : CapabilityFactory
    {
        private readonly Memory memory;
        public override string HumanReadableName => $"{GetType().Name}";
        
        public MemoryLoadFactory(Memory memory)
        {
            this.memory = memory;
        }
        
        public override Capability Create(string gene, StringReader genome)
        {
            var hr = new HumanReadable(HumanReadableName, gene);
            return new MemoryLoad(hr, memory);
        } 
    }
    
    public class MemoryLoad : Capability
    {
        private readonly Memory memory;
        private const long EnergyCost = 1;

        public MemoryLoad(HumanReadable hr, Memory memory)
        {
            HumanReadable = hr;
            this.memory = memory;
        }

        public override int InputCount
        {
            get { return 0; }
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