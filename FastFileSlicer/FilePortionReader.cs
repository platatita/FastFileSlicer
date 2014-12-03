using System;
using System.IO;
using System.Text;

namespace FastFileSlicer
{
    internal class FilePortionReader : FileReaderBase
    {
        private readonly string fullFileNamePath;
        private readonly long startByte;
        private readonly long endByte;
        private readonly char columnSeparator;
        private byte[] buffer;

        public FilePortionReader (string fullFileNamePath, long startByte, long endByte, char columnSeparator)
        {
            this.fullFileNamePath = fullFileNamePath;
            this.startByte = startByte;
            this.endByte = endByte;
            this.columnSeparator = columnSeparator;
        }

        public void Read()
        {
            using (FileStream fs = new FileStream(this.fullFileNamePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                SeekFileStream(fs);
                StartSlicing(fs);
            }
        }

        private void SeekFileStream(FileStream fileStream)
        {
            FileStreamSeekManager fssm = new FileStreamSeekManager(fileStream, this.startByte);
            fssm.Seek();
        }

        private void StartSlicing(FileStream fileStream)
        {
            bool stopSearching = false;
            int bytesToRead = 8192;
            int startBufferIndex = 0;

            do
            {
                int readBytes = Read(fileStream, bytesToRead);
                string fileName = GetSliceFileName(startBufferIndex, readBytes);
//                if (findCharIndex > 0)
//                {
//                    startBufferIndex = findCharIndex;
//                }

            } while(stopSearching);
        }

        private string GetSliceFileName(int startBufferIndex, int readBytes)
        {
            int findCharIndex = FindChar(this.columnSeparator, readBytes, startBufferIndex);
            if (findCharIndex > 0)
            {
                return Encoding.UTF8.GetString(this.buffer, startBufferIndex, findCharIndex - startBufferIndex);
            }

            return null;
        }
    }
}

