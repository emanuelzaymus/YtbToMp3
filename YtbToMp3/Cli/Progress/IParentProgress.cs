namespace YtbToMp3.Cli.Progress
{
    internal interface IParentProgress
    {
        void ReportSubProgressIncrease(double subProgressIncrease);
    }
}