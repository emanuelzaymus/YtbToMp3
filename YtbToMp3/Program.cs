using YtbToMp3;
using YtbToMp3.Cli;
using YtbToMp3.Output;


var cli = new CliDownloader(new YoutubeToMp3(), new ConsoleOutput());

if (cli.InvalidArguments(args))
{
    cli.PrintInvalidArgumentsHelp();
    return;
}

await cli.DownloadWithCliAsync(args);