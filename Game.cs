using System;
using System.Collections.Generic;
using System.Text;

namespace StickGame
{

    public class Game
    {
        private readonly Random randomazer;

        public int InitialSticksNumber { get; }

        public Player Turn { get; private set; }

        public int RemainingSticks { get; private set; }
        public int MaxNumberSticksOnOneMove { get; private set; }

        public GameStatus GameStatus { get; private set; }

        public event Action<int> ComputerPlayed;
        public event EventHandler<int> HumanTurnToMAkeMove;

        public event Action<Player> EndOfGame;

        public Game(int initialSticksNumber, Player whoMakesFirstMove, int maxNumberSticksOnOneMove)
        {
            if (initialSticksNumber < 10 || initialSticksNumber > 100)
                throw new ArgumentException("Initial number of stick should be >= 10 and <= 30");

            randomazer = new Random();
            InitialSticksNumber = initialSticksNumber;
            RemainingSticks = initialSticksNumber;
            Turn = whoMakesFirstMove;
            MaxNumberSticksOnOneMove = maxNumberSticksOnOneMove;
        }


        public void Start()
        {
            if (GameStatus == GameStatus.GameOver)
                RemainingSticks = InitialSticksNumber;

            if(GameStatus == GameStatus.InProgress)
            {
                throw new InvalidOperationException("Can't call Start when game is already in progress.");
            }

            GameStatus = GameStatus.InProgress;
            while (GameStatus == GameStatus.InProgress)
            {
                if(Turn == Player.Computer)
                {
                    CompMakesMove();
                }
                else
                {
                    HumanMakesMove();
                }
                FireEndOfGameIfRequired();

                Turn = Turn == Player.Computer ? Player.Human : Player.Computer;

            }
        }

        public void HumanTakes (int sticks)
        {
            if(sticks < 1 || sticks> MaxNumberSticksOnOneMove)
            {
                throw new ArgumentException($"You can take from 1 to {MaxNumberSticksOnOneMove} sticks in a single move");
            }

            if(sticks> RemainingSticks)
            {
                throw new ArgumentException($"you can't take more than remaining. Remains: {RemainingSticks}");
            }
            TakesSticks(sticks);
        }


        private void FireEndOfGameIfRequired()
        {
           if(RemainingSticks == 0)
            {
                GameStatus = GameStatus.GameOver;
                if (EndOfGame != null)
                    EndOfGame(Turn == Player.Computer ? Player.Human : Player.Computer);
            }
        }

        private void HumanMakesMove()
        {
            if (HumanTurnToMAkeMove != null)
                HumanTurnToMAkeMove(this, RemainingSticks);
        }

        private void CompMakesMove()
        {
            int maxNumber = RemainingSticks >= MaxNumberSticksOnOneMove ? MaxNumberSticksOnOneMove : RemainingSticks;
            int sticks = 0;

            if (RemainingSticks == maxNumber)
            {
                sticks = maxNumber - 1;
            }
            else if (InitialSticksNumber == RemainingSticks || RemainingSticks % (MaxNumberSticksOnOneMove+1) == 0)
            {
                sticks = randomazer.Next(1, maxNumber);
            }
            else
            {
                sticks = RemainingSticks % (MaxNumberSticksOnOneMove+1);
            }

            TakesSticks(sticks);
            if (ComputerPlayed != null)
                ComputerPlayed(sticks);
        }

        private void TakesSticks(int sticks)
        {
            RemainingSticks -= sticks;
        }
    }
}
