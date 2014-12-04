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

        public static void Main (string[] args)
        {
            Stopwatch clock = Stopwatch.StartNew();

            Console.WriteLine("{0} start processing", DateTime.Now.ToString("yyyyMMdd HH:mm:ss"));

            try
            {
                string fileName = ReadFileName (args);
                FileStreamSeekManager fileStreamSeekManager = new FileStreamSeekManager(0);
                FilePortionReader fpr = new FilePortionReader(fileName, fileStreamSeekManager, -1, '\t');
                fpr.Read();

                return;

                foreach(string line in File.ReadLines (fileName))
                {
                    ProcessLine(line);

                    if (LineCounter == ReportLineCount)
                    {
                        Console.WriteLine("{0} processed lines {1}", DateTime.Now.ToString("yyyyMMdd HH:mm:ss"), TotalLineCounter);
                        LineCounter = 0;
                    }
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

        private static void ProcessLine(string line)
        {
            Interlocked.Increment(ref LineCounter);
            Interlocked.Increment(ref TotalLineCounter);

            string fileNameFromLine = ReadFileNameFromLine (line);
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
                    buildFileName.Append (".tcv");

                    return buildFileName.ToString ();
                }
                buildFileName.Append (c);
            }

            throw new InvalidOperationException (string.Format ("Cannot read file name from the line: {0}", line));
        }
    }
}
