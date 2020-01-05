using System.Collections.Generic;
using System.IO;
using Capabilities.Util;
using Sim;
using Input = Sim.Input;

namespace Capabilities
{
    public class DecideFactory : CapabilityFactory
    {
        public override Capability Create(StringReader genome)
        {
            return new Decide();
        } 
    }
    
    public class Decide : Capability
    {
        private const long EnergyCost = 1;

        public override int GetInputCount()
        {
            return 2;
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