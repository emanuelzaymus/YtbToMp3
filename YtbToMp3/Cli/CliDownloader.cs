using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YtbToMp3.Cli.Progress;
using YtbToMp3.Output;

namespace YtbToMp3.Cli
{
    internal class CliDownloader
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

            Start();

            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var youtubeUrls = ReadAllUrls(youtubeUrlsFile);

                var allTasks = GetAllDownloadTasks(youtubeUrls, outputDirectory, cancellationTokenSource.Token);

                await Task.WhenAll(allTasks); // TODO: Super nasty B U G ....
            }

            End();
        }

        private string[] ReadAllUrls(string youtubeUrlsFile)
        {
            var youtubeUrls = File.ReadAllLines(youtubeUrlsFile);

            return youtubeUrls.Where(url => !string.IsNullOrEmpty(url)).ToArray();
        }

        private List<Task> GetAllDownloadTasks(IReadOnlyCollection<string> youtubeUrls, string outputDirectory,
            CancellationToken cancellationToken)
        {
            var overallProgress = new OverallCliProgress(youtubeUrls.Count, _output);

            var allTasks = youtubeUrls
                .Select(url => DownloadWithCliTask(url, outputDirectory, overallProgress, cancellationToken))
                .ToList();

            _output.WriteSync("\nTotal Progress: ");
            overallProgress.CursorPosition = _output.CursorPosition;
            overallProgress.PrintZeroPercent();

            return allTasks;
        }

        private Task DownloadWithCliTask(string youtubeUrl, string outputDirectory, IParentProgress overallProgress,
            CancellationToken cancellationToken)
        {
            _output.WriteLineSync(youtubeUrl);

            var videoTitle = _youtubeToMp3.GetVideoTitle(youtubeUrl);
            _output.WriteSync(videoTitle);

            var cliProgress = new CliProgress(_output.CursorPosition, _output, overallProgress);

            cliProgress.PrintZeroPercent();

            return _youtubeToMp3.DownloadAsync(youtubeUrl, outputDirectory, cliProgress, cancellationToken);
        }

        private void Start()
        {
            _output.SetCursorVisible(false);

            _stopwatch.Restart();
        }

        private void End()
        {
            _stopwatch.Stop();

            // Print elapsed seconds with 2 decimal places
            _output.WriteLineSync($"\nFinished in {_stopwatch.ElapsedMilliseconds / 1000.0:F2} s");
            _output.SetCursorVisible(true);
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