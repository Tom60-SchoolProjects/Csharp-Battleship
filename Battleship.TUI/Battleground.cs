namespace Battleship.TUI;

/// <summary>
/// This class is responsible for drawing the battleground
/// </summary>
internal class Battleground {
    private readonly Ocean[] oceans;
    private readonly ConsoleBuffer buffer = new();

    public readonly MessageSystem MessageSystem = new();
    public readonly Game game = new();
    public bool ShowLiveShips;

    //private bool[,] hits;

    public Battleground() {
        oceans = new Ocean[game.Fields.Length];
        
        for (int i = 0; i < game.Fields.Length; i++)
            oceans[i] = new Ocean(
                Convert.ToUInt32(game.Fields[i].Config.Size.X) * 2,
                Convert.ToUInt32(game.Fields[i].Config.Size.Y)
            );

        //hits = new bool[game.Fields[0].Config.Size.X * 2, game.Fields[0].Config.Size.Y];
    }

    /*public void Hit(uint x, uint y)
    {
        hits[x * 2, y] = true;
        hits[(x * 2) + 1, y] = true;
    }*/

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
            for (uint i = 0; i < game.Fields.Length; i++)
            {
                uint offsetX = Convert.ToUInt32(buffer.BufferWidth / (game.Fields.Length + 1)) * (i + 1) - game.Fields[i].Config.Size.X / 2;
                uint offsetY = Convert.ToUInt32((buffer.BufferHeight / 2 - game.Fields[i].Config.Size.X * 2 / 2) - MessageSystem.messageBuffer.Length);

                DrawOcean(i, offsetX, offsetY);
                DrawShips(i, offsetX + 1, offsetY + 1);
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

    private void DrawOcean(uint playerId, uint offsetX, uint offsetY) {
        for (uint x = 0; x < game.Fields[playerId].Config.Size.X * 2 + 1; x++) {
            for (uint y = 0; y < game.Fields[playerId].Config.Size.Y + 1; y++) {

                if (y == 0) // Top
                {
                    var number = (x / 2) + 1;

                    if (x % 2 == 1)
                        buffer.WriteTo(number.ToString("00"), offsetY + y, offsetX + x, ConsoleColor.Black, number % 2 == 0 ? ConsoleColor.Gray : ConsoleColor.White);
                }
                else if (x == 0) // Left
                    buffer.WriteTo((char)(y + 'A' - 1), offsetY + y, offsetX + x, ConsoleColor.Black, y % 2 == 0 ? ConsoleColor.Gray : ConsoleColor.White);
                else
                {
                    if (game.Fields[playerId].Missed.Contains((Convert.ToUInt32(Math.Floor((x - 1) / 2f) * 2), y - 1)))
                        buffer.WriteTo(oceans[playerId].windGrid[x - 1, y - 1] ? '~' : '�',
                            offsetY + y, offsetX + x,
                            ConsoleColor.White, ConsoleColor.DarkGray);
                    else
                        buffer.WriteTo(oceans[playerId].windGrid[x - 1, y - 1] ? '~' : '-',
                            offsetY + y, offsetX + x,
                            ConsoleColor.White, ConsoleColor.DarkBlue);
                }

            }
        }
    }

    private void DrawShips(uint playerId, uint offsetX, uint offsetY)
    {
        foreach(var ship in game.Fields[playerId].Ships)
        {
            if (ship.Visible)            
                Ship.DrawShip(buffer, ship, offsetX, offsetY, playerId != game.ActivePlayer || !ShowLiveShips);
        }
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