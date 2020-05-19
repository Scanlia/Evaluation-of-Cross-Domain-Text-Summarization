using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;

namespace GigawordParser
{
    class Program
    {
        private static DirectoryInfo dirInfo = new DirectoryInfo("E:\\gigaword5\\data\\xml");
        private static FileInfo[] files = null;

        private static string allDir = "C:\\HONOURS\\gigawordProcessed\\all\\";
        private static string testDir = "C:\\HONOURS\\gigawordProcessed\\test\\";
        private static string trainDir = "C:\\HONOURS\\gigawordProcessed\\train\\";
        private static string valDir = "C:\\HONOURS\\gigawordProcessed\\val\\";

        private static Random rand = new Random();

        private enum Mode { None, Head, Next, Text }
        private static Mode currentMode = Mode.None;
        private static string currentTitle = "";
        private static string currentDoc = "";
        private static List<string> article = new List<string>();

        private static List<int> pickedZipIDs = new List<int>();

        private static int train = 287227;
        private static int val = 13368;
        private static int test = 11490;

        private static int minTitleChars = 35;
        private static int minArticleChars = 450;

        static void Main(string[] args)
        {
            files = dirInfo.GetFiles();

            Console.WriteLine("Detected " + files.Length + " files.");

            ParseAndPick(allDir, train + test + val);
            //ParseAndPick(trainDir, train);
            //ParseAndPick(testDir, test);
            //ParseAndPick(valDir, val);

            Console.WriteLine("DONE!");
            Console.ReadKey();
        }

        private static void ParseAndPick(string outputDir, int count)
        {
            int currentFileCount = 0;

            while (true)
            {
                if (currentFileCount >= count)
                {
                    Console.WriteLine("Done " + currentFileCount + " " + outputDir);
                    break;
                }

                int loopCount = 0;
                int fileID = 0;

                // Picks a random gz file that hasn't been unzipped yet.
                while (true)
                {
                    loopCount++;

                    fileID = rand.Next(files.Length - 1);

                    if (!pickedZipIDs.Contains(fileID))
                    {
                        pickedZipIDs.Add(fileID);
                        break;
                    }

                    if (loopCount > 10000)
                    {
                        Console.WriteLine("Couldn't find an unused zip.");
                        break;
                    }
                }

                var file = dirInfo.FullName + "\\" + files[fileID].Name;

                if (!file.EndsWith("gz"))
                {
                    Console.WriteLine("What file??");
                    continue;
                }

                using (FileStream zipToOpen = new FileStream(file, FileMode.Open))
                {
                    using (GZipStream gStream = new GZipStream(zipToOpen, CompressionMode.Decompress))
                    {
                        Console.WriteLine("Decompressing " + file);
                        StreamReader sr = new StreamReader(gStream);

                        string line = "";

                        while (!sr.EndOfStream)
                        {
                            if (currentFileCount >= count)
                            {
                                break;
                            }

                            line = sr.ReadLine();
                            if (line == null) continue;
                            line = Regex.Replace(line, @"[^\u0000-\u007F]+", string.Empty).Trim();

                            if (currentMode == Mode.Head)
                            {
                                currentTitle = ParseText(line);

                                if (!TitleAllowed(currentTitle) || currentTitle.Length < minTitleChars)
                                {
                                    article.Clear();
                                    currentMode = Mode.None;
                                    Console.WriteLine("File " + file + " " + currentFileCount + "/" + count + " Skipping (TITLE): " + currentTitle);
                                    continue;
                                }

                                currentMode = Mode.Next;
                            }

                            if (currentMode == Mode.Text)
                            {
                                if (line.Trim() != "<P>" && line != "</P>")
                                {
                                    string parsedLine = ParseText(line);
                                    if (ArticleSentenceAllowed(parsedLine))
                                    {
                                        article.Add(parsedLine);
                                    }
                                }
                            }

                            if (currentMode == Mode.None && line.StartsWith("<DOC"))
                            {
                                currentDoc = line.Split('"')[1];
                            }

                            if (currentMode == Mode.None && line.Equals("<HEADLINE>"))
                            {
                                currentMode = Mode.Head;
                            }

                            if (currentMode == Mode.Next && line.Equals("<TEXT>"))
                            {
                                currentMode = Mode.Text;
                            }

                            if (currentMode == Mode.Text && line.Equals("</TEXT>"))
                            {
                                int totalChars = 0;

                                foreach (var artLine in article)
                                    totalChars += artLine.Length;

                                if (article.Count < 6 || totalChars < minArticleChars)
                                {
                                    Console.WriteLine("File " + file + " " + currentFileCount + "/" + count + " Skipping (ARTICLE): " + article.Count + " " + totalChars);
                                    article.Clear();
                                    currentMode = Mode.None;
                                    continue;
                                }

                                StreamWriter streamWriter = new StreamWriter(outputDir + currentDoc + ".txt");
                                streamWriter.NewLine = "\n";

                                foreach (var sent in article)
                                {
                                    streamWriter.Write(sent + "\n\n");
                                }

                                streamWriter.Write("@highlight\n\n");
                                streamWriter.Write(currentTitle + "\n");

                                streamWriter.Flush();
                                streamWriter.Close();

                                currentFileCount++;

                                article.Clear();
                                currentMode = Mode.None;
                            }
                        }

                        sr.Close();
                    }
                }
            }
        }

        private static bool ArticleSentenceAllowed(string parsedLine)
        {
            if (parsedLine.Length == 0)
                return false;

            if (parsedLine.Trim().Split(' ').Length < 8)
                return false;

            return true;
        }

        private static bool TitleAllowed(string s)
        {
            if (s.Trim().Split(' ').Length < 7)
                return false;

            return s.Length > minTitleChars;
        }

        private static string ParseText(string input)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var word in input.Split())
            {
                if (word.EndsWith(")"))
                {
                    stringBuilder.Append(word.Trim(')') + " ");
                }
            }

            string output = stringBuilder.ToString().Trim();

            if (!output.EndsWith(".")) output += " .";

            return output;
        }
    }
}
