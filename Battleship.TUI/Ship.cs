

namespace Battleship.TUI;

/// <summary>
/// This class is responsible fot drawing ships
/// </summary>

internal static class Ship {

    internal static void DrawHorizontalShip(in ConsoleBuffer buffer, int x, int y, int size, bool isSelected = false)
    {
        var foregroundColor = isSelected ? ConsoleColor.Black : ConsoleColor.White;
        var backgroundColor = isSelected ? ConsoleColor.White : ConsoleColor.Black;
        size--;

        buffer.WriteTo("<", x, y, foregroundColor, backgroundColor);

        for (int i = 1; i < size; i++)
            buffer.WriteTo("=", x, y + i, foregroundColor, backgroundColor);

        buffer.WriteTo(">", x, y + size, foregroundColor, backgroundColor);
    }

    internal static void DrawVerticalShip(in ConsoleBuffer buffer, int x, int y, int size, bool isSelected = false)
    {
        var foregroundColor = isSelected ? ConsoleColor.Black : ConsoleColor.White;
        var backgroundColor = isSelected ? ConsoleColor.White : ConsoleColor.Black;
        size--;

        buffer.WriteTo("/\\", x, y, foregroundColor, backgroundColor);

        for (int i = 1; i < size; i++)
            buffer.WriteTo("||", x + i, y, foregroundColor, backgroundColor);

        buffer.WriteTo("\\/", x + size, y, foregroundColor, backgroundColor);
    }
}