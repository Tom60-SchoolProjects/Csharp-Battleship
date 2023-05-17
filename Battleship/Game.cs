using Battleship.API;

namespace Battleship;

public class Game {
    #region Properties
    public  GridConfig Config { get; }
    public  int[,]     Grid   { get; }
    private Random     Random { get; } = new();
    #endregion

    #region Methods
    private void InitGrid() {
        foreach (var ship in Config.Ships) {
            int startX = Random.Next(Config.Size.X),
                startY = Random.Next(Config.Size.Y);

            
        }
    }

    private bool CheckNeighbors(int x, int y) {
        
    }
    #endregion

    #region Constructor
    public Game() {
        Config         = new GridConfig();
        (int x, int y) = Config.Size;
        Grid           = new int[x, y];
    }
    #endregion
}