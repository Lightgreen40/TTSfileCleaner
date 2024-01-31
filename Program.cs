using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using IniParser;
using IniParser.Model;




namespace TTSfileCleaner
{
    class Program
    {
        static void Main(string[] args)
        {
            string iniFilePath = "settings.ini";
            FileIniDataParser parser = new FileIniDataParser();
            IniData iniData = parser.ReadFile(iniFilePath);


            Console.WriteLine(@"State filepath to TTSfile to be cleaned, e.g. 'C:\tts_news.txt':");
            string ttsFilepath = Console.ReadLine();


            //remove lines < [INI] chars:
            List<string> ttsFileLines = File.ReadAllLines(ttsFilepath).ToList();
            int removeLinesWithLessThanCharsValue = Convert.ToInt32(iniData["Settings"]["RemoveLinesWithLessThanCharsValue"]);
            for (int i = ttsFileLines.Count - 1; i >= 0; i--)
            {
                if (ttsFileLines[i].Count() < removeLinesWithLessThanCharsValue)
                {
                    ttsFileLines.Remove(ttsFileLines[i]);
                    try
                    {
                        Console.WriteLine($"This line was removed: {ttsFileLines[i]}");
                    }
                    catch (Exception exception)
                    {
                        Console.ForegroundColor= ConsoleColor.Red;
                        Console.WriteLine(exception.ToString());
                        Console.ResetColor();
                        continue;
                    }
                }
            }


            //remove identical and very similar lines:
            int maximumLevenshteinDistanceSimilarLinesValue = Convert.ToInt32(iniData["Settings"]["MaximumLevenshteinDistanceSimilarLinesValue"]);
            for (int i = ttsFileLines.Count - 1; i > 0; i--)
            {
                try
                {
                    int distance = ComputeLevenshteinDistance(ttsFileLines[i], ttsFileLines[i - 1]);
                    Console.WriteLine(distance.ToString());
                    Console.WriteLine(ttsFileLines[i] + " – " + ttsFileLines[i - 1]);
                    Console.WriteLine();
                    Console.WriteLine();
                    if (distance <= maximumLevenshteinDistanceSimilarLinesValue)   //[INI] "distance" = amount of differing characters
                    {
                        ttsFileLines.Remove(ttsFileLines[i]);
                        ttsFileLines.Remove(ttsFileLines[i - 1]);
                        Console.WriteLine($"This line was removed: {ttsFileLines[i]}");
                    }
                }
                catch (Exception exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(exception.ToString());
                    Console.ResetColor();
                    continue;
                }
            }


            Console.WriteLine();
            
            File.WriteAllLines(Path.GetDirectoryName(ttsFilepath) + @"\" + Path.GetFileNameWithoutExtension(ttsFilepath) + "_cleaned" + Path.GetExtension(ttsFilepath), ttsFileLines);

            Console.WriteLine("Done – TTSfile cleaned!");

            Console.ReadLine();
        }




        static int ComputeLevenshteinDistance(string source, string target)
        {
            if ((source == null) || (target == null)) return 0;
            if ((source.Length == 0) || (target.Length == 0)) return 0;
            if (source == target) return 0;   //if source and target are identical, the distance is obviously 0 (characters)! (originally, source.Length was stated here, which is nonsensical)
            int sourceStringCount = source.Length;
            int targetStringCount = target.Length;

            // Step 1
            if (sourceStringCount == 0)
                return targetStringCount;
            if (targetStringCount == 0)
                return sourceStringCount;
            int[,] distance = new int[sourceStringCount + 1, targetStringCount + 1];

            // Step 2
            for (int i = 0; i <= sourceStringCount; distance[i, 0] = i++) ;
            for (int j = 0; j <= targetStringCount; distance[0, j] = j++) ;
            for (int i = 1; i <= sourceStringCount; i++)
            {
                for (int j = 1; j <= targetStringCount; j++)
                {
                    // Step 3
                    int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;
                    // Step 4
                    distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
                }
            }
            return distance[sourceStringCount, targetStringCount];
        }
    }
}
