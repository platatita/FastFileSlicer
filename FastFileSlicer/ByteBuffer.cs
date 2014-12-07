using System;
using System.Collections.Generic;
using System.Text;

namespace FastFileSlicer
{
    internal class ByteBuffer
    {
        private readonly List<Tuple<int, int, byte[]>> byteColleciton;

        public ByteBuffer(int capacity)
        {
            this.Capacity = capacity;
            this.byteColleciton = new List<Tuple<int, int, byte[]>>();
        }

        public int Count { get { return this.byteColleciton.Count; } }
        public int Capacity { get; private set; }
        
        public void Clear()
        {
            this.byteColleciton.Clear();
        }

        public void AddRange(byte[] buffer, int start, int length)
        {
            if (length <= 0)
            {
                return;
            }

            this.byteColleciton.Add(new Tuple<int, int, byte[]>(start, length, buffer));
        }

        public List<Tuple<int, int, byte[]>> ToArray()
        {
            return this.byteColleciton;
        }

        public override string ToString()
        {
            return string.Format("[ByteBuffer: Count={0}, Capacity={1}]", 
                Count, 
                Capacity);
        }
    }
}

