using Battleship.TUI;
using System.Text;

var playZoneStartX = 1;
var playZoneStartY = 1;
var lignes = 10;
var collones = 10;

var ocean = new Ocean(10 * 2, 10);

while (true)
{
    DrawOcean();

    DrawShip(2, 3);

    await Task.Delay(1000);
}



Console.SetCursorPosition(0, playZoneStartX + lignes + 1);

void DrawOcean()
{
    Console.Clear();

    Console.SetCursorPosition(0, playZoneStartX);

    var drawGrid = ocean.GetNextDraw();

    for (int y = 0; y < collones; y++)
    {
        Console.SetCursorPosition(playZoneStartX, playZoneStartY + y);

        for (int x = 0; x < lignes * 2; x++)
            Console.Write(drawGrid[x, y]);
    }
}

void DrawShip(int x, int y)
{
    Console.SetCursorPosition(x, y);
    Console.Write('^');
    Console.SetCursorPosition(x, y + 1);
    Console.Write('║');
    Console.SetCursorPosition(x, y + 2);
    Console.Write('║');
    Console.SetCursorPosition(x, y + 3);
    Console.Write('║');
    Console.SetCursorPosition(x, y + 4);
    Console.Write('v');
}