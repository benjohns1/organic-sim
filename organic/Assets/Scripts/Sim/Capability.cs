using System.Collections.Generic;
using System.IO;

namespace Sim
{
    public abstract class CapabilityFactory
    {
        public abstract Capability Create(StringReader genome);
    }
    
    public abstract class Capability
    {
        public abstract Output Run(Input input);

        public virtual int GetInputCount()
        {
            return 1;
        }
    }
    
    public struct Input
    {
        public ulong[] Data;
        public ulong AvailableEnergy;
    }

    public struct Output
    {
        public ulong[] Data;
        public long Energy;
    }
}