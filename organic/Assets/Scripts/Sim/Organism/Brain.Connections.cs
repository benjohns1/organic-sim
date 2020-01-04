using System.Collections.Generic;

namespace Sim.Organism
{
    public partial class Brain
    {
        private class Connections
        {
            private readonly Queue<ulong> inputs = new Queue<ulong>();
            private readonly Queue<ulong> outputs = new Queue<ulong>();

            public ulong Get()
            {
                return inputs.Count > 0 ? inputs.Dequeue() : 0;
            }

            public void Put(ulong val)
            {
                outputs.Enqueue(val);
            }

            public void NextLayer()
            {
                while (outputs.Count > 0)
                {
                    inputs.Enqueue(outputs.Dequeue());
                }

                outputs.Clear();
            }
        }
    }
}