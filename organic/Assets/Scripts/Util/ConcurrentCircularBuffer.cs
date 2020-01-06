using System.Collections.Generic;
using System.Linq;

namespace Util
{
    public interface ICircularBuffer<T>
    {
        void Clear();
        void Add(T item);
        IEnumerable<T> Read();
    }
    
    public class ConcurrentCircularBuffer<T> : ICircularBuffer<T>
    {
        private readonly LinkedList<T> buffer;
        private readonly int maxSize;

        public ConcurrentCircularBuffer(int maxSize)
        {
            this.maxSize = maxSize;
            buffer = new LinkedList<T>();
        }

        public void Clear()
        {
            lock (buffer)
            {
                buffer.Clear();   
            }
        }

        public void Add(T item)
        {
            lock (buffer)
            {
                buffer.AddFirst(item);
                if (buffer.Count > maxSize)
                {
                    buffer.RemoveLast();
                }
            }
        }

        public IEnumerable<T> Read()
        {
            lock (buffer)
            {
                return buffer.ToArray();
            }
        }
    }
}