using Battleship.API;
using Battleship.TUI;
using System.Diagnostics;
using System.Runtime.InteropServices;

// This is the entry point of the program
while (true)
{
    // Main menu
    await ShowMenuAsync();

    // Game
    await ShowGameAsync(new CancellationTokenSource());
}

/// <summary>
/// Show the main menu
/// </summary>
async Task ShowMenuAsync() {

    var menu = new Menu();

    while (true)
    {
        // Manage input
        if (Console.KeyAvailable)
        {
            var key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;
                
                case ConsoleKey.Enter:
                    return;

                case ConsoleKey.S:
                    OpenUrl("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
                    break;
            }
        }

        // Menu drawing every 250ms
        if (DateTime.Now.Millisecond % 250 <= 10)
            menu.Draw();

        // Refresh speed
        await Task.Delay(10);
    }
}

/// <summary>
/// Show the game
/// </summary>
async Task ShowGameAsync(CancellationTokenSource cancellationToken)
{
    var battleground = new Battleground();

    battleground.MessageSystem.WriteMessage("Battle started!");

    // Main loop
    var backgroundTask = Task.Run(() => {
        while (!cancellationToken.IsCancellationRequested)
        {
            battleground.Update();
        }
    });

    // We ask the players to place their ships
    for (ushort i = 0; i < battleground.game.Fields.Length; i++)
    {
        var field = battleground.game.Fields[i];
        var attempts = 0;
        battleground.ShowLiveShips = false;

        battleground.MessageSystem.WriteMessage($"Player {i + 1} that your turn to setup your ships!");
        battleground.MessageSystem.WriteMessage($"Press any key to continue...");
        Console.ReadKey();

        battleground.ShowLiveShips = true;
        battleground.game.ActivePlayer = i;

        foreach (var ship in field.Ships)
        {
            humremovethatlater:
            battleground.MessageSystem.WriteMessage($"Enter the position you want to place the ship with the size of {ship.Length} ? (ex: E3, I10, ...):");
            var startPos = await AskPositionAsync(battleground);

            List<Direction> directions = field.AvailableDirections(startPos, ship.Length);
            if (directions.Count is 0)
            {
                battleground.MessageSystem.WriteMessage($"You can't place your ship here:");
                goto humremovethatlater;

                attempts += 1;
                if (attempts > 10_000)
                {
                    throw new TimeoutException("Max number of placement attempts reached");
                }

                continue;
            }

            ship.Start = startPos;

            Direction? direction = null;

            while (direction is null && !cancellationToken.IsCancellationRequested)
            {
                battleground.MessageSystem.WriteMessage($"Enter the direction you want to place the ship (available direction: {string.Join(',', directions)}):");
                var directionString = await battleground.MessageSystem.ReadMessage();

                direction = directionString.ToLower() switch
                {
                    "north" => Direction.North,
                    "south" => Direction.South,
                    "east" => Direction.East,
                    "west" => Direction.West,
                    _ => null
                };
            }

            if (direction is not null)
                ship.Direction = direction.Value;

            ship.Visible = true;
        }
    }


    battleground.game.ActivePlayer = 0;

    while (!cancellationToken.IsCancellationRequested) {
        // We warn the player that it's his turn and we wait for him to press a key before showing his ships
        battleground.ShowLiveShips = false;
        battleground.MessageSystem.WriteMessage($"Player {battleground.game.ActivePlayer + 1} it's your turn!");
        battleground.MessageSystem.WriteMessage($"Press any keys to show your ships.");
        Console.ReadKey();

        // Show his ships
        battleground.ShowLiveShips = true;

        battleground.MessageSystem.WriteMessage($"Enter the position you want to attack (ex: E3, I10, ...):");
        var pos = await AskPositionAsync(battleground);

        // Attack the position
        var result = battleground.game.Fields[battleground.game.ActivePlayer == 0 ? (ushort)1 : (ushort)0].ShootAt(pos);
        battleground.game.GetCurrentPlayer().Missed.Add(pos);
        
        // Show the result
        if (result)
            battleground.MessageSystem.WriteMessage($"You hit a ship at {(char)(pos.y + 'A')}{pos.x + 1}!");
        else
            battleground.MessageSystem.WriteMessage($"You missed at {(char)(pos.y + 'A')}{pos.x + 1}!");

        // Check if the game is over
        foreach (var field in battleground.game.Fields)
        {
            if (!field.Ships.Any(ship => ship.Broken.Any(b => !b)))
            {
                // Show the winner
                battleground.MessageSystem.WriteMessage($"Player {battleground.game.ActivePlayer + 1} won the game!");
                battleground.MessageSystem.WriteMessage($"Press any keys to return to the main menu.");

                // Wait for the player to press a key before returning to main menu
                Console.ReadKey();
                cancellationToken.Cancel();
            }
        }

        // Switch player
        battleground.game.SwitchPlayer();
    }
}

async Task<(uint x, uint y)> AskPositionAsync(Battleground battleground)
{
    string playerInput = string.Empty;
    var size = battleground.game.GetCurrentPlayer().Config.Size;
    uint x = uint.MaxValue;
    uint y = uint.MaxValue;

    // Wait for the player to enter a valid position
    while (x == uint.MaxValue && x == y)
    {
        // Read the player input
        playerInput = (await battleground.MessageSystem.ReadMessage()).Trim().ToUpper();

        // Get the position
        try
        {
            // I may of may not have inverted the letters and numbers on the board (letter should be on the top, not on the left) so I have to invert them here
            y = Convert.ToUInt32(playerInput[0] - 'A');
            x = uint.Parse(playerInput[1..]) - 1;

            if (x >= size.X || y >= size.Y)
                throw new ArgumentException("The position is out of range.");
        }
        catch
        {
            battleground.MessageSystem.WriteMessage($"Wrong input.");
            x = uint.MaxValue;
            y = uint.MaxValue;
        }
    }

    return (x, y);
}


// https://stackoverflow.com/a/43232486
/// <summary>
/// Open an URL in the default browser
/// </summary>
/// <param name="url">URL to open</param>
void OpenUrl(string url)
{
    try
    {
        Process.Start(url);
    }
    catch
    {
        // hack because of this: https://github.com/dotnet/corefx/issues/10361
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            url = url.Replace("&", "^&");
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Process.Start("xdg-open", url);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Process.Start("open", url);
        }
        else
        {
            throw;
        }
    }
}
