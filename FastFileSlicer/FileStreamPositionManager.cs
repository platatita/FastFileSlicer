using System;
using System.IO;

namespace FastFileSlicer
{
    internal class FileStreamPositionManager
    {
        private readonly long startByte;

        public FileStreamPositionManager(long startByte)
        {
            this.startByte = startByte;
        }

        public bool Seek(StreamReader streamReader)
        {
            bool seekResult = false;
            
            if (startByte > 0 && this.startByte < streamReader.StreamLength)
            {
                SeekStreamToStartByte(streamReader);

                seekResult = SeekStreamToNextLine(streamReader);
            }

            return seekResult;
        }

        private void SeekStreamToStartByte(StreamReader streamReader)
        {
            streamReader.Seek(startByte, SeekOrigin.Begin);
        }

        private bool SeekStreamToNextLine(StreamReader streamReader)
        {
            long orgPosition = streamReader.Position;
            int findCharIndex = streamReader.FindChar(Environment.NewLine[0]);
            if (findCharIndex > 0)
            {
                streamReader.Seek(orgPosition + findCharIndex, SeekOrigin.Begin);
                return true;
            }

            return false;
        }
    }
}

