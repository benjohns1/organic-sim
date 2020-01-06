using System.Collections.Generic;
using System.IO;
using Capabilities.Util;
using Sim;
using Sim.Organism.Genome;
using Input = Sim.Organism.Genome.Input;

namespace Capabilities
{
    public class DecideFactory : CapabilityFactory
    {
        public override string HumanReadableName => $"{GetType().Name}";

        public override Capability Create(string gene, StringReader genome)
        {
            var hr = new HumanReadable(HumanReadableName, gene);
            return new Decide(hr);
        }
    }
    
    public class Decide : Capability
    {
        private const long EnergyCost = 1;

        public override int InputCount => 2;

        public Decide(HumanReadable hr)
        {
            HumanReadable = hr;
        }

        public override Output Run(Input input)
        {
            // Return the first input value if the second input evaluates to true
            return new Output
            {
                Data = new []{Convert.Bool(input.Data[1]) ? input.Data[0] : 0},
                Energy = -EnergyCost,
            };
        }
    }

}