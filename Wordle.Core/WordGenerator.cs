namespace Wordle.Core;

public class WordGenerator : IWordGenerator
{
    private readonly IList<string> _wordlist;
    private readonly Random _random;

    public WordGenerator(IList<string> wordlist)
    {
        _wordlist = wordlist;
        _random = new Random();
    }

    public string NextWord()
    {
        return _wordlist.ElementAt(_random.Next(_wordlist.Count));
    }
}