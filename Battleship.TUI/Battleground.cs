

namespace Battleship.TUI;

/// <summary>
/// This class is responsible for drawing the battleground
/// </summary>
internal class Battleground {
    private readonly Ocean ocean;
    private readonly ConsoleBuffer buffer = new();

    public Battleground(uint lignes, uint colonnes) {
        ocean = new(lignes * 2, colonnes);
    }

    public void Draw() {
        buffer.Clear();

        DrawOcean();
        DrawShips();

        buffer.Flush();
    }

    private void DrawOcean() {
        var oceanDraw = ocean.GetNextDraw();
        int offsetX = Convert.ToInt32(Console.BufferWidth / 2 - ocean.Lignes / 2);
        int offsetY = Convert.ToInt32(Console.BufferHeight / 2 - ocean.Colonnes / 2);

        for (int y = 0; y < ocean.Colonnes + 1; y++) {
            for (int x = 0; x < ocean.Lignes + 1; x++) {
                
                if (y == 0) // Top
                    if (x % 2 == 1)
                        buffer.WriteTo(((x / 2) + 1).ToString(), offsetY + y, offsetX + x);
                     else {
                        /* buffer.WriteTo(" ", y, x); */ }
                else if (x == 0) // Left
                    buffer.WriteTo((char)(y + 'A' - 1), offsetY + y, offsetX + x);
                else
                    buffer.WriteTo(oceanDraw[x - 1, y - 1], offsetY + y, offsetX + x, ConsoleColor.Cyan);

            }
        }
    }

    private void DrawShips() {
        
        int offsetX = Convert.ToInt32(Console.BufferWidth / 2 - ocean.Lignes / 2);
        int offsetY = Convert.ToInt32(Console.BufferHeight / 2 - ocean.Colonnes / 2);

        Ship.DrawHorizontalShip(buffer, offsetY + 1, offsetX + 1, 5);
        Ship.DrawVerticalShip(buffer, offsetY + 5, offsetX + 5, 5);

        Ship.DrawHorizontalShip(buffer, offsetY + 5, offsetX + 10, 5, true);
    }
}