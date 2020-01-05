using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Capabilities.Util;
using Sim;
using Input = Sim.Input;

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
        }

        public override Capability Create(StringReader genome)
        {
            var c = genome.Read();
            var outputCount = 2;
            if (c != -1)
            {
                outputCount += Math.Min(dialect.IndexOf((char) c) + 0, maxOutputs);
            }
            
            return new FanOut(outputCount);
        } 
    }
    
    public class FanOut : Capability
    {
        private const long BaseCost = 1;
        private const long EnergyCostPerOutput = 1;
        private readonly int outputCount;
        private readonly long energyCost;

        public FanOut(int outputCount)
        {
            this.outputCount = outputCount;
            energyCost = BaseCost + (EnergyCostPerOutput * outputCount);
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