using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace JSONAnalyser
{
    class Program
    {
        private static string dir = "C:\\HONOURS\\finished_files\\train_giga";

        static void Main(string[] args)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dir);

            int count = 0;

            bool reWriteJSON = false;

            double wordsLenAvg = 0;
            int wordsLenAvgCount = 1;

            double wordsInSent = 0;
            int wordsInSentCount = 1;

            double artLenAvg = 0;
            int artLenCount = 1;

            foreach (var file in dirInfo.GetFiles())
            {
                if (++count % 1000 == 0)
                {
                    Console.WriteLine(count + " Sent: " + wordsInSent.ToString("##.000") + " Art: " + artLenAvg.ToString("##.000"));
                }

                reWriteJSON = false;
                JSONClass doc = null;

                using (StreamReader streamReader = new StreamReader(file.FullName))
                {
                    string content = streamReader.ReadToEnd();
                    doc = JsonConvert.DeserializeObject<JSONClass>(content);

                    artLenAvg = artLenAvg * (artLenCount - 1) / artLenCount + doc.article.Count / (double)artLenCount;
                    artLenCount++;

                    foreach (var sentence in doc.article)
                    {
                        var words = sentence.Trim('.', ' ').Split(' ');

                        wordsInSent = wordsInSent * (wordsInSentCount - 1) / wordsInSentCount + words.Length / (double)wordsInSentCount;
                        wordsInSentCount++;

                        foreach (var word in words)
                        {
                            wordsLenAvg = wordsLenAvg * (wordsLenAvgCount - 1) / wordsLenAvgCount + word.Length / (double)wordsLenAvgCount;
                            wordsLenAvgCount++;
                        }
                    }
                }
            }

            Console.WriteLine("C/W AVG: " + wordsLenAvg);
            Console.WriteLine("W/S AVG: " + wordsInSent);
            Console.WriteLine("S/A AVG: " + artLenAvg);
            Console.WriteLine("DONE");
            Console.ReadKey();

        }

        public static bool IsValidEmail(string email)
        {
            if (email.Length == 1)
                return false;

            if (email.Contains("@"))
            {
                Console.WriteLine(email);
                return true;
            }

            return false;
        }
    }

    public class JSONClass
    {
        public string id;

        public List<string> article;
        [JsonProperty(PropertyName = "abstract")]
        public List<string> summary;

        public List<int> extracted;
        public List<float> score;
    }
}
