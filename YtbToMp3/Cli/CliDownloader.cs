using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace YtbToMp3.Cli
{
    internal class CliDownloader
    {
        private readonly YoutubeToMp3 _youtubeToMp3;

        public CliDownloader(YoutubeToMp3 youtubeToMp3)
        {
            _youtubeToMp3 = youtubeToMp3;
        }

        public bool InvalidArguments(string[] args)
        {
            return args.Length is < 1 or > 2;
        }

        public void PrintInvalidArgumentsHelp()
        {
            Console.WriteLine("YtbToMp3.exe has wrong number of input arguments. \n");
            PrintArgumentsHelp();
        }

        public async Task DownloadWithCliAsync(string[] args)
        {
            var youtubeUrlsFile = args[0];
            var outputDirectory = args.Length > 1 ? args[1] : ".";

            Console.CursorVisible = false;
            var stopwatch = Stopwatch.StartNew();

            var youtubeUrls = ReadAllUrls(youtubeUrlsFile);

            foreach (var url in youtubeUrls)
            {
                await DownloadWithCliProgressAsync(url, outputDirectory);
            }

            // Print elapsed seconds with 2 decimal places
            Console.WriteLine($"\nFinished in {stopwatch.ElapsedMilliseconds / 1000.0:F2} s");
            Console.CursorVisible = true;
        }

        private string[] ReadAllUrls(string youtubeUrlsFile)
        {
            var youtubeUrls = File.ReadAllLines(youtubeUrlsFile);

            return youtubeUrls
                .Where(url => !string.IsNullOrEmpty(url))
                .ToArray();
        }

        private async Task DownloadWithCliProgressAsync(string youtubeUrl, string outputDirectory)
        {
            Console.WriteLine(youtubeUrl);

            try
            {
                var videoTitle = await _youtubeToMp3.GetVideoTitleAsync(youtubeUrl);
                Console.Write(videoTitle);

                var cliProgress = new CliProgress();

                cliProgress.PrintZeroPercent();

                await _youtubeToMp3.DownloadAsync(youtubeUrl, outputDirectory, cliProgress);
            }
            catch (ArgumentException)
            {
                Console.WriteLine($"Youtube url '{youtubeUrl}' is not valid or was not found.");
            }
        }

        private void PrintArgumentsHelp()
        {
            Console.WriteLine(
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