using System.Collections.Generic;
using System.Linq;
using Sim.Organism.Genome;

namespace Sim.Organism
{
    public partial class Brain
    {
        private readonly IEnumerable<Capability> capabilities;
        private readonly Connections connections = new Connections();

        public struct Out
        {
            public ulong RemainingEnergy;
            public IEnumerable<string> Debug;
        }

        public Brain(string genome, Factory geneFactory)
        {
            capabilities = geneFactory.ParseGenome(genome);
        }

        public Out Tick(ulong availableEnergy)
        {
            connections.Clear();
            var debug = new List<string>();
            foreach (var capability in capabilities)
            {
                var input = LoadInput(availableEnergy, capability);
                var output = capability.Run(input);
                connections.Put(output.Data);
                debug.Add($"{capability.GetType().Name}({string.Join(", ", input.Data.Select(d => (float) d / ulong.MaxValue))}) -> ({string.Join(", ", output.Data.Select(d => (float) d / ulong.MaxValue))})   - {connections}");
                
                if (output.Energy > 0)
                {
                    availableEnergy += (ulong) output.Energy;
                }
                else
                {
                    var outputEnergy = (ulong) (-output.Energy);
                    if (availableEnergy <= outputEnergy)
                    {
                        return new Out
                        {
                            RemainingEnergy = 0,
                            Debug = debug,
                        };
                    }
                    availableEnergy -= outputEnergy;
                }
            }

            return new Out
            {
                RemainingEnergy = availableEnergy,
                Debug = debug,
            };
        }

        private Input LoadInput(ulong availableEnergy, Capability capability)
        {
            var inputCount = capability.GetInputCount();
            var input = new Input
            {
                Data = new ulong[inputCount],
                AvailableEnergy = availableEnergy,
            };
            for (var i = 0; i < inputCount; i++)
            {
                input.Data[i] = connections.Get();
            }

            return input;
        }
    }
}