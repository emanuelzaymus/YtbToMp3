using System;

namespace YtbToMp3.Cli
{
    internal class CliProgress : IProgress<double>
    {
        private const double Tolerance = 0.0001;

        private readonly (int Left, int Top) _cursorPosition;

        private double _lastValue;

        public CliProgress((int Left, int Top) cursorPosition)
        {
            _cursorPosition = cursorPosition;
        }

        public void Report(double value)
        {
            // Update only if the new value is at least 5% greater than last value.
            if (value >= _lastValue + 0.05 || Math.Abs(value - 1) < Tolerance)
            {
                Print(value);

                _lastValue = value;
            }
        }

        public void PrintZeroPercent() => Print(0);

        private void Print(double value)
        {
            Console.SetCursorPosition(_cursorPosition.Left, _cursorPosition.Top);

            // Print in percent format
            Console.WriteLine($" {value:P2}");
        }
    }
}