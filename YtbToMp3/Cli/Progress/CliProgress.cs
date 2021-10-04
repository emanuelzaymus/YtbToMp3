using System;
using YtbToMp3.Output;

namespace YtbToMp3
{
    class CliProgress : IProgress<double>
    {
        private readonly (int Left, int Top) _consolePosition;

        private readonly ISynchronizedOutput _output;

        private readonly IParentProgress _parentProgress;

        private double _lastValue = 0;

        public CliProgress((int Left, int Top) consolePosition, ISynchronizedOutput output, IParentProgress parentProgress)
        {
            _consolePosition = consolePosition;
            _output = output;
            _parentProgress = parentProgress ?? throw new ArgumentNullException(nameof(parentProgress));
        }

        public void Report(double value)
        {
            // Update only if the new value is at least 5% greater than last value.
            if (value >= _lastValue + 0.05 || value == 1)
            {
                Print(value);

                double increase = value - _lastValue;
                _parentProgress.PreportSubProgressIncrease(increase);

                _lastValue = value;
            }
        }

        public void PrintZeroPercent() => Print(0);

        private void Print(double value)
        {
            _output.CursorPositon = _consolePosition;

            // Print in percent format
            _output.WriteLineSync(string.Format(" {0:P2}", value));
        }

    }
}
