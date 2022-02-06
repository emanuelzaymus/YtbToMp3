namespace YtbToMp3.Output
{
    internal interface ISynchronizedOutput
    {
        (int Left, int Top) CursorPosition { get; set; }

        void SetCursorVisible(bool visible);

        void WriteSync(string value);

        void WriteSync(char value);

        void WriteLineSync(string value);
    }
}