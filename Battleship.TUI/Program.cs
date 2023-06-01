using Battleship;
using Battleship.TUI;
using Battleship.TUI.Enums;
using System.Diagnostics;
using System.Runtime.InteropServices;

// Main menu
await ShowMenuAsync();

// Game
await ShowGameAsync();

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
        if (DateTime.Now.Millisecond % 250 < 100)
            menu.Draw();

        // Refresh speed
        await Task.Delay(10);
    }
}

async Task ShowGameAsync() {
    var battleground = new Battleground(); //game.Config.Size.X, game.Config.Size.Y

    battleground.MessageSystem.WriteMessage("Battle started!");


    // Main loop
    var backgroundTask = Task.Run(() => {
        while (true)
        {
            battleground.Update();
        }
    });


    while (true) {
        var msg = await battleground.MessageSystem.ReadMessage();
        battleground.MessageSystem.WriteMessage(msg);
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
