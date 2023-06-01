

using System.Diagnostics;
using Battleship.TUI.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Battleship.TUI;

/// <summary>
/// This class is responsible for drawing the battleground
/// </summary>
internal class Battleground {
    private readonly Ocean[] oceans;
    private readonly ConsoleBuffer buffer = new();

    public readonly MessageSystem MessageSystem = new();
    public readonly Game game = new();

    public Battleground() {
        oceans = new Ocean[game.Fields.Length];
        
        for (int i = 0; i < game.Fields.Length; i++)
            oceans[i] = new Ocean(
                Convert.ToUInt32(game.Fields[i].Grid.Length),
                Convert.ToUInt32(game.Fields[i].Grid[0].Length)
            );
    }

    public void Update()
    {
        while (Console.KeyAvailable)
        {
            var keyInfo = Console.ReadKey(true);

            switch (keyInfo.Key)
            {
                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;

                default:
                    MessageSystem.UpdateKey(keyInfo);
                    break;
            }
        }

        // Draw with animation every 500ms
        Draw(DateTime.Now.Millisecond % 500 < 100);
    }

    private void Draw(bool animation = false)
    {
        if (Console.WindowHeight * Console.WindowWidth != buffer.BufferHeight * buffer.BufferWidth)
            buffer.ChangeBufferSize();

        buffer.Clear();

        if (animation)
            foreach (var ocean in oceans)
                ocean.UpdateWind();

        // Draw UI in priority order
        try
        {
            for (int i = 0; i < game.Fields.Length; i++) {
                DrawOcean(i);
                DrawShips(i);
            }

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

    private void DrawOcean(int playerId) {            
        int offsetX = buffer.BufferWidth / game.Fields.Length * playerId - game.Fields[playerId].Grid.GetLength(0) / 2;
        int offsetY = buffer.BufferHeight / game.Fields.Length * playerId - game.Fields[playerId].Grid.GetLength(1) / 2;

        for (int y = 0; y < game.Fields[playerId].Grid.GetLength(0) + 1; y++) {
            for (int x = 0; x < game.Fields[playerId].Grid.GetLength(1) + 1; x++) {

                if (y == 0) // Top
                {
                    var number = (x / 2) + 1;

                    if (x % 2 == 1)
                        buffer.WriteTo(number.ToString("00"), offsetY + y, offsetX + x, ConsoleColor.Black, number % 2 == 0 ? ConsoleColor.Gray : ConsoleColor.White);
                }
                else if (x == 0) // Left
                    buffer.WriteTo((char)(y + 'A' - 1), offsetY + y, offsetX + x, ConsoleColor.Black, y % 2 == 0 ? ConsoleColor.Gray : ConsoleColor.White);
                else
                    buffer.WriteTo(oceans[playerId].windGrid[x - 1, y - 1] ? '~' : '-', offsetY + y, offsetX + x, ConsoleColor.White, ConsoleColor.DarkBlue);

            }
        }
    }

    private void DrawShips(int playerId) {
        // Calulate the offset depending on the player id
        int offsetX = buffer.BufferWidth / game.Fields.Length * playerId - game.Fields[playerId].Grid.GetLength(0) / 2;
        int offsetY = buffer.BufferHeight / game.Fields.Length * playerId - game.Fields[playerId].Grid.GetLength(1) / 2;

        /* int offsetX = buffer.BufferWidth / 2 - game.PlayersField[playerId].Grid.GetLength(0) / 3;
        int offsetY = buffer.BufferHeight / 2 - game.PlayersField[playerId].Grid.GetLength(1) / 3; */

        Ship.DrawHorizontalShip(buffer, offsetY + 1, offsetX + 1, 5);
        Ship.DrawVerticalShip(buffer, offsetY + 5, offsetX + 5, 5);

        Ship.DrawHorizontalShip(buffer, offsetY + 5, offsetX + 10, 5, true);

        Ship.DrawHorizontalShipwreck(buffer, offsetY + 7, offsetX + 12, 5);
        Ship.DrawVerticalShipwreck(buffer, offsetY + 9, offsetX + 2, 5);
    }

    private void DrawConsole()
    {
        buffer.WriteTo(new string('-', buffer.BufferWidth), buffer.BufferHeight - MessageSystem.messageBuffer.Length - 3, 0, ConsoleColor.Gray);
        buffer.WriteTo(new string('-', buffer.BufferWidth), buffer.BufferHeight - MessageSystem.messageBuffer.Length - 4, 0, ConsoleColor.Gray);

        for (int i = 0; i < MessageSystem.messageBuffer.Length; i++)
            buffer.WriteTo(MessageSystem.messageBuffer[i], buffer.BufferHeight - MessageSystem.messageBuffer.Length + i - 2, 0, ConsoleColor.Gray);

        buffer.WriteTo(MessageSystem.consoleInput, buffer.BufferHeight - 1, 0, ConsoleColor.Gray);
        buffer.WriteTo(new string('-', buffer.BufferWidth), buffer.BufferHeight - 2, 0, ConsoleColor.Gray);
    }

}