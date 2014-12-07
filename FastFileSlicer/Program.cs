using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace FastFileSlicer
{
    class MainClass
    {
        private const int ReportLineCount = 100000;

        private static long LineCounter;
        private static long TotalLineCounter;
        private static string directoryBasePath;
        private static string fileExtension;

        public static void Main (string[] args)
        {
            Stopwatch clock = Stopwatch.StartNew();

            Console.WriteLine("{0} start processing", DateTime.Now.ToString("yyyyMMdd HH:mm:ss"));

            try
            {
                string fileName = ReadFileName(args);
                string algorithmType = ReadAlgorithm(args);
                int bufferSize = ReadBufferSize(args);

                Console.WriteLine("File to slice:\t {0}", fileName);
                Console.WriteLine("Algorithm type:\t {0}", algorithmType);
                Console.WriteLine("Buffer size:\t   {0}", bufferSize);

                if (string.IsNullOrWhiteSpace(algorithmType) || algorithmType == "simple")
                {
                    ProcessBySimpleReader(fileName);
                }
                else
                {
                    ProcessByFilePortionReader(fileName, bufferSize);
                }
            }
            catch(System.Exception ex) 
            {
                Console.WriteLine (ex.ToString ());
            }
            finally 
            {
                Console.WriteLine("{0} end processing in {1} ms", DateTime.Now.ToString("yyyyMMdd HH:mm:ss"), clock.ElapsedMilliseconds);
            }
        }

        private static string ReadFileName (string[] args)
        {
            return args [0] as string;
        }

        private static string ReadAlgorithm(string[] args)
        {
            return args.Length > 1 ? args[1] as string : string.Empty;
        }

        private static int ReadBufferSize(string[] args)
        {
            return args.Length > 2 ? Convert.ToInt32(args[2]) : 1024;
        }

        private static void ProcessByFilePortionReader(string fileName, int bufferSize)
        {
            Console.WriteLine("Processing by file portion reader");

            FileStreamPositionManager fileStreamSeekManager = new FileStreamPositionManager(0);
            FilePortionReader fpr = new FilePortionReader(fileName, bufferSize, fileStreamSeekManager, '\t');
            fpr.Slice();
        }

        private static void ProcessBySimpleReader(string fileName)
        {
            Console.WriteLine("Processing by simple reader");

            fileExtension = Path.GetExtension(fileName);
            directoryBasePath = Path.GetDirectoryName(fileName);

            foreach (string line in File.ReadLines(fileName))
            {
                ProcessLine(line);
                if (LineCounter == ReportLineCount)
                {
                    Console.WriteLine("{0} processed lines {1}", DateTime.Now.ToString("yyyyMMdd HH:mm:ss"), TotalLineCounter);
                    LineCounter = 0;
                }
            }
        }

        private static void ProcessLine(string line)
        {
            Interlocked.Increment(ref LineCounter);
            Interlocked.Increment(ref TotalLineCounter);

            string fileNameFromLine = Path.Combine(directoryBasePath, string.Concat(ReadFileNameFromLine (line), fileExtension));

            using (FileStream fs = new FileStream(fileNameFromLine, FileMode.Append, FileAccess.Write, FileShare.ReadWrite)) 
            {
                using (StreamWriter sw = new StreamWriter(fs)) 
                {
                    sw.Write (line);
                    sw.Write (Environment.NewLine);
                }
            }
        }

        static string ReadFileNameFromLine (string line)
        {
            StringBuilder buildFileName = new StringBuilder (32);
            for (int i = 0; i < line.Length; i++) 
            {
                char c = line [i];
                if (c == '\t') 
                {
                    return buildFileName.ToString ();
                }
                buildFileName.Append (c);
            }

            throw new InvalidOperationException (string.Format ("Cannot read file name from the line: {0}", line));
        }
    }
}
