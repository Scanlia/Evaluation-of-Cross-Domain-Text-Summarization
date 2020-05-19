using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SummaryComparisonPicker
{
    class Program
    {
        private const int numberOfSummaries = 1000;
        private static string groundTruth = "C:\\HONOURS\\OUTPUTS\\reference";
        private static string outputDir_abs = "C:\\HONOURS\\OUTPUTS\\crossdomain-fastabsrl";
        private static string outputDir_uni = "C:\\HONOURS\\OUTPUTS\\crossdomain-unified";

        private static List<SummaryComparison> list1 = new List<SummaryComparison>();

        static void Main(string[] args)
        {
            DirectoryInfo di_uni = new DirectoryInfo(outputDir_uni);
            FileInfo[] fileInfos = di_uni.GetFiles();

            for (int index = 0; index < fileInfos.Length; index++)
            {
                var file = fileInfos[index];
                SummaryComparison sc = new SummaryComparison();
                sc.docID = int.Parse(file.Name.Split('_')[0]);

                StreamReader sr = new StreamReader(file.FullName);
                sc.parsedUNI = ParseText(sr.ReadLine());
                sr.Close();

                string absPath = outputDir_abs + "\\" + sc.docID + ".dec";
                string refPath = groundTruth + "\\" + sc.docID.ToString("000000") + "_reference.txt";

                if (!File.Exists(absPath) || !File.Exists(refPath))
                {
                    Console.WriteLine("Can't find file " + absPath);
                    Console.ReadLine();
                    return;
                }

                StreamReader sr3 = new StreamReader(refPath);
                sc.groundTruth = ParseText(sr3.ReadLine());
                sr3.Close();

                StreamReader sr2 = new StreamReader(absPath);
                sc.parsedABS = ParseText(sr2.ReadLine());
                sr2.Close();

                if (list1.Count < numberOfSummaries)
                    list1.Add(sc);
                else
                {
                    int maxDistance = 0;
                    int indexOfMax = 0;

                    for (int i = 0; i < list1.Count; i++)
                    {
                        if (list1[i].GetDifference() > maxDistance)
                        {
                            maxDistance = list1[i].GetDifference();
                            indexOfMax = i;
                        }
                    }

                    if (maxDistance > sc.GetDifference() && !sc.SameSummary())
                        list1[indexOfMax] = sc;
                }

                if (index % 100 == 0)
                    Console.WriteLine(file);
            }

            var sortedList = list1.OrderBy(i => i.GetDifference()).ToList();

            foreach (var item in sortedList)
            {
                Console.WriteLine(item.GetDifference() + " " + item.docID);
            }

            var outputFile = di_uni.Parent.FullName + "\\SummariesOutput.txt";

            if (File.Exists(outputFile))
                File.Delete(outputFile);

            using (StreamWriter streamWriter = new StreamWriter(outputFile))
            {
                foreach (var item in sortedList)
                {
                    streamWriter.WriteLine(item.docID);
                    streamWriter.WriteLine(item.groundTruth);
                    streamWriter.WriteLine(item.parsedABS);
                    streamWriter.WriteLine(item.parsedUNI);
                    streamWriter.WriteLine();
                }
            }

            Console.ReadLine();
            Console.WriteLine("Completed output.");
        }

        private static string ParseText(string source)
        {
            source = source.Replace(" -lrb- ", " (");
            source = source.Replace(" -rrb- ", ") ");
            source = source.Replace("&amp;", "&");
            source = source.Replace(" .", ".");
            source = source.Replace(" , ", ", ");
            source = source.Replace(" 's ", "'s ");
            return source;
        }
    }

    class SummaryComparison
    {
        public int docID;
        public string groundTruth;
        public string parsedABS;
        public string parsedUNI;

        public int GetDifference()
        {
            return Math.Abs(parsedABS.Length - parsedUNI.Length);
        }

        public bool SameSummary()
        {
            return parsedABS == parsedUNI;
        }
    }
}
