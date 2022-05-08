using System.Collections;

namespace Wordle.Core;

public class Board : IEnumerable<IEnumerable<Cell>>
{
    public int Rows { get; init; }
    public int Cols { get; init; }

    private readonly List<List<Cell>> _board;

    public Board(int maxGuesses, int lettersInGuess)
    {
        Rows = maxGuesses;
        Cols = lettersInGuess;

        _board = Enumerable.Range(0, Rows).Select(_ =>
            Enumerable.Range(0, Cols).Select(_ =>
                new Cell()
            ).ToList()
        ).ToList();
    }

    public void SetRow(int idx, IList<Cell> c)
    {
        for (var i = 0; i < c.Count; ++i)
        {
            _board[idx][i] = c[i];
        }
    }

    public IEnumerator<IEnumerable<Cell>> GetEnumerator()
    {
        return _board.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}