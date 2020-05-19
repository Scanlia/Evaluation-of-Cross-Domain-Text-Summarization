using System;
using System.Collections.Generic;
using System.IO;

namespace FolderSplitter
{
    class Program
    {
        private static int train = 287227;
        private static int val = 13368;
        private static int test = 11490;

        private static SortedSet<int> usedIDs = new SortedSet<int>();
        private static Random rand = new Random(DateTime.Now.Millisecond);

        private static string allDir = "C:\\HONOURS\\gigawordProcessed\\all\\";

        private static string testDir = "C:\\HONOURS\\gigawordProcessed\\test\\";
        private static string trainDir = "C:\\HONOURS\\gigawordProcessed\\train\\";
        private static string valDir = "C:\\HONOURS\\gigawordProcessed\\val\\";

        static void Main(string[] args)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(allDir);
            var currentFiles = dirInfo.GetFiles();

            for (int i = 0; i < val; i++)
            {
                MoveARandomFile(currentFiles, valDir);
                if (i % 1000 == 0)
                    Console.WriteLine("Creating TRAIN " + i + "/" + val);
            }

            for (int i = 0; i < train; i++)
            {
                MoveARandomFile(currentFiles, trainDir);
                if (i % 1000 == 0)
                    Console.WriteLine("Creating TRAIN " + i + "/" + train);
            }

            for (int i = 0; i < test; i++)
            {
                MoveARandomFile(currentFiles, testDir);
                if (i % 1000 == 0)
                    Console.WriteLine("Creating TRAIN " + i + "/" + test);
            }

            Console.WriteLine("DONE! Press Enter...");
            Console.ReadLine();
        }

        private static void MoveARandomFile(FileInfo[] currentFiles, string newDir)
        {
            int moveIndex = rand.Next(currentFiles.Length);
            int tryCount = 0;

            while (usedIDs.Contains(moveIndex))
            {
                if (tryCount++ > 10000)
                {
                    Console.WriteLine("TRIED SO MANY TIMES :'(");
                    Console.ReadKey();
                }

                moveIndex = rand.Next(currentFiles.Length);
            }

            usedIDs.Add(moveIndex);
            File.Move(currentFiles[moveIndex].FullName, newDir + currentFiles[moveIndex].Name);
        }
    }
}
