namespace Sim.Organism
{
    public partial class Brain
    {
        private readonly Interaction[][] layers;
        private readonly Connections connections = new Connections();

        public Brain(string genome, GeneFactory geneFactory)
        {
            layers = geneFactory.ParseGenome(genome);
        }

        public ulong Tick(ulong availableEnergy)
        {
            foreach (var layer in layers)
            {
                foreach (var interaction in layer)
                {
                    var input = LoadInput(availableEnergy, interaction);
                    var output = interaction.Run(input);
                    
                    if (output.Energy > 0)
                    {
                        availableEnergy += (ulong) output.Energy;
                    }
                    else
                    {
                        var outputEnergy = (ulong) (-output.Energy);
                        if (availableEnergy <= outputEnergy)
                        {
                            return 0;
                        }
                        availableEnergy -= outputEnergy;
                    }
                    
                    connections.Put(output.Data);
                }

                connections.NextLayer();
            }

            return availableEnergy;
        }

        private Input LoadInput(ulong availableEnergy, Interaction interaction)
        {
            var inputCount = interaction.GetInputCount();
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