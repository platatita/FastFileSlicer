using System;

namespace FastFileSlicer
{
    internal class SliceFileNameManager
    {
        private readonly string columnSeparator;

        public SliceFileNameManager(string columnSeparator)
        {
            this.columnSeparator = columnSeparator;
        }

        public bool Create(int startBufferIndex, int readBytes)
        {
            return false;
        }
    }
}

