using System;
using System.Collections.Generic;
using System.Text;

namespace FastFileSlicer
{
    internal class ByteBuffer
    {
        private readonly List<byte> byteColleciton;

        public ByteBuffer(int capacity)
        {
            this.Capacity = capacity;
            this.byteColleciton = new List<byte>(capacity);
        }

        public int Count { get; private set; }
        public int Capacity { get; private set; }

        public void Clear()
        {
            this.byteColleciton.Clear();
            this.Count = 0;
        }

        public void AddRange(byte[] buffer, int start, int length)
        {
            for (int i = start; i < start + length; i++)
            {
                this.byteColleciton.Add(buffer[i]);
                this.Count++;
            }
        }

        public byte[] ToArray()
        {
            return this.byteColleciton.ToArray();
        }

        public override string ToString()
        {
            return string.Format("[ByteBuffer: Count={0}, Capacity={1}, AsText='{2}']", 
                Count, 
                Capacity,
                ConvertToString());
        }

        private string ConvertToString()
        {
            return Encoding.UTF8.GetString(ToArray(), 0, Count);
        }
    }
}

