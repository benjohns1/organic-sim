using System;
using System.IO;
using Sim.Organism.Genome;
using Input = Sim.Organism.Genome.Input;

namespace Capabilities
{
    public class StaticFactory : CapabilityFactory
    {
        private readonly ulong value;

        public StaticFactory(ulong value)
        {
            this.value = value;
            HumanReadableName = $"{GetType().Name}({Util.Convert.ScaledFloat(value)})";
        }

        public override string HumanReadableName { get; }

        public override Capability Create(string gene, StringReader genome)
        {
            var hr = new HumanReadable(HumanReadableName, gene);
            return new Static(hr, value);
        }
    }
    
    public class Static : Capability
    {
        private readonly ulong value;
        
        public Static(HumanReadable hr, ulong value)
        {
            HumanReadable = hr;
            this.value = value;
        }

        public override int InputCount => 0;

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