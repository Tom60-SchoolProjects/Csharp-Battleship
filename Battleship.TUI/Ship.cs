

using System.Drawing;

namespace Battleship.TUI;

/// <summary>
/// This class is responsible fot drawing ships
/// </summary>

internal static class Ship {

    internal static void DrawShip(in ConsoleBuffer buffer, in API.Ship config, uint offsetX, uint offsetY)
    {
        bool horizontal = config.Direction == API.Direction.East || config.Direction == API.Direction.West;
        bool reverse = config.Direction == API.Direction.West || config.Direction == API.Direction.North;

        for (uint i = 0; i < config.Length; i++)
        {
            var broken = config.Broken[i];
            var foregroundColor = broken ? ConsoleColor.DarkGray : ConsoleColor.White;
            var backgroundColor = broken ? ConsoleColor.DarkBlue : ConsoleColor.DarkBlue;

            uint x = (horizontal ? config.Start.x + i : config.Start.x) + offsetX;
            uint y = (horizontal ? config.Start.y : config.Start.y + i) + offsetY;

            string shipPartStart = horizontal ? (broken ? "/" : "<") : (broken ? "/_" : "/\\");
            string shipPartEnd = horizontal ? (broken ? "_" : ">") : (broken ? "v\\ " : " \\/");
            string shipPart = horizontal ? (broken ? "x" : "=") : (broken ? "xx" : "||");

            if (i == 0)
                buffer.WriteTo(shipPartStart, y, x, foregroundColor, backgroundColor);
            else if (Convert.ToUInt32(config.Length) - 1 == i)
                buffer.WriteTo(shipPartEnd, y, x, foregroundColor, backgroundColor);
            else
                buffer.WriteTo(shipPart, y, x, foregroundColor, backgroundColor);
        }
    }

   /* internal static void DrawHorizontalShip(in ConsoleBuffer buffer, uint x, uint y, uint size, bool isSelected = false)
    {
        var foregroundColor = isSelected ? ConsoleColor.Black : ConsoleColor.White;
        var backgroundColor = isSelected ? ConsoleColor.White : ConsoleColor.DarkBlue;
        size *= 2;
        size--;

        buffer.WriteTo("<", x, y, foregroundColor, backgroundColor);

        for (uint i = 1; i < size; i ++)
            buffer.WriteTo("=", x, y + i, foregroundColor, backgroundColor);

        buffer.WriteTo(">", x, y + size, foregroundColor, backgroundColor);
    }

    internal static void DrawVerticalShip(in ConsoleBuffer buffer, uint x, uint y, uint size, bool isSelected = false)
    {
        var foregroundColor = isSelected ? ConsoleColor.Black : ConsoleColor.White;
        var backgroundColor = isSelected ? ConsoleColor.White : ConsoleColor.DarkBlue;
        size--;

        buffer.WriteTo("/\\", x, y, foregroundColor, backgroundColor);

        for (uint i = 1; i < size; i++)
            buffer.WriteTo("||", x + i, y, foregroundColor, backgroundColor);

        buffer.WriteTo("\\/", x + size, y, foregroundColor, backgroundColor);
    }

    internal static void DrawHorizontalShipwreck(in ConsoleBuffer buffer, uint x, uint y, uint size)
    {
        var foregroundColor = ConsoleColor.DarkGray;
        var backgroundColor = ConsoleColor.DarkBlue;

        size *= 2;
        size--;

        buffer.WriteTo("/", x, y, foregroundColor, backgroundColor);

        for (uint i = 1; i < size; i ++)
            buffer.WriteTo("x", x, y + i, foregroundColor, backgroundColor);

        buffer.WriteTo("_", x, y + size, foregroundColor, backgroundColor);
    }

    internal static void DrawVerticalShipwreck(in ConsoleBuffer buffer, uint x, uint y, uint size)
    {
        var foregroundColor = ConsoleColor.DarkGray;
        var backgroundColor = ConsoleColor.DarkBlue;

        size--;

        buffer.WriteTo("/_", x, y, foregroundColor, backgroundColor);

        for (uint i = 1; i < size; i++)
            buffer.WriteTo("xx", x + i, y, foregroundColor, backgroundColor);

        buffer.WriteTo("v\\", x + size, y, foregroundColor, backgroundColor);
    }*/
}