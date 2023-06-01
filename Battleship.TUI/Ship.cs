

namespace Battleship.TUI;

/// <summary>
/// This class is responsible fot drawing ships
/// </summary>

internal static class Ship {

    internal static void DrawHorizontalShip(in ConsoleBuffer buffer, int x, int y, int size, bool isSelected = false)
    {
        var foregroundColor = isSelected ? ConsoleColor.Black : ConsoleColor.White;
        var backgroundColor = isSelected ? ConsoleColor.White : ConsoleColor.DarkBlue;
        size *= 2;
        size--;

        buffer.WriteTo("<", x, y, foregroundColor, backgroundColor);

        for (int i = 1; i < size; i ++)
            buffer.WriteTo("=", x, y + i, foregroundColor, backgroundColor);

        buffer.WriteTo(">", x, y + size, foregroundColor, backgroundColor);
    }

    internal static void DrawVerticalShip(in ConsoleBuffer buffer, int x, int y, int size, bool isSelected = false)
    {
        var foregroundColor = isSelected ? ConsoleColor.Black : ConsoleColor.White;
        var backgroundColor = isSelected ? ConsoleColor.White : ConsoleColor.DarkBlue;
        size--;

        buffer.WriteTo("/\\", x, y, foregroundColor, backgroundColor);

        for (int i = 1; i < size; i++)
            buffer.WriteTo("||", x + i, y, foregroundColor, backgroundColor);

        buffer.WriteTo("\\/", x + size, y, foregroundColor, backgroundColor);
    }

    internal static void DrawHorizontalShipwreck(in ConsoleBuffer buffer, int x, int y, int size)
    {
        var foregroundColor = ConsoleColor.DarkGray;
        var backgroundColor = ConsoleColor.DarkBlue;

        size *= 2;
        size--;

        buffer.WriteTo("/", x, y, foregroundColor, backgroundColor);

        for (int i = 1; i < size; i ++)
            buffer.WriteTo("x", x, y + i, foregroundColor, backgroundColor);

        buffer.WriteTo("_", x, y + size, foregroundColor, backgroundColor);
    }

    internal static void DrawVerticalShipwreck(in ConsoleBuffer buffer, int x, int y, int size)
    {
        var foregroundColor = ConsoleColor.DarkGray;
        var backgroundColor = ConsoleColor.DarkBlue;

        size--;

        buffer.WriteTo("/_", x, y, foregroundColor, backgroundColor);

        for (int i = 1; i < size; i++)
            buffer.WriteTo("xx", x + i, y, foregroundColor, backgroundColor);

        buffer.WriteTo("v\\", x + size, y, foregroundColor, backgroundColor);
    }
}