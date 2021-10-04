using System;
using YtbToMp3.Output;

namespace YtbToMp3.Cli
{
    internal class ConsoleOutput : ISynchronizedOutput
    {
        private static readonly object _lock = new();

        public bool CursorVisible
        {
            get => Console.CursorVisible;
            set
            {
                lock (_lock)
                {
                    Console.CursorVisible = value;
                }
            }
        }

        public (int Left, int Top) CursorPositon
        {
            get => Console.GetCursorPosition();
            set
            {
                lock (_lock)
                {
                    Console.SetCursorPosition(value.Left, value.Top);
                }
            }
        }

        public void WriteSync(string value)
        {
            lock (_lock)
            {
                Console.Write(value);
            }
        }

        public void WriteSync(char value)
        {
            WriteSync(value.ToString());
        }

        public void WriteLineSync(string value)
        {
            lock (_lock)
            {
                Console.WriteLine(value);
            }
        }

    }
}
