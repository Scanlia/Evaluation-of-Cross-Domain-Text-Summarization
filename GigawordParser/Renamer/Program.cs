using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renamer
{
    class Program
    {
        private static string dir = "C:\\HONOURS\\finished_files\\giga\\test\\";

        static void Main(string[] args)
        {
            /*for (int i = 7232; i < 11489; i++)
            {
                File.Move(dir + i + ".json", dir + (i - 7232) + ".json");
            }*/

            for (int i = 0; i <= 4257; i++)
            {
                File.Move(dir + i + ".json", dir + "reversed\\" +  getReverse(i) + ".json");
            }
        }

        private static int getReverse(int i)
        {
            return (i * -1) + 4257;
        }
    }
}
