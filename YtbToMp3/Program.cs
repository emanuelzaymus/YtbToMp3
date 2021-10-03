using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace YtbToMp3
{
    class Program
    {
        public static async Task<int> Main(string[] args)
        {
            if (args.Length < 1 || args.Length > 2)
            {
                PrintHelp();
                return -1;
            }

            string youtubeUrlsFile = args[0];
            string outputDirectory = args.Length > 1 ? args[1] : ".";


            Console.WriteLine("Started");
            Stopwatch stopwatch = new();

            stopwatch.Start();

            using (CancellationTokenSource cancellationTokenSource = new())
            {
                var youtubrUrls = await File.ReadAllLinesAsync(youtubeUrlsFile, cancellationTokenSource.Token);

                YoutubeToMp3 youtubeToMp3 = new();

                await youtubeToMp3.DownloadAsync(youtubrUrls, outputDirectory, cancellationToken: cancellationTokenSource.Token);
            }

            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);

            Console.WriteLine("End");

            return 0;
        }

        private static void PrintHelp()
        {
            Console.WriteLine(
                "YtbToMp3.exe has wrong number of input arguments. \n\n" +
                "YtbToMp3.exe Arguments: \n" +
                "    1. Text file with Youtube urls, e.g.: \n" +
                "        https://www.youtube.com/watch?v=2UxUJR31MEe \n" +
                "        https://www.youtube.com/watch?v=osfDwb9U95w \n" +
                "        https://www.youtube.com/watch?v=LqFtyWFG85q \n" +
                "        ... \n\n" +
                "    2. Output directory where to save the files. (Optional - default: \".\") \n\n" +
                "E.g.: YtbToMp3.exe song_urls.txt ./my_songs/"
            );
        }

    }
}
