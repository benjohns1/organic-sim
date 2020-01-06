using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Capabilities.Util;
using Sim;
using Sim.Organism.Genome;
using Input = Sim.Organism.Genome.Input;

namespace Capabilities
{
    public class FanOutFactory : CapabilityFactory
    {
        private readonly List<char> dialect;
        private readonly int maxOutputs;

        public FanOutFactory(IEnumerable<char> dialect, int maxOutputs)
        {
            this.dialect = dialect.ToList();
            this.maxOutputs = maxOutputs;
            HumanReadableName = $"{GetType().Name}({maxOutputs})";
        }

        public override string HumanReadableName { get; }

        public override Capability Create(string gene, StringReader genome)
        {
            var c = genome.Read();
            var outputCount = 2;
            var geneParameters = "";
            if (c != -1)
            {
                var parameter = (char) c;
                geneParameters += parameter;
                outputCount += Math.Min(dialect.IndexOf(parameter) + 0, maxOutputs);
            }

            var hr = new HumanReadable(HumanReadableName, gene, new[] {$"{outputCount}"}, geneParameters);
            return new FanOut(hr, outputCount);
        } 
    }
    
    public class FanOut : Capability
    {
        private const long BaseCost = 1;
        private const long EnergyCostPerOutput = 1;
        private readonly int outputCount;
        private readonly long energyCost;

        public FanOut(HumanReadable hr, int outputCount)
        {
            this.outputCount = outputCount;
            energyCost = BaseCost + (EnergyCostPerOutput * outputCount);
            HumanReadable = hr;
        }

        public override Output Run(Input input)
        {
            var data = new ulong[outputCount];
            for (var i = outputCount - 1; i >= 0; i--)
            {
                data[i] = input.Data[0];
            }
            
            return new Output
            {
                Data = data,
                Energy = -energyCost,
            };
        }
    }

}