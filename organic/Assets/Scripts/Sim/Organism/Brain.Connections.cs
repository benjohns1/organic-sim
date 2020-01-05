using System.Collections.Generic;
using System.Linq;

namespace Sim.Organism
{
    public partial class Brain
    {
        private class Connections
        {
            private readonly Queue<ulong> queue = new Queue<ulong>();

            public void Clear()
            {
                queue.Clear();
            }

            public ulong Get()
            {
                return queue.Count > 0 ? queue.Dequeue() : 0;
            }

            public void Put(IEnumerable<ulong> values)
            {
                foreach (var value in values)
                {
                    queue.Enqueue(value);
                }
            }

            public override string ToString()
            {
                return queue.Count > 0
                    ? $"[{string.Join(",", queue.Select(i => (float) i / ulong.MaxValue))}]"
                    : "";
            }
        }
    }
}