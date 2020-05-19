using System;
using System.Collections.Generic;
using System.IO;

namespace GigawordPicker
{
    class Program
    {
        private static DirectoryInfo dirInfo = new DirectoryInfo("D:\\gigaword");

        private static string testDir = "C:\\HONOURS\\gigawordProcessed\\test\\";
        private static string trainDir = "C:\\HONOURS\\gigawordProcessed\\train\\";
        private static string valDir = "C:\\HONOURS\\gigawordProcessed\\val\\";

        private static int train = 287227;
        private static int val = 13368;
        private static int test = 11490;
        
        static void Main(string[] args)
        {
            var dirs = dirInfo.GetDirectories();
            var rand = new Random();

            foreach (var dir in dirs)
            {
                var fileInfos = dir.GetFiles();

                for (int i = 0; i < test / dirs.Length; i++)
                {
                    while (true)
                    {
                        var file = fileInfos[rand.Next(fileInfos.Length)];
                        var destFileName = testDir + file.Name;
                        var otherFileName = trainDir + file.Name;
                        var otherFileName2 = valDir + file.Name;
                        if (!File.Exists(destFileName) && !File.Exists(otherFileName) && !File.Exists(otherFileName2))
                        {
                            File.Copy(file.FullName, destFileName);
                            break;
                        }
                    }
                }

                Console.WriteLine(dir.Name + " Test (Done)");

                for (int i = 0; i < val / dirs.Length; i++)
                {
                    while (true)
                    {
                        var file = fileInfos[rand.Next(fileInfos.Length)];
                        var destFileName = valDir + file.Name;
                        var otherFileName = trainDir + file.Name;
                        var otherFileName2 = testDir + file.Name;
                        if (!File.Exists(destFileName) && !File.Exists(otherFileName) && !File.Exists(otherFileName2))
                        {
                            File.Copy(file.FullName, destFileName);
                            break;
                        }
                    }
                }

                Console.WriteLine(dir.Name + " Val (Done)");

                for (int i = 0; i < train / dirs.Length; i++)
                {
                    while (true)
                    {
                        var file = fileInfos[rand.Next(fileInfos.Length)];
                        var destFileName = trainDir + file.Name;
                        var otherFileName = testDir + file.Name;
                        var otherFileName2 = valDir + file.Name;
                        if (!File.Exists(destFileName) && !File.Exists(otherFileName) && !File.Exists(otherFileName2))
                        {
                            File.Copy(file.FullName, destFileName);
                            break;
                        }
                    }
                }

                Console.WriteLine(dir.Name + " Train (Done)");
            }
        }
    }
}
