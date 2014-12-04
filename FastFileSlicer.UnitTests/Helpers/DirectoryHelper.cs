using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace FastFileSlicer.UnitTests
{
    public class DirectoryHelper
    {
        public static void DeleteFilesFromTopDirectory(string directoryPath, string searchPattern, params string[] excludedFileNames)
        {
            foreach (string file in GetFilePathsFromTopDirectory(directoryPath, searchPattern))
            {
                if (!excludedFileNames.Contains(Path.GetFileName(file), StringComparer.InvariantCultureIgnoreCase))
                {
                    File.Delete(file);
                }
            }
        }

        private static List<string> GetFilePathsFromTopDirectory(string directoryPath, string searchPattern)
        {
            return Directory.GetFiles(directoryPath, searchPattern, SearchOption.TopDirectoryOnly).ToList();
        }

        public static List<string> GetFileNamesFromTopDirectory(string directoryPath, string searchPattern, params string[] excludedFileNames)
        {
            return Directory.GetFiles(directoryPath, searchPattern, SearchOption.TopDirectoryOnly)
                .Select(f => Path.GetFileName(f))
                .Except(excludedFileNames)
                .ToList();
        }

        public static void CompareFiles(string expectedDir, string actualDir, params string[] excludedFileNames)
        {
            var expectedFiles = GetFilePathsFromTopDirectory(expectedDir, "*.*");
            var actualFiles = GetFilePathsFromTopDirectory(actualDir, "*.*");
            var actualFilesCount = actualFiles.Count - excludedFileNames.Length;
            Assert.AreEqual(expectedFiles.Count, actualFilesCount, "Number of files in expected dir and actual dir differs");

            foreach (string actualFilePath in actualFiles)
            {
                var actualFileName = Path.GetFileName(actualFilePath);

                if (excludedFileNames.Contains(actualFileName, StringComparer.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                var expectedFilePath = expectedFiles.Find(f => Path.GetFileName(f).Equals(actualFileName, StringComparison.InvariantCultureIgnoreCase));
                Assert.IsNotNull(expectedFilePath, "Could not find actual file '{0}' in expected", actualFileName);

                CollectionAssert.AreEqual(
                    File.ReadLines(expectedFilePath), 
                    File.ReadLines(actualFilePath), 
                    "Comparison of expected '{0}' to actual '{1}' failed", 
                    expectedFilePath, 
                    actualFilePath);
            }
        }
    }
}

