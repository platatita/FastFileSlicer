using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace FastFileSlicer.UnitTests
{
    public class FilePortionReaderTests
    {
        protected const string TestBaseDirectory = "Integrations";
        protected const string TestFile = "file_to_slice.txt";
        private const char ColumnSeparator = '\t';

        public class Test1
        {
            private static readonly string TestDirectory = Path.Combine(TestBaseDirectory, "Test1");
            private static readonly string ExpectedDirectory = Path.Combine(TestBaseDirectory, "Test1", "Expected");
            private static readonly string TestFilePath = Path.Combine(TestBaseDirectory, "Test1", TestFile);

            [TestFixture]
            public class Buffer_1024
            {
                [SetUp]
                public void Before_Each_Test()
                {
                    DirectoryHelper.DeleteFilesFromTopDirectory(TestDirectory, "*.*", TestFile);
                }

                [Test]
                public void Creates_One_File_When_SliceFile_Has_Only_One_Line()
                {
                    long startByte = 0;
                    int bufferSize = 1024;

                    FileStreamPositionManager fileStreamSeekManager = new FileStreamPositionManager(startByte);
                    FilePortionReader target = new FilePortionReader(TestFilePath, bufferSize, fileStreamSeekManager, ColumnSeparator);
                    target.Slice();

                    DirectoryHelper.CompareFiles(ExpectedDirectory, TestDirectory, TestFile);
                }
            }

            [TestFixture]
            public class Buffer_16
            {
                [SetUp]
                public void Before_Each_Test()
                {
                    DirectoryHelper.DeleteFilesFromTopDirectory(TestDirectory, "*.*", TestFile);
                }

                [Test]
                public void Creates_One_File_When_SliceFile_Has_Only_One_Line()
                {
                    long startByte = 0;
                    int bufferSize = 16;

                    FileStreamPositionManager fileStreamSeekManager = new FileStreamPositionManager(startByte);
                    FilePortionReader target = new FilePortionReader(TestFilePath, bufferSize, fileStreamSeekManager, ColumnSeparator);
                    target.Slice();

                    DirectoryHelper.CompareFiles(ExpectedDirectory, TestDirectory, TestFile);
                }
            }
        }

        [TestFixture]
        public class Test2
        {
            private static readonly string TestDirectory = Path.Combine(TestBaseDirectory, "Test2");
            private static readonly string ExpectedDirectory = Path.Combine(TestBaseDirectory, "Test2", "Expected");
            private static readonly string TestFilePath = Path.Combine(TestBaseDirectory, "Test2", TestFile);

            private FilePortionReader target;

            [SetUp]
            public void Before_Each_Test()
            {
                DirectoryHelper.DeleteFilesFromTopDirectory(TestDirectory, "*.*", TestFile);
            }

            [Test]
            public void Creates_Two_Files_When_SliceFile_Has_Five_Lines_But_Only_Two_Differen_Values_In_First_Column()
            {
                long startByte = 0;
                int bufferSize = 1024;

                FileStreamPositionManager fileStreamSeekManager = new FileStreamPositionManager(startByte);
                this.target = new FilePortionReader(TestFilePath, bufferSize, fileStreamSeekManager, ColumnSeparator);
                this.target.Slice();

                DirectoryHelper.CompareFiles(ExpectedDirectory, TestDirectory, TestFile);
            }
        }
    }
}

