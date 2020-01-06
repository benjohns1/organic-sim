using System.IO;

namespace Sim.Organism.Genome
{
    public abstract class CapabilityFactory
    {
        public abstract string HumanReadableName { get; }
        
        public abstract Capability Create(string gene, StringReader genome);
    }

    public abstract class Capability
    {
        public abstract Output Run(Input input);

        public virtual int InputCount => 1;
        
        public HumanReadable HumanReadable { get; protected set; }
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