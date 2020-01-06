using System.Collections.Generic;
using System.IO;
using Sim;
using Sim.Organism.Genome;
using Input = Sim.Organism.Genome.Input;

namespace Capabilities
{
    public class HaltFactory : CapabilityFactory
    {
        public override string HumanReadableName => $"{GetType().Name}";
        public override Capability Create(string gene, StringReader genome)
        {
            var hr = new HumanReadable(HumanReadableName, gene);
            return new Halt(hr);
        }
    }
    
    public class Halt : Capability
    {
        public Halt(HumanReadable hr)
        {
            HumanReadable = hr;
        }

        public override Output Run(Input input)
        {
            return new Output
            {
                Data = new ulong[]{},
                Energy = -1,
            };
        }
    }

}