using System.Collections.Generic;
using System.IO;
using Sim;
using Input = Sim.Input;

namespace Capabilities
{
    public class HaltFactory : CapabilityFactory
    {
        public override Capability Create(StringReader genome)
        {
            return new Halt();
        }
    }
    
    public class Halt : Capability
    {   
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