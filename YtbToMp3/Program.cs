using System.Threading.Tasks;
using YtbToMp3.Cli;

namespace YtbToMp3
{
    class Program
    {
        public static async Task<int> Main(string[] args)
        {
            CliDownloader cli = new(new YoutubeToMp3(), new ConsoleOutput());

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
