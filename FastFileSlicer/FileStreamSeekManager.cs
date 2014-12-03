using System;
using System.IO;

namespace FastFileSlicer
{
    internal class FileStreamSeekManager : FileReaderBase
    {
        private const int BytesToRead = 16;
        private readonly FileStream fileStream;
        private readonly long startByte;
        private bool seekResult;

        public FileStreamSeekManager(FileStream fileStream, long startByte)
        {
            this.fileStream = fileStream;
            this.startByte = startByte;
        }

        public bool Seek()
        {
            this.seekResult = false;
            
            if (startByte > 0 && this.startByte < this.fileStream.Length)
            {
                SeekFileStreamToStartByte();
                SeekFileStreamToNextLine();
            }

            return this.seekResult;
        }

        private void SeekFileStreamToStartByte()
        {
            this.fileStream.Seek(startByte, SeekOrigin.Begin);
        }

        private void SeekFileStreamToNextLine()
        {
            do
            {
                int readBytes = base.Read(this.fileStream, BytesToRead);
                if (readBytes <= 0)
                {
                    break;
                }

                int findCharIndex = base.FindChar(Environment.NewLine[0], readBytes);
                if (findCharIndex > 0)
                {
                    long newPosition = findCharIndex - readBytes;
                    this.fileStream.Seek(newPosition, SeekOrigin.Current);
                    this.seekResult = true;
                    break;
                }

            } while(true);
        }
    }
}

