using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YtbToMp3.Output;

namespace YtbToMp3
{
    class CliDownloader
    {
        private readonly YoutubeToMp3 _youtubeToMp3;

        private readonly ISynchronizedOutput _output;

        private readonly Stopwatch _stopwatch = new();

        public CliDownloader(YoutubeToMp3 youtubeToMp3, ISynchronizedOutput synchronizedOutput)
        {
            _youtubeToMp3 = youtubeToMp3 ?? throw new ArgumentNullException(nameof(youtubeToMp3));
            _output = synchronizedOutput;
        }

        public bool InvalidArguments(string[] args)
        {
            return args.Length < 1 || args.Length > 2;
        }

        public void PrintInvalidArgumentsHelp()
        {
            Console.WriteLine("YtbToMp3.exe has wrong number of input arguments. \n");
            PrintArgumentsHelp();
        }

        public async Task DownloadWithCliAsync(string[] args)
        {
            string youtubeUrlsFile = args[0];
            string outputDirectory = args.Length > 1 ? args[1] : ".";

            Start();

            using (CancellationTokenSource cancellationTokenSource = new())
            {
                var youtubeUrls = await ReadAllUrlsAsync(youtubeUrlsFile, cancellationTokenSource.Token);

                List<Task> allTasks = GetAllDownloadTasks(youtubeUrls, outputDirectory, cancellationTokenSource.Token);

                await Task.WhenAll(allTasks);
            }

            End();
        }

        private async Task<string[]> ReadAllUrlsAsync(string youtubeUrlsFile, CancellationToken cancellationToken)
        {
            var youtubeUrls = await File.ReadAllLinesAsync(youtubeUrlsFile, cancellationToken);

            return youtubeUrls.Where(url => !string.IsNullOrEmpty(url)).ToArray();
        }

        private List<Task> GetAllDownloadTasks(string[] youtubrUrls, string outputDirectory, CancellationToken cancellationToken)
        {
            var overallProgress = new OverallCliProgress(youtubrUrls.Length, _output);

            var allTasks = youtubrUrls
                .Select(url => DownloadWithCliTask(url, outputDirectory, cancellationToken, overallProgress))
                .ToList();

            _output.WriteSync("\nTotal Progress: ");
            overallProgress.ConsolePosition = _output.CursorPositon;
            overallProgress.PrintZeroPercent();

            return allTasks;
        }

        private Task DownloadWithCliTask(string youtubeUrl, string outputDirectory, CancellationToken cancellationToken, IParentProgress overallProgress)
        {
            _output.WriteLineSync(youtubeUrl);

            var videoTitle = _youtubeToMp3.GetVideoTitle(youtubeUrl);
            _output.WriteSync(videoTitle);

            CliProgress cliProgress = new(_output.CursorPositon, _output, overallProgress);

            cliProgress.PrintZeroPercent();

            return _youtubeToMp3.DownloadTask(youtubeUrl, outputDirectory, cliProgress, cancellationToken);
        }

        private void Start()
        {
            //_output.CursorVisible = false;

            _stopwatch.Restart();
        }

        private void End()
        {
            _stopwatch.Stop();

            _output.WriteLineSync($"\nElapsed time: {_stopwatch.ElapsedMilliseconds} ms");
            _output.CursorVisible = true;
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
