using YtbToMp3;
using YtbToMp3.Cli;


var cli = new CliDownloader(new YoutubeToMp3());

if (cli.InvalidArguments(args))
{
    cli.PrintInvalidArgumentsHelp();
    return;
}

await cli.DownloadWithCliAsync(args);