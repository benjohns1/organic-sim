using System.Collections.Generic;
using System.IO;
using Sim;
using Sim.Organism.Genome;
using Input = Sim.Organism.Genome.Input;

namespace Capabilities
{
    public class InvertFactory : CapabilityFactory
    {   
        public override string HumanReadableName => $"{GetType().Name}";
        public override Capability Create(string gene, StringReader genome)
        {
            var hr = new HumanReadable(HumanReadableName, gene);
            return new Invert(hr);
        }
    }
    
    public class Invert : Capability
    {
        public Invert(HumanReadable hr)
        {
            HumanReadable = hr;
        }

        public override Output Run(Input input)
        {
            return new Output
            {
                Data = new []{ulong.MaxValue - input.Data[0]},
                Energy = -1,
            };
        }
    }

}