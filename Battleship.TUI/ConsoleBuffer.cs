namespace Battleship.TUI;

internal class ConsoleBuffer
{
    public readonly int BufferHeight;
    public readonly int BufferWidth;
    public readonly char[] Buffer;
    public readonly ConsoleColor[] BufferForegroundColor;    
    public readonly ConsoleColor[] BufferBackgroundColor;

    public ConsoleBuffer() : this(Console.BufferHeight, Console.BufferWidth) { }

    public ConsoleBuffer(int height, int width)
    {
        BufferHeight = height;
        BufferWidth = width;
        Buffer = Enumerable.Repeat(' ', BufferHeight * BufferWidth).ToArray();
        BufferForegroundColor = new ConsoleColor[BufferHeight * BufferWidth];
        BufferBackgroundColor = new ConsoleColor[BufferHeight * BufferWidth];
    }

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
    public void WriteTo( in string text, in int top, in int left,
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
    public void WriteTo( in char character, in int top, in int left, 
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

    public void Clear()
    {
        for (int y = 0; y < BufferHeight; y++)
        {
            for (int x = 0; x < BufferWidth; x++)
            {
                Buffer[y * BufferWidth + x] = ' ';
            }
        }
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
                var xOffset = leftOffset + x;
                var yOffset = topOffset + y;

                if (xOffset >= Console.BufferWidth)
                    continue;

                if (yOffset >= Console.BufferHeight)
                    continue;

                Console.SetCursorPosition(xOffset, yOffset);
                Console.ForegroundColor = BufferForegroundColor[y * BufferWidth + x];
                Console.BackgroundColor = BufferBackgroundColor[y * BufferWidth + x];
                Console.Write(Buffer[y * BufferWidth + x]);
            }
        }
    }
}