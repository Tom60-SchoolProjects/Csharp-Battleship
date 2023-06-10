namespace Battleship.TUI;

/// <summary>
/// Class to handle the console buffer.
/// Console buffer is a buffer that store the current state of the console, this allow to increase performance.
/// </summary>
internal class ConsoleBuffer
{
    public int BufferHeight { get; private set; }
    public int BufferWidth { get; private set; }
    public char[] Buffer { get; private set; }
    public char[] oldBuffer { get; private set; }
    public ConsoleColor[] BufferForegroundColor { get; private set; }
    public ConsoleColor[] BufferBackgroundColor { get; private set; }

    public ConsoleBuffer() : this(Console.WindowHeight, Console.WindowWidth) { }

    public ConsoleBuffer(int height, int width)
    {
        BufferHeight = height;
        BufferWidth = width;
        Buffer = Enumerable.Repeat(' ', BufferHeight * BufferWidth).ToArray();
        oldBuffer = new char[Buffer.Length];
        BufferForegroundColor = new ConsoleColor[BufferHeight * BufferWidth];
        BufferBackgroundColor = new ConsoleColor[BufferHeight * BufferWidth];
    }


    public void WriteTo(in string text, in int top, in int left,
                         in ConsoleColor foregroundColor = ConsoleColor.Gray,
                         in ConsoleColor backgroundColor = ConsoleColor.Black) =>
        WriteTo(text, Convert.ToUInt32(top), Convert.ToUInt32(left), foregroundColor, backgroundColor);


    /// <summary>
    /// Write to the given position inside the buffer
    /// </summary>
    /// <param name="text">Text to write</param>
    /// <param name="top">Top position</param>
    /// <param name="left">Left posisition</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Throw when the top is greater than the height of the buffer
    /// - or -
    /// when left is greater than the width of the buffer
    /// </exception>
    public void WriteTo( in string text, in uint top, in uint left,
                         in ConsoleColor foregroundColor = ConsoleColor.Gray,
                         in ConsoleColor backgroundColor = ConsoleColor.Black )
    {
        var startIndex = BufferWidth * top + left;

        // Ouai j'avoue c'est de la merde #GiveUp
        if (top > Buffer.Length)
            throw new ArgumentOutOfRangeException("top is greater than the height of the buffer");

        if (left + text.Length > BufferWidth)
            throw new ArgumentOutOfRangeException("left is greater than the width of the buffer");

        for (int i = 0; i < text.Length; i++)
        {
            Buffer[startIndex + i] = text[i];
            BufferForegroundColor[startIndex + i] = foregroundColor;
            BufferBackgroundColor[startIndex + i] = backgroundColor;
        }
    }

    public void WriteTo(in char text, in int top, in int left,
                         in ConsoleColor foregroundColor = ConsoleColor.Gray,
                         in ConsoleColor backgroundColor = ConsoleColor.Black) =>
        WriteTo(text, Convert.ToUInt32(top), Convert.ToUInt32(left), foregroundColor, backgroundColor);


    /// <summary>
    /// Write to the given position inside the buffer
    /// </summary>
    /// <param name="character">Character to write</param>
    /// <param name="top">Top position</param>
    /// <param name="left">Left posisition</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Throw when the top is greater than the height of the buffer
    /// - or -
    /// when left is greater than the width of the buffer
    /// </exception>
    public void WriteTo( in char character, in uint top, in uint left, 
                         in ConsoleColor foregroundColor = ConsoleColor.Gray,
                         in ConsoleColor backgroundColor = ConsoleColor.Black )
    {
        var startIndex = BufferWidth * top + left;

        if (top >= BufferHeight)
            throw new ArgumentOutOfRangeException("top is greater than the height of the buffer");

        if (left >= BufferWidth)
            throw new ArgumentOutOfRangeException("left is greater than the width of the buffer");

        Buffer[startIndex] = character;
        BufferForegroundColor[startIndex] = foregroundColor;
        BufferBackgroundColor[startIndex] = backgroundColor;
    }

    /// <summary>
    /// Clear the buffer
    /// </summary>
    public void Clear()
    {
        Buffer = Enumerable.Repeat(' ', BufferHeight * BufferWidth).ToArray();
    }

    /// <summary>
    /// Send the current buffer to the console buffer
    /// </summary>
    /// <param name="topOffset">The top offset</param>
    /// <param name="leftOffset">The left offset (will work on the first line only)</param>
    public void Flush(in int topOffset = 0, in int leftOffset = 0)
    {
        // TODO: make left offset work on every line (I added that but it's useless for now)

        for (int y = 0; y < BufferHeight; y++)
        {
            for (int x = 0; x < BufferWidth; x++)
            {
                int xOffset = leftOffset + x;
                int yOffset = topOffset + y;

                if (xOffset >= Console.WindowWidth)
                    continue;

                if (yOffset >= Console.WindowWidth)
                    continue;

                if (Buffer[y * BufferWidth + x] == oldBuffer[y * BufferWidth + x])
                    continue;

                Console.SetCursorPosition(xOffset, yOffset);
                Console.ForegroundColor = BufferForegroundColor[y * BufferWidth + x];
                Console.BackgroundColor = BufferBackgroundColor[y * BufferWidth + x];
                Console.Write(Buffer[y * BufferWidth + x]);
            }
        }

        Array.Copy(Buffer, oldBuffer, Buffer.Length);
    }

    /// <summary>
    /// Change the size of the buffer
    /// </summary>
    /// <param name="height">Height of the buffer</param>
    /// <param name="width">Width of the buffer</param>
    public void ChangeBufferSize(int? height = null, int? width = null)
    {
        height ??= Console.WindowHeight;
        width ??= Console.WindowWidth;

        BufferHeight = height.Value;
        BufferWidth = width.Value;
        Buffer = Enumerable.Repeat(' ', BufferHeight * BufferWidth).ToArray();
        oldBuffer = new char[Buffer.Length];
        BufferForegroundColor = new ConsoleColor[BufferHeight * BufferWidth];
        BufferBackgroundColor = new ConsoleColor[BufferHeight * BufferWidth];
    }
}