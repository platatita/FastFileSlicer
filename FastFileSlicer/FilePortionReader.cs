using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace FastFileSlicer
{
    internal class FilePortionReader
    {
        private readonly string fullFileNamePath;
        private readonly int bufferSize;
        private readonly FileStreamPositionManager fileStreamPositionManager;
        private readonly char columnSeparator;
        private readonly string directoryBasePath;
        private readonly string fileExtension;
        private StreamReader streamReader;

        public FilePortionReader (string fullFileNamePath, int bufferSize, FileStreamPositionManager fileStreamSeekManager, char columnSeparator)
        {
            this.fullFileNamePath = fullFileNamePath;
            this.bufferSize = bufferSize;
            this.fileExtension = Path.GetExtension(fullFileNamePath);
            this.directoryBasePath = Path.GetDirectoryName(fullFileNamePath);
            this.fileStreamPositionManager = fileStreamSeekManager;
            this.columnSeparator = columnSeparator;
        }

        public void Slice()
        {
            using (FileStream fs = new FileStream(this.fullFileNamePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                CreateStreamReader(fs);
                SeekFileStream();
                StartSlicing();
            }
        }

        private void CreateStreamReader(FileStream fileStream)
        {
            this.streamReader = new StreamReader(fileStream, this.bufferSize);
        }

        private void SeekFileStream()
        {
            this.fileStreamPositionManager.Seek(this.streamReader);
        }

        private void StartSlicing()
        {
            do
            {
                Tuple<int, int, byte[]> fileNameBuffer = ReadFileNameBuffer();
                if (fileNameBuffer.Item2 <= 0)
                {
                    break;
                }

                string fileName = GetSliceFileName(fileNameBuffer);
                StoreDataIntoFile(fileName, fileNameBuffer);

                Tuple<int, int, byte[]> dataBuffer = ReadDataBufferToEndOfLine();
                StoreDataIntoFile(fileName, dataBuffer);

            } while(this.streamReader.ReadBytes > 0);
        }

        private Tuple<int, int, byte[]> ReadFileNameBuffer()
        {
            return this.streamReader.ReadBytesToChar(this.columnSeparator);
        }

        private string GetSliceFileName(Tuple<int, int, byte[]> fileNameBuffer)
        {
            var fileName = Encoding.UTF8.GetString(fileNameBuffer.Item3, fileNameBuffer.Item1, fileNameBuffer.Item2);

            return Path.Combine(this.directoryBasePath, string.Concat(fileName, this.fileExtension));
        }

        private Tuple<int, int, byte[]> ReadDataBufferToEndOfLine()
        {
            return this.streamReader.ReadBytesToChar(Environment.NewLine[0], true);
        }

        private void StoreDataIntoFile(string fileName, Tuple<int, int, byte[]> data)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.Write))
            {
                fs.Write(data.Item3, data.Item1, data.Item2);
                fs.Flush();
            }
        }
    }
}

