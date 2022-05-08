using Wordle.CLI;

Console.CursorVisible = false;

var game = new CliGame();
game.Run();

Console.CursorVisible = true;
