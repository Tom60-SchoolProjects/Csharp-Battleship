namespace Battleship; 

public class Game {
    public  bool        TurnP1    { get; } 
    private PlayerField Player1 { get; }
    private PlayerField Player2 { get; }

    public Game() {
        TurnP1 = true;
        
        Player1 = new PlayerField();
        Player2 = new PlayerField();
    }
}