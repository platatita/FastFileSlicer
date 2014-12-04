using System;
using System.IO;

namespace FastFileSlicer
{
    internal class FileStreamSeekManager : FileReaderBase
    {
        private const int BytesToRead = 16;
        private readonly long startByte;

        public FileStreamSeekManager(long startByte)
        {
            this.startByte = startByte;
        }

        public bool Seek(FileStream fileStream)
        {
            bool seekResult = false;
            
            if (startByte > 0 && this.startByte < fileStream.Length)
            {
                SeekFileStreamToStartByte(fileStream);

                seekResult = SeekFileStreamToNextLine(fileStream);
            }

            return seekResult;
        }

        private void SeekFileStreamToStartByte(FileStream fileStream)
        {
            fileStream.Seek(startByte, SeekOrigin.Begin);
        }

        private bool SeekFileStreamToNextLine(FileStream fileStream)
        {
            do
            {
                int readBytes = base.Read(fileStream, BytesToRead);
                if (readBytes <= 0)
                {
                    break;
                }

                int findCharIndex = base.FindChar(Environment.NewLine[0], readBytes);
                if (findCharIndex > 0)
                {
                    long newPosition = findCharIndex - readBytes;
                    fileStream.Seek(newPosition, SeekOrigin.Current);
                    return true;
                }

            } while(true);

            return false;
        }
    }
}

