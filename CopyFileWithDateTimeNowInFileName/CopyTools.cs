using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class CopyTools
{
    public static String GenerateDateTime()
    {
        DateTime dateTime = DateTime.Now;
        string dateTimeLine = dateTime.ToString("yyyyMMdd'_'HHmmss'_'zzz");
        dateTimeLine = dateTimeLine.Replace(":", "");
        dateTimeLine = dateTimeLine.Replace("-", "n");
        dateTimeLine = dateTimeLine.Replace("+", "p");
        return dateTimeLine;
    }


    public static void CopyFile(string srcFileNamePath, string outputFolderPath, string timeNow)
    {
        string outputDir = "." + Path.PathSeparator;
        if (!String.IsNullOrEmpty(srcFileNamePath))
        {
            outputDir = Path.GetDirectoryName(srcFileNamePath);
        }
        if (!String.IsNullOrEmpty(outputFolderPath))
        {
            outputDir = (outputFolderPath);
        }
        string fileName = Path.GetFileName(srcFileNamePath);
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(srcFileNamePath);
        string extension = Path.GetExtension(srcFileNamePath);
        string outputFileName = String.IsNullOrEmpty(extension) ? $"{fileName}_{timeNow}" :
            $"{fileNameWithoutExtension}_{timeNow}{extension}";
        string outputFilePath = Path.Combine(outputDir, outputFileName);

        var srcFileInfo = new FileInfo(srcFileNamePath);

        srcFileInfo.CopyTo(outputFilePath);
        for (int i = 0; i < 3; i++)
        {
            bool isSameHash = compareFiles(srcFileNamePath, outputFilePath);
            if (isSameHash) break;
            if (!isSameHash)
            {
                srcFileInfo.CopyTo(outputFilePath);
            }
        }

    }

    public static bool compareFiles(string srcPath, string destPath)
    {
        if (String.IsNullOrEmpty(srcPath)) return false;
        if (String.IsNullOrEmpty(destPath)) return false;
        if (Hash(srcPath) == Hash(destPath)) return true;
        return false;
    }

    public static String Hash(string path)
    {
        string hash = "";
        var fi1 = new FileInfo(path);

        using (SHA256 mySHA256 = SHA256.Create())
        {

            using (FileStream fileStream = fi1.Open(FileMode.Open))
            {
                try
                {

                    fileStream.Position = 0;

                    byte[] hashValue = mySHA256.ComputeHash(fileStream);

                    hash = byteToString(hashValue);
                }
                catch (IOException e)
                {
                    Console.WriteLine($"I/O Exception: {e.Message}");
                }
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine($"Access Exception: {e.Message}");
                }

                return hash;

            }
        }
    }

    public static String byteToString(byte[] array)
    {
        var stringBuilder = new StringBuilder();
        for (int i = 0; i < array.Length; i++)
        {
            stringBuilder.Append($"{array[i]:X2}");

        }
        return stringBuilder.ToString();
    }
}