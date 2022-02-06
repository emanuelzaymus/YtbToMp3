using System.Threading.Tasks;
using YtbToMp3.Cli;
using YtbToMp3.Output;

namespace YtbToMp3
{
    internal static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var cli = new CliDownloader(new YoutubeToMp3(), new ConsoleOutput());

            if (cli.InvalidArguments(args))
            {
                cli.PrintInvalidArgumentsHelp();
                return -1;
            }

            await cli.DownloadWithCliAsync(args);

            return 0;
        }
    }
}