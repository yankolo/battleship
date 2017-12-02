using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
   public class Game
    {
        private Board _userBoard;
        private Board _radarBoard;
        private int _userShots = 0;
        private int _userHits = 0;
        private int _cpuShots = 0;
        private int _cpuHits = 0;
        private String _displayedText;

        private bool _isGameWon = false;

        public Game()
        {
            _userBoard = new Board(10, 10, true);
            _radarBoard = new Board(10, 10, false);
            ShipFactory.FillBoardRandomly(_radarBoard, 1, 2, 3, 4);
            ShipFactory.FillBoardRandomly(_userBoard, 1, 2, 3, 4);
        }

        public void ShootOpponent(Coordinate hitCoordinate)
        {
            Field field = _userBoard.GetField(hitCoordinate);

            if (field.Ship != null && !field.IsRevealed)
            {
                field.IsRevealed = true;
                _userShots++;
                _userHits++;
                field.Ship.Size--;
                if (field.Ship.Size ==0)
                {
                    _displayedText = "That's a hit! This " + field.Ship.Name + " has been sunked.";
                }
                else
                {
                   _displayedText = "That's a hit!";

                }
                
            }
            else if (field.Ship == null && !field.IsRevealed)
            {
                field.IsRevealed = true;
                _userShots++;
                _displayedText = "That was a shot in the water!";
            }
            else if (field.IsRevealed)
            {
                _displayedText = "You already shot there.";
            }

            if (_userHits >= 30)
            {
                _isGameWon = true;
                _displayedText = "Congratulations you sunk every ship";

                // add text output and end game --> ask user to restart or end game.
            }
        }
        public void ShootUser(Coordinate hitCoordinate)
        {
            Field field = _userBoard.GetField(hitCoordinate);

            if (field.Ship != null && !field.IsRevealed)
            {
                field.IsRevealed = true;
                _cpuShots++;
                _cpuHits++;
                field.Ship.Size--;
                if (field.Ship.Size == 0)
                {
                    _displayedText = "You have been hit!" + field.Ship.Name + " has been sunked.";
                }
                else
                {
                    _displayedText = "You have been hit!";

                }

            }
            else if (field.Ship == null && !field.IsRevealed)
            {
                field.IsRevealed = true;
                _cpuShots++;
            }
            if (_cpuHits >= 30)
            {
                _isGameWon = true;
                _displayedText = "Sorry the cpu destroyed every ship you have :(";

                // add text output and end game --> ask user to restart or end game.
            }
        }


        public Board RadarBoard { get { return _radarBoard; } }
        public Board UserBoard { get { return _userBoard; } }
        public int UserShots { get { return _userShots; } }
        public int UserHits { get { return _userHits; } }
        public int CpuShots { get { return _cpuShots; } }
        public int CpuHits { get { return _cpuHits; } }
        public bool IsGameWOn { get { return _isGameWon; } }
        public String DisplayedText { get { return _displayedText; } }
    }
}
