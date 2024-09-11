// See https://aka.ms/new-console-template for more information
//
// Why?
// To reliably get the YearMonthDate_HourMinuteSecond_TimeZone string and
// copy a file to the destination with the appended date and time now in the destination file name

using System.Linq;
using System.IO;

namespace Program
{


    class Program
    {
        static void Main(String[] args)
        {
            string timeNow = (CopyTools.GenerateDateTime());
            if (!args.Any()) return;

            string srcFileNamePath = args[0];
            string outputFolderPath = "";

            if (args.Length > 1)
            {
                outputFolderPath = args[1];
                if (!Directory.Exists(outputFolderPath)) return;
            }

            if (!File.Exists(srcFileNamePath)) return;

            CopyTools.CopyFile(srcFileNamePath, outputFolderPath, timeNow);

        }
    }
}
