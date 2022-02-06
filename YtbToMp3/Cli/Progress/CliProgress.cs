using System;
using YtbToMp3.Output;

namespace YtbToMp3.Cli.Progress
{
    internal class CliProgress : IProgress<double>
    {
        private const double Tolerance = 0.0001;

        private readonly (int Left, int Top) _cursorPosition;

        private readonly ISynchronizedOutput _output;

        private readonly IParentProgress _parentProgress;

        private double _lastValue;

        public CliProgress((int Left, int Top) cursorPosition, ISynchronizedOutput output,
            IParentProgress parentProgress)
        {
            _cursorPosition = cursorPosition;
            _output = output;
            _parentProgress = parentProgress ?? throw new ArgumentNullException(nameof(parentProgress));
        }

        public void Report(double value)
        {
            // Update only if the new value is at least 5% greater than last value.
            if (value >= _lastValue + 0.05 || Math.Abs(value - 1) < Tolerance)
            {
                Print(value);

                double increase = value - _lastValue;
                _parentProgress.ReportSubProgressIncrease(increase);

                _lastValue = value;
            }
        }

        public void PrintZeroPercent() => Print(0);

        private void Print(double value)
        {
            _output.CursorPosition = _cursorPosition;

            // Print in percent format
            _output.WriteLineSync($" {value:P2}");
        }
    }
}