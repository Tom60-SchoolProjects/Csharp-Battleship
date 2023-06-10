using System.Diagnostics;

namespace Battleship;

/// <summary>
/// Define a game, with 2 players and their boards
/// </summary>
public class Game {
    #region Properties
    public          ushort         ActivePlayer { get; private set; }
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
    #endregion
}