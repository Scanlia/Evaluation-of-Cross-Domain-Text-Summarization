using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesListToTxt
{
    class Program
    {
        private static DirectoryInfo gigaFolder = new DirectoryInfo("C:\\HONOURS\\gigawordProcessed");

        static void Main(string[] args)
        {
            foreach (var folder in gigaFolder.GetDirectories())
            {
                var path = gigaFolder.FullName + "\\" + folder.Name + ".txt";
                StreamWriter streamWriter = new StreamWriter(path);
                streamWriter.NewLine = "\n";

                Console.WriteLine("Writing to " + path);

                foreach (var file in folder.GetFiles())
                    streamWriter.WriteLine(file.Name);

                streamWriter.Flush();
                streamWriter.Close();

                Console.WriteLine("Finished " + path);
            }
        }
    }
}
