namespace YtbToMp3.Output
{
    interface ISynchronizedOutput
    {
        public bool CursorVisible { get; set; }

        public (int Left, int Top) CursorPositon { get; set; }

        void WriteSync(string value);

        void WriteSync(char value);

        void WriteLineSync(string value);
    }
}
