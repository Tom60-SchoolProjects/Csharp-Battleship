

using Battleship.TUI.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Battleship.TUI;

/// <summary>
/// This class is responsible for drawing the battleground
/// </summary>
internal class Battleground {
    private readonly Ocean ocean;
    private readonly ConsoleBuffer buffer = new();
    private string[] consoleMessage = { "", "", "" };
    private string consoleInput = "";

    public Battleground(uint lignes, uint colonnes) {
        ocean = new(lignes * 2, colonnes);
    }

    public void Update()
    {
        if (Console.KeyAvailable)
        {

            //Console.SetCursorPosition(0, Console.WindowHeight - 1);

            var keyInfo = Console.ReadKey(true);

            switch (keyInfo.Key)
            {
                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;

                case ConsoleKey.Enter:
                    consoleInput = "";
                    break;

                case ConsoleKey.Backspace:
                    if (consoleInput.Length > 0)
                        consoleInput = consoleInput.Remove(consoleInput.Length - 1);
                    break;

                default:
                    consoleInput += keyInfo.KeyChar;
                    break;
            }
        }


        Draw();
    }

    public void WriteMessage(string message)
    {
        var newConsoleMessage = new string[consoleMessage.Length + 1];

        for (int i = 0; i < consoleMessage.Length; i++)
            newConsoleMessage[i] = consoleMessage[i];

        newConsoleMessage[^1] = message;
        consoleMessage = newConsoleMessage;
    }

    private void Draw()
    {
        if (Console.WindowHeight * Console.WindowWidth != buffer.BufferHeight * buffer.BufferWidth)
            buffer.ChangeBufferSize();

        buffer.Clear();

        // Draw UI in priority order
        try
        {
            DrawOcean();
            DrawShips();
            DrawConsole();
        }
        catch (Exception ex)
          when (ex is IndexOutOfRangeException || ex is ArgumentOutOfRangeException)
        {
            buffer.WriteTo("Please resize the window to be bigger", buffer.BufferHeight / 2, buffer.BufferWidth / 2 - 20, ConsoleColor.Red);
        }

        buffer.Flush();

        // Set console cursor and color to default
        Console.SetCursorPosition(0, Console.WindowHeight - 1);
        Console.ResetColor();
    }

    private void DrawOcean() {
        var oceanDraw = ocean.GetNextDraw();
        int offsetX = Convert.ToInt32(buffer.BufferWidth / 2 - ocean.Lignes / 2);
        int offsetY = Convert.ToInt32((buffer.BufferHeight / 2 - ocean.Colonnes / 2) - consoleMessage.Length );

        for (int y = 0; y < ocean.Colonnes + 1; y++) {
            for (int x = 0; x < ocean.Lignes + 1; x++) {

                if (y == 0) // Top
                {
                    var number = (x / 2) + 1;

                    if (x % 2 == 1)
                        buffer.WriteTo(number.ToString("00"), offsetY + y, offsetX + x, ConsoleColor.Black, number % 2 == 0 ? ConsoleColor.Gray : ConsoleColor.White);
                    /*else
                    {
                        var oldChar = buffer.Buffer[buffer.BufferWidth * (offsetY + y) + (offsetX + x)]; // This allow to keep the old char
                        buffer.WriteTo(oldChar, offsetY + y, offsetX + x, ConsoleColor.Black, number % 2 == 1 ? ConsoleColor.Gray : ConsoleColor.White);
                    }*/
                }
                else if (x == 0) // Left
                    buffer.WriteTo((char)(y + 'A' - 1), offsetY + y, offsetX + x, ConsoleColor.Black, y % 2 == 0 ? ConsoleColor.Gray : ConsoleColor.White);
                else
                    buffer.WriteTo(oceanDraw[x - 1, y - 1], offsetY + y, offsetX + x, ConsoleColor.White, ConsoleColor.DarkBlue);

            }
        }
    }

    private void DrawShips() {
        
        int offsetX = Convert.ToInt32(buffer.BufferWidth / 2 - ocean.Lignes / 2);
        int offsetY = Convert.ToInt32(buffer.BufferHeight / 2 - ocean.Colonnes / 2);

        Ship.DrawHorizontalShip(buffer, offsetY + 1, offsetX + 1, 5);
        Ship.DrawVerticalShip(buffer, offsetY + 5, offsetX + 5, 5);

        Ship.DrawHorizontalShip(buffer, offsetY + 5, offsetX + 10, 5, true);
    }

    private void DrawConsole()
    {
        buffer.WriteTo(new string('-', buffer.BufferWidth), buffer.BufferHeight - consoleMessage.Length - 3, 0, ConsoleColor.Gray);
        buffer.WriteTo(new string('-', buffer.BufferWidth), buffer.BufferHeight - consoleMessage.Length - 4, 0, ConsoleColor.Gray);

        for (int i = 0; i < consoleMessage.Length; i++)
            buffer.WriteTo(consoleMessage[i], buffer.BufferHeight - consoleMessage.Length + i - 2, 0, ConsoleColor.Gray);

        buffer.WriteTo(consoleInput, buffer.BufferHeight - 1, 0, ConsoleColor.Gray);
        buffer.WriteTo(new string('-', buffer.BufferWidth), buffer.BufferHeight - 2, 0, ConsoleColor.Gray);
    }

}