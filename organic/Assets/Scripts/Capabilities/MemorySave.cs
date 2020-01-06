using System.Collections.Generic;
using System.IO;
using Capabilities.Util;
using Sim;
using Sim.Organism.Genome;
using Input = Sim.Organism.Genome.Input;

namespace Capabilities
{
    public class MemorySaveFactory : CapabilityFactory
    {
        public override string HumanReadableName => $"{GetType().Name}";
        private readonly Memory memory;
        
        public MemorySaveFactory(Memory memory)
        {
            this.memory = memory;
        }
        
        public override Capability Create(string gene, StringReader genome)
        {
            var hr = new HumanReadable(HumanReadableName, gene);
            return new MemorySave(hr, memory);
        } 
    }
    
    public class MemorySave : Capability
    {
        private readonly Memory memory;
        private const long EnergyCost = 1;

        public MemorySave(HumanReadable hr, Memory memory)
        {
            HumanReadable = hr;
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