using System;
using System.IO;

namespace FastFileSlicer
{
    internal abstract class FileReaderBase
    {
        protected byte[] buffer;

        protected int Read(FileStream fileStream, int byteCount)
        {
            this.buffer = new byte[byteCount];

            return fileStream.Read(this.buffer, 0, this.buffer.Length);
        }

        protected int FindChar(char charToFind, int readBytes, int startBufferIndex = 0)
        {
            for (int i = startBufferIndex; i < readBytes; i++)
            {
                char c = (char)this.buffer[i];
                if (c == charToFind)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}

