namespace Wordle.Core;

public class Cell
{
    public char Letter { get; set; }
    public GuessStatus GuessStatus { get; set; }

    public Cell()
    {
        Letter = ' ';
        GuessStatus = GuessStatus.Wrong;
    }
    
    public Cell(char letter, GuessStatus guessStatus)
    {
        Letter = letter;
        GuessStatus = guessStatus;
    }
}