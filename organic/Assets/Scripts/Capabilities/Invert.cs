using System.Collections.Generic;
using System.IO;
using Sim;
using Input = Sim.Input;

namespace Capabilities
{
    public class InvertFactory : CapabilityFactory
    {   
        public override Capability Create(StringReader genome)
        {
            return new Invert();
        }
    }
    
    public class Invert : Capability
    {   
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