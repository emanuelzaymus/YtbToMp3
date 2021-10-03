using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YtbToMp3
{
    class CliProgress : IProgress<double>
    {
        private double _lastValue = 0;

        public void Report(double value)
        {
            if (_lastValue + 0.05 < value)
            {
                Console.WriteLine(value * 100 + "%");
                _lastValue = value;
            }
        }
    }
}
