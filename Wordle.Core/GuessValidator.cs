namespace Wordle.Core;

public class GuessValidator : IGuessValidator
{
    private readonly ISet<string> _validGuesses;

    public GuessValidator(ISet<string> validGuesses)
    {
        _validGuesses = validGuesses;
    }

    public bool Validate(in string guess)
    {
        return _validGuesses.Contains(guess);
    }
}