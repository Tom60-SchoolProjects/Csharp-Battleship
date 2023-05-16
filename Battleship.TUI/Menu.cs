using System;using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.TUI;

internal class Menu
{
    private int waveOffset = 0;
    private ConsoleBuffer buffer = new ConsoleBuffer();

    public void Draw()
    {
        buffer = new();

        waveOffset = waveOffset > buffer.BufferWidth ? 0 : waveOffset + 1;

        try {
            DrawWave(3, buffer.BufferHeight - 11, 0.30, waveOffset * 1, ConsoleColor.DarkGray);

            DrawWave(4, buffer.BufferHeight - 9, 0.20, waveOffset * 1.5, ConsoleColor.Blue);

            DrawTitle(buffer.BufferHeight - 19);
            DrawMenu(buffer.BufferHeight / 3);

            DrawWave(5, buffer.BufferHeight - 7, 0.18, waveOffset * 2, ConsoleColor.DarkCyan);
        } catch (Exception ex) when (ex is IndexOutOfRangeException ||
                                     ex is ArgumentOutOfRangeException) {
            buffer.Clear();
            buffer.WriteTo("Please resize the window to be bigger", buffer.BufferHeight / 2, buffer.BufferWidth / 2 - 20, ConsoleColor.Red);
        }

        buffer.Flush();

        // Set console cursor and color to default
        Console.SetCursorPosition(0, Console.BufferHeight - 1);
        Console.ResetColor();
    }

    private void DrawTitle(int startY)
    {
        var title = new string[] {
            @"                                     # #  ( )",
            @"                                  ___#_#___|__",
            @"                              _  |____________|  _",
            @"                       _=====| | |            | | |==== _",
            @"                 =====| |.---------------------------. | |====",
            @"   <--------------------'   .  .  .  .  .  .  .  .   '--------------/   ",
            @"     \                             Battleship                      /",
            @"      \___________________________________________________________/" };

        int titleWitdh = 72;

        for (int i = 0; i < title.Length; i++)
        {
            buffer.WriteTo(title[i], startY + i, buffer.BufferWidth / 2 - titleWitdh / 2, ConsoleColor.Gray);
        }

    }

    private void DrawMenu(int startY)
    {
        var menu = new string[] {
            @" .---------------------------------.",
            @" | Press Enter to start a new game |",
            @" |     Press S to put on music     |",
            @" '---------------------------------'", };

        int menuWitdh = menu[0].Length;

        for (int i = 0; i < menu.Length; i++)
        {
            // buffer.BufferHeight / 3 + 10
            buffer.WriteTo(menu[i], startY + i, buffer.BufferWidth / 2 - menuWitdh / 2, ConsoleColor.Gray);
        }
    }

    private void DrawBackground()
    {
        /*Console.ForegroundColor = ConsoleColor.Black;
        DrawWave(5, Console.BufferHeight - 7, 0.18, waveOffset);*/

        waveOffset = waveOffset > Console.BufferWidth ? 0 : waveOffset + 1;

        Console.ForegroundColor = ConsoleColor.DarkGray;
        DrawWave(3, Console.BufferHeight - 10, 0.30, waveOffset * 1);

        Console.ForegroundColor = ConsoleColor.Blue;
        DrawWave(4, Console.BufferHeight - 8, 0.20, waveOffset * 1.5);

        Console.ForegroundColor = ConsoleColor.DarkCyan;
        DrawWave(5, Console.BufferHeight + 10, 0.18, waveOffset * 2);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="height"></param>
    /// <param name="y">The bottom position of the wave</param>
    private void DrawWave(int magnitude, int startY, double euh, double offset, ConsoleColor waveColor = ConsoleColor.Gray, bool drawInside = false)
    {
        //Console.CursorLeft = 0;

        // y to y + height
        for (int x = 0; x < buffer.BufferWidth; x++)
        {
            int y = startY + (int)Math.Round(Math.Sin(x * euh - offset) * magnitude, MidpointRounding.AwayFromZero);

            // Avoid some ArgumentOutOfRangeException
            if (0 > y || y >= buffer.BufferHeight) continue;

            /*Console.CursorTop = y;
            Console.Write('~');*/

            buffer.WriteTo('~', y, x, waveColor);
            /* Console.Move

            //because output appends, ensure the window is reset
            using (Stream stdout = Console.OpenStandardOutput(cols * rows))
            {
                stdout.Write(buffer, 0, buffer.Length);
            }*/

            if (drawInside)
                for (int subY = y + 1; subY < buffer.BufferHeight; subY++)
                {
                    buffer.WriteTo('*', x, subY, waveColor);
                    /*Console.SetCursorPosition(x, subY);
                    Console.Write('*');*/
                }
        }
            
    }
}