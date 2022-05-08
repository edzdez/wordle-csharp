namespace Wordle.Core;

public interface IGuessValidator
{
    bool Validate(in string guess);
}