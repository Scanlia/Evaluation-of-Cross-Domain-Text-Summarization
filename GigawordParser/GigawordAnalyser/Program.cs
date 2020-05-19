using System;
using System.IO;
using System.Reflection.Emit;

namespace GigawordAnalyser
{
    class Program
    {
        private static string folder = "C:\\HONOURS\\finished_files\\giga\\test\\reversed";

        static void Main(string[] args)
        {
            V2();

            Console.WriteLine("Done.");
            Console.ReadKey();
        }

        private static void V2()
        {
            DirectoryInfo dir = new DirectoryInfo(folder);

            foreach (var file in dir.GetFiles())
            {
                using (StreamReader sr = new StreamReader(file.FullName))
                {
                    int lineCount = 0;

                    string previousLine = "";

                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();

                        if (line.Contains("],"))
                        {
                            if (previousLine.EndsWith(","))
                                Console.WriteLine(file.Name);
                            break;
                        }

                        previousLine = line;
                    }
                }
            }
        }
    }
}
