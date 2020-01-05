using System.Collections.Generic;
using System.IO;
using Sim;
using Input = Sim.Input;

namespace Capabilities
{
    public class StaticFactory : CapabilityFactory
    {
        private readonly ulong value;

        public StaticFactory(ulong value)
        {
            this.value = value;
        }
        
        public override Capability Create(StringReader genome)
        {
            return new Static(value);
        }
    }
    
    public class Static : Capability
    {
        private readonly ulong value;
        
        public Static(ulong value)
        {
            this.value = value;
        }
        
        public override int GetInputCount()
        {
            return 0;
        }
        
        public override Output Run(Input input)
        {
            return new Output
            {
                Data = new []{value},
                Energy = -1,
            };
        }
    }

}