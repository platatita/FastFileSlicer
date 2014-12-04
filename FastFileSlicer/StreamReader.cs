using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FastFileSlicer
{
    internal class StreamReader : IDisposable
    {
        private const int NotFoundIndex = -1;

        private readonly Stream stream;
        private readonly byte[] buffer;

        private ByteBuffer tmpByteBuffer;

        internal int StartBufferIndex { get; private set; }
        internal int ReadBytes { get; private set; }

        internal long Position { get { return this.stream.Position; } }
        internal long StreamLength { get { return this.stream.Length; } }

        internal StreamReader(Stream stream, int bufferSize)
        {
            this.stream = stream;
            this.buffer = new byte[bufferSize];
            this.tmpByteBuffer = new ByteBuffer(bufferSize * 2);
        }

        #region IDisposable implementation

        /// <summary>
        /// Invokes the <see cref="Stream.Dispose"/> method on the provided <see cref="Stream"/> in the constructor.
        /// </summary>
        public void Dispose()
        {
            this.stream.Dispose();
        }

        #endregion

        internal long Seek(long offset, SeekOrigin seekOrigin)
        {
            var result = this.stream.Seek(offset, seekOrigin);

            this.ReadBytes = 0;
            this.StartBufferIndex = 0;
            this.tmpByteBuffer.Clear();

            return result;
        }

        internal int FindChar(char charToFind, bool includeCharToFind = false)
        {
            if (this.ReadBytes == 0)
            {
                ReadFromStream();
            }

            for (int i = this.StartBufferIndex; i < this.ReadBytes; i++)
            {
                char c = (char)this.buffer[i];
                if (c == charToFind)
                {
                    return this.StartBufferIndex = includeCharToFind ? i + 1 : i;
                }
            }

            return NotFoundIndex;
        }

        internal int[] FindString(string textToFind)
        {
            int[] charIndexs = new int[textToFind.Length];

            for (int i = 0; i < textToFind.Length; i++)
            {
                char c = textToFind[i];

                int index = FindChar(c);
                if (index < 0)
                {
                    charIndexs[i] = index;
                    return charIndexs;
                }
                if (i > 0 && charIndexs[i - 1] != index - 1)
                {
                    this.StartBufferIndex = index - i;
                    i = -1;
                    continue;
                }

                charIndexs[i] = index;
            }

            return charIndexs;
        }

        internal Tuple<int, int, byte[]> ReadBytesToChar(char charToFind, bool includeCharToFind = false)
        {
            this.tmpByteBuffer.Clear();
            var orgStartBufferIndex = this.StartBufferIndex;
            var index = 0;

            do
            {
                index = FindChar(charToFind, includeCharToFind);
                if (index == NotFoundIndex)
                {
                    this.tmpByteBuffer.AddRange(this.buffer, orgStartBufferIndex, this.ReadBytes - orgStartBufferIndex);
                    ReadFromStream();
                    orgStartBufferIndex = this.StartBufferIndex;
                }

            } while(index == NotFoundIndex && this.ReadBytes > 0);

            if (this.tmpByteBuffer.Count > 0)
            {
                if (index > 0)
                {
                    this.tmpByteBuffer.AddRange(this.buffer, 0, index);
                }

                return new Tuple<int, int, byte[]>(0, this.tmpByteBuffer.Count, this.tmpByteBuffer.ToArray());
            }
            else
            {
                return new Tuple<int, int, byte[]>(orgStartBufferIndex, index - orgStartBufferIndex, this.buffer);
            }
        }

        private int ReadFromStream()
        {
            this.StartBufferIndex = 0;
            return this.ReadBytes = this.stream.Read(this.buffer, 0, this.buffer.Length);
        }
    }
}

