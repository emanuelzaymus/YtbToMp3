using System;

namespace YtbToMp3.Output
{
    internal class ConsoleOutput : ISynchronizedOutput
    {
        private static readonly object Lock = new();

        public (int Left, int Top) CursorPosition
        {
            get => Console.GetCursorPosition();
            set
            {
                lock (Lock)
                {
                    Console.SetCursorPosition(value.Left, value.Top);
                }
            }
        }

        public void SetCursorVisible(bool visible)
        {
            lock (Lock)
            {
                Console.CursorVisible = visible;
            }
        }

        public void WriteSync(string value)
        {
            lock (Lock)
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
            lock (Lock)
            {
                Console.WriteLine(value);
            }
        }
    }
}