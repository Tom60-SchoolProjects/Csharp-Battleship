using Battleship;
using Battleship.TUI;
using Battleship.TUI.Enums;
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

async Task ShowGameAsync(CancellationTokenSource cancellationToken) {
    var battleground = new Battleground(); //game.Config.Size.X, game.Config.Size.Y

    battleground.MessageSystem.WriteMessage("Battle started!");


    // Main loop
    var backgroundTask = Task.Run(() => {
        while (!cancellationToken.IsCancellationRequested)
        {
            battleground.Update();
        }
    });


    while (!cancellationToken.IsCancellationRequested) {
        // We warn the player that it's his turn and we wait for him to press a key before showing his ships
        battleground.ShowLiveShips = false;
        battleground.MessageSystem.WriteMessage($"Player {battleground.game.ActivePlayer + 1} that your turn!");
        battleground.MessageSystem.WriteMessage($"Press any keys to show your ships.");
        Console.ReadKey();

        // Show his ships
        battleground.ShowLiveShips = true;

        battleground.MessageSystem.WriteMessage($"Enter the posisition you want to attack (ex: E3, I10, ...):");

        string playerInput = string.Empty;
        uint x = uint.MaxValue;
        uint y = uint.MaxValue;
        var size = battleground.game.GetCurrentPlayer().Config.Size;

        // Wait for the player to enter a valid position
        while (x == uint.MaxValue && x == y)
        {
            // Read the player input
            playerInput = await battleground.MessageSystem.ReadMessage();

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

        // Attack the position
        battleground.MessageSystem.WriteMessage($"Attacking position {playerInput[0]}{y}...");
        var result = battleground.game.Fields[battleground.game.ActivePlayer == 0 ? (ushort)1 : (ushort)0].ShootAt((x, y));
        
        // Show the result
        if (result)
            battleground.MessageSystem.WriteMessage($"You hit a ship!");
        else
            battleground.MessageSystem.WriteMessage($"You missed!");

        // Check if the game is over
        foreach (var field in battleground.game.Fields)
        {
            if (!field.Ships.Any(ship => ship.Broken.Any(b => !b)))
            {
                // Show the winner
                battleground.MessageSystem.WriteMessage($"Player {battleground.game.ActivePlayer + 1} won the game!");
                // Wait for the player to press a key before returning to main menu
                Console.ReadKey();
                cancellationToken.Cancel();
            }
        }

        // Switch player
        battleground.game.SwitchPlayer();
    }
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
