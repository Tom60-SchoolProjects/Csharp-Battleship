using System.Diagnostics;
using Battleship.API;

namespace Battleship;

/// <summary>
/// Define a game, with 2 players and their boards
/// </summary>
public class Game {
    #region Properties
    public          ushort         ActivePlayer { get; /*private*/ set; } // Same, not a good things to do
    public readonly PlayerField[ ] Fields;
    #endregion

    #region Methods
    /// <summary>
    /// Change the active player
    /// </summary>
    public void SwitchPlayer() {
        ActivePlayer = ActivePlayer switch {
                           0 => 1,
                           1 => 0,
                           _ => throw new UnreachableException(),
                       };
    }

    public PlayerField GetCurrentPlayer()
        => Fields[ActivePlayer];
    #endregion

    #region Constructors
    public Game() {
        ActivePlayer = 0;

        Fields    = new PlayerField[2];
        Fields[0] = new PlayerField();
        Fields[1] = new PlayerField();
    }

    // Sorry Athur but that add too much work for me
    /*public Game(List<Ship> ships) {
        ActivePlayer = 0;

        Fields    = new PlayerField[2];
        Fields[0] = new PlayerField(ships);
        Fields[1] = new PlayerField();
    }

    public Game(List<Ship> ships1, List<Ship> ships2) {
        ActivePlayer = 0;

        Fields    = new PlayerField[2];
        Fields[0] = new PlayerField(ships1);
        Fields[1] = new PlayerField(ships2);
    }*/
    #endregion
}