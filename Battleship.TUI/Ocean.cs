using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.TUI
{
    internal class Ocean
    {
        internal uint Lignes { get; init; }
        internal uint Colonnes { get; init; }

        private int windSpeedX = 1;
        private int windSpeedY = 1;

        internal bool[,] windGrid { get; private set; }

        internal Ocean(uint lignes, uint colonnes)
        {
            Lignes = lignes;
            Colonnes = colonnes;

            var random = new Random();
            windGrid = new bool[Lignes, Colonnes];

            for (uint y = 0; y < Colonnes; y++)
            {
                for (uint x = 0; x < Lignes; x++)
                {
                    if (random.Next(0, 5) == 0)
                        windGrid[x, y] = true;
                }
            }
        }

        internal void UpdateWind()
        {
            var newWindGrid = new bool[Lignes, Colonnes];

            for (uint y = 0; y < Colonnes; y++)
            {
                for (uint x = 0; x < Lignes; x++)
                {
                    var windPosX = x + windSpeedX;
                    var windPosY = y + windSpeedY;

                    if (windPosX >= Lignes)
                        windPosX -= Lignes;

                    if (windPosY >= Colonnes)
                        windPosY -= Colonnes;

                    if (windGrid[x, y])
                        newWindGrid[windPosX, windPosY] = true;
                }
            }

            windGrid = newWindGrid;
        }

        /* internal char[,] GetNextDraw()
        {
            UpdateWind();

            var grid = new char[Lignes, Colonnes];

            for (uint y = 0; y < Colonnes; y++)
            {
                for (uint x = 0; x < Lignes; x++)
                {
                    grid[x, y] = windGrid[x, y] ? '~' : '-';
                }
            }

            return grid;
        } */
    }
}
