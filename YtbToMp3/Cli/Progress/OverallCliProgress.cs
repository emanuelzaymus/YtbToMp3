using System;
using YtbToMp3.Output;

namespace YtbToMp3
{
    class OverallCliProgress : IParentProgress
    {
        private const double Tolerance = 0.001;

        private readonly int _numberOfSubProgresses;
        private readonly ISynchronizedOutput _output;
        private double _lastValue;

        public (int Left, int Top) ConsolePosition { get; set; }

        public OverallCliProgress(int numberOfSubProgresses, ISynchronizedOutput output)
        {
            _numberOfSubProgresses = numberOfSubProgresses;
            _output = output;
        }
        public void PreportSubProgressIncrease(double subProgressIncrease)
        {
            double partialIncrease = subProgressIncrease / _numberOfSubProgresses;

            _lastValue += partialIncrease;

            Print(_lastValue);
        }

        public void PrintZeroPercent() => Print(0);

        private void Print(double value)
        {
            _output.CursorPositon = ConsolePosition;

            for (int i = 0; i < 20; i++)
            {
                _output.WriteSync(LessOrEqualWithTolerance(i + 1, value * 20) ? '#' : '.');
            }

            // Print in percent format
            _output.WriteLineSync(string.Format(" {0:P2}", value));
        }

        private bool LessOrEqualWithTolerance(double first, double second)
        {
            if (first <= second)
            {
                return true;
            }

            // Equal
            if (Math.Abs(first - second) <= Tolerance)
            {
                return true;
            }

            return false;
        }

    }
}
