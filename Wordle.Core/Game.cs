namespace Wordle.Core;

public class Game
{
    private const int MaxGuesses = 6;
    private const int LettersInGuess = 5;

    private readonly IWordGenerator _wordGenerator;
    private readonly IGuessValidator _validator;

    public bool Win { get; private set; }
    public bool GameOver { get; private set; }
    public int GuessNum { get; private set; }

    public string Answer { get; private set; } = null!;
    public ISet<char> GuessedCharacters { get; private set; } = null!;
    public Board Board { get; private set; } = null!;

    public Game(IWordGenerator wordGenerator, IGuessValidator validator)
    {
        _wordGenerator = wordGenerator;
        _validator = validator;

        Reset();
    }

    public void Reset()
    {
        GameOver = false;
        Answer = _wordGenerator.NextWord();
        GuessedCharacters = new HashSet<char>();
        Board = new Board(MaxGuesses, LettersInGuess);
        GuessNum = 0;
        Win = false;
    }

    public void MakeGuess(string guess)
    {
        var cells = GradeGuess(guess.ToLower());
        Board.SetRow(GuessNum, cells);

        ++GuessNum;

        if (guess == Answer)
        {
            Win = true;
            GameOver = true;
        }
        else if (GuessNum == 6)
        {
            GameOver = true;
        }
    }

    private IList<Cell> GradeGuess(in string guess)
    {
        var result = guess.Select(c => new Cell(c, GuessStatus.Wrong)).ToList();

        var letterCounts = CountLetters(Answer);
        for (var i = 0; i < result.Count; ++i)
        {
            if (guess[i] == Answer[i])
            {
                --letterCounts[guess[i]];
                result[i].GuessStatus = GuessStatus.Correct;
            }
        }

        for (var i = 0; i < result.Count; ++i)
        {
            if (guess[i] != Answer[i] && letterCounts.ContainsKey(guess[i]) && letterCounts[guess[i]] > 0)
            {
                --letterCounts[guess[i]];
                result[i].GuessStatus = GuessStatus.WrongSpot;
            }
        }

        return result;
    }

    private static Dictionary<char, int> CountLetters(in string word)
    {
        var result = new Dictionary<char, int>();

        foreach (var c in word)
        {
            if (result.ContainsKey(c))
            {
                ++result[c];
            }
            else
            {
                result.Add(c, 1);
            }
        }

        return result;
    }

    public bool ValidateGuess(string guess)
    {
        var guessLower = guess.ToLower();
        return _validator.Validate(in guessLower);
    }
}