namespace Battleship; 

public class Game {
    public            ushort         ActivePlayer  { get; private set; } 
    public  readonly  PlayerField[]  Fields;

    public Game() {
        ActivePlayer = 0;
        
        Fields = new PlayerField[2];
        Fields[0] = new PlayerField();
        Fields[1] = new PlayerField();
    }

    public void SwitchPlayer()
    {
        ActivePlayer = ActivePlayer == 0 ? (ushort) 1 : (ushort) 0;
    }

    public PlayerField GetCurrentPlayer() => Fields[ActivePlayer];
}