using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace FastFileSlicer.UnitTests
{
    [TestFixture]
    public class FileStreamPositionManagerTests
    {
        private const string TestDirectory = "FileStreamSeekManagerTests";
        private const string TestFilePath = "FileStreamSeekManagerTests/test_file.txt";
        private FileStream fileStream;
        private FastFileSlicer.StreamReader streamReader;
        private FileStreamPositionManager target;

        [SetUp]
        public void Before_Each_Test()
        {
            Directory.CreateDirectory(TestDirectory);
            this.fileStream = new FileStream(TestFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
            this.streamReader = new StreamReader(fileStream, 1024);
        }

        [TearDown]
        public void After_Each_Test()
        {
            this.streamReader.Dispose();
            Directory.Delete(TestDirectory, true);
        }

        [Test]
        public void Does_Not_Change_FileStream_Position_When_StartByte_Equals_To_Zero()
        {
            long startByte = 0;
            this.target = new FileStreamPositionManager(startByte);
            var actual = this.target.Seek(this.streamReader);

            Assert.IsFalse(actual);
            Assert.AreEqual(0, this.streamReader.Position);
        }

        [Test]
        public void Does_Not_Change_FileStream_Position_When_File_Is_Empty()
        {
            long startByte = 100;
            this.target = new FileStreamPositionManager(startByte);
            var actual = this.target.Seek(this.streamReader);

            Assert.IsFalse(actual);
            Assert.AreEqual(0, this.streamReader.Position);
        }

        [Test]
        public void Seeks_FileStream_Position_To_Begin_Of_Second_Line_When_StartByte_Equals_To_7_In_First_Line()
        {
            WriteLines(5);

            long startByte = 7;
            this.target = new FileStreamPositionManager(startByte);
            var actual = this.target.Seek(this.streamReader);

            Assert.IsTrue(actual);
            Assert.AreEqual(19, this.streamReader.Position);
        }

        [Test]
        public void Seeks_FileStream_Position_To_Begin_Of_Fifth_Line_When_StartByte_Equals_To_65_In_Fourth_Line()
        {
            WriteLines(5);

            long startByte = 65;
            this.target = new FileStreamPositionManager(startByte);
            var actual = this.target.Seek(this.streamReader);

            Assert.IsTrue(actual);
            Assert.AreEqual(79, this.streamReader.Position);
        }

        [Test]
        public void Seeks_FileStream_Position_To_End_Of_File_When_StartByte_Equals_To_98_In_Last_Line()
        {
            WriteLines(5);

            long startByte = 95;
            this.target = new FileStreamPositionManager(startByte);
            var actual = this.target.Seek(this.streamReader);

            Assert.IsTrue(actual);
            Assert.AreEqual(99, this.streamReader.Position);
        }

        private void WriteLines(int lineCount)
        {
            StreamWriter sw = new StreamWriter(this.fileStream);

            for (int i = 0; i < lineCount; i++)
            {
                sw.WriteLine(string.Format("It is the line no {0}", i));
            }

            sw.Flush();
        }
    }
}

