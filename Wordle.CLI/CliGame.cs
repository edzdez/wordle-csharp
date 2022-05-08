using Wordle.Core;

namespace Wordle.CLI;

public class CliGame
{
    private readonly Game _game;

    private readonly HashSet<ConsoleKey> _actionKeys = new()
    {
        ConsoleKey.Y,
        ConsoleKey.N,
    };

    public CliGame()
    {
        _game = new Game(
            new WordGenerator(ReadListFromFile("assets/possible_answers.txt")),
            new GuessValidator(
                new HashSet<string>(ReadListFromFile("assets/possible_guesses.txt"))
            )
        );
    }

    private int PlayGame()
    {
        while (!_game.GameOver)
        {
            DisplayBoard();
            while (!ReadGuess())
            {
                Thread.Sleep(500);
                DisplayBoard();
            }
        }

        return _game.GuessNum;
    }

    private void DisplayBoard()
    {
        for (var i = 0; i < 6; ++i)
        {
            Console.Clear();
            Console.WriteLine("       \x1B[4mWordle\x1B[24m");
            Console.WriteLine("Press Ctrl-C to quit.");

            Console.WriteLine();

            Console.WriteLine("+---+---+---+---+---+");
            foreach (var guess in _game.Board)
            {
                Console.Write("|");
                foreach (var cell in guess)
                {
                    Console.BackgroundColor = cell.GuessStatus switch
                    {
                        GuessStatus.Correct => ConsoleColor.Green,
                        GuessStatus.WrongSpot => ConsoleColor.Yellow,
                        _ => ConsoleColor.DarkGray
                    };

                    Console.ForegroundColor = cell.GuessStatus switch
                    {
                        GuessStatus.Correct => ConsoleColor.Black,
                        GuessStatus.WrongSpot => ConsoleColor.Black,
                        _ => ConsoleColor.White
                    };

                    Console.Write($" {cell.Letter} ");
                    Console.ResetColor();
                    Console.Write('|');
                }

                Console.WriteLine();
                Console.WriteLine("+---+---+---+---+---+");
            }

            Console.WriteLine();
        }
    }

    private bool ReadGuess()
    {
        var guess = Console.ReadLine();
        if (_game.ValidateGuess(guess!))
        {
            _game.MakeGuess(guess!);
            return true;
        }
        else
        {
            DisplayBoard();

            Console.WriteLine("Invalid Guess!");
            return false;
        }
    }

    public void Run()
    {
        var stop = false;
        while (!stop)
        {
            var moves = PlayGame();
            
            DisplayBoard();
            Console.WriteLine(
                $"You {(_game.Win ? $"Won with {moves} guesses!" : $"Lost! The correct answer was {_game.Answer}.")}"
            );
            Console.WriteLine("Play again? (y/n)");

            ConsoleKey key;
            do
            {
                key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.N)
                {
                    stop = true;
                }
                else
                {
                    _game.Reset();
                    break;
                }
            } while (!_actionKeys.Contains(key));
        }
    }

    private static IList<string> ReadListFromFile(string filename)
        => File.ReadAllLines(filename).ToList();
}