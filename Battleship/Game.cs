using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    class Game
    {
        private Board _radarBoard;
        private int _shots = 0;
        private int _hits = 0;

        private bool _isGameWon = false;

        public Game()
        {
            _radarBoard = new Board(10, 10);
            ShipFactory.FillBoardRandomly(_radarBoard, 1, 2, 3, 4);
        }

        public HitResult ShootOpponent(Coordinate hitCoordinate)
        {
            Field field = _radarBoard.GetField(hitCoordinate);

            if (_hits >= 30)
            {
                _isGameWon = true;
                // add text output and end game --> ask user to restart or end game.
            }

            if (field.IsShip && !field.IsRevealed)
            {
                field.IsRevealed = true;
                _shots++;
                _hits++;
                return HitResult.ShipShot;
            }
            else if (!field.IsShip && !field.IsRevealed)
            {
                field.IsRevealed = true;
                _shots++;
                return HitResult.WaterShot;
            }
            else if (field.IsRevealed)
            {
                return HitResult.AlreadyRevealed;
            }
            throw new Exception("Error shooting at " + hitCoordinate.X + ", " + hitCoordinate.Y);
        }


        public Board RadarBoard { get { return _radarBoard; } }
        public int Shots { get { return _shots; } set { _shots = value; } }
        public int Hits { get { return _hits; } set { _hits = value; } }
        public bool IsGameWOn { get { return _isGameWon; } }
    }
}
