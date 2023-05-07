using System;

namespace StickGame
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Game game = new Game(20, Player.Computer, 3);
            game.ComputerPlayed += Game_ComputerPlayed;
            game.HumanTurnToMAkeMove += Game_HumanTurnToMAkeMove;
            game.EndOfGame += Game_EndOfGame;

            game.Start();
        }

        private static void Game_EndOfGame(Player player)
        {
            Console.WriteLine($"Winner: {player}");
        }

        private static void Game_HumanTurnToMAkeMove(object sender, int remainingSticks)
        {
            Console.WriteLine($"Remaining sticks: {remainingSticks}");
            Console.WriteLine("Take some stiks");

            bool takenCorrectly = false;
            while (!takenCorrectly)
            {
                if (int.TryParse(Console.ReadLine(), out int takenSticks))
                {
                    var game = (Game)sender;

                    try
                    {
                        game.HumanTakes(takenSticks);
                        takenCorrectly = true;
                    }
                    catch (ArgumentException ex)
                    {

                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        private static void Game_ComputerPlayed(int sticksTaken)
        {
            Console.WriteLine($"Machine took: {sticksTaken}");
        }
    }
}
