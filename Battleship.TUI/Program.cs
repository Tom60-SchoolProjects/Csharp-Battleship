using Battleship.TUI;
using Battleship.TUI.Enums;
using System.Diagnostics;
using System.Runtime.InteropServices;

switch (await ShowMenuAsync()) {
    case MenuOption.Exit:
        return;
    case MenuOption.Start:
        break;
}

var battleground = new Battleground(20, 20);

battleground.WriteMessage("Battle started!");

// Main loop
while (true)
{
    battleground.Update();

    // Refresh speed
    await Task.Delay(1);
}

async Task<MenuOption> ShowMenuAsync() {

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
                    return MenuOption.Exit;
                
                case ConsoleKey.Enter:
                    return MenuOption.Start;
                case ConsoleKey.S:
                    OpenUrl("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
                    break;
            }
        }
        // Menu loop
        menu.Draw();

        // Refresh speed
        await Task.Delay(250);
    }
}

// https://stackoverflow.com/a/43232486
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