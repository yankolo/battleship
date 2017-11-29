using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace Battleship
{
    class Field
    {
        private bool _isRevealed;
        private bool _isShip;

        public Field(bool isShip)
        {
            _isShip = isShip;
            _isRevealed = false;
        }

        public bool IsRevealed { get { return _isRevealed; } set { _isRevealed = value; } }
        public bool IsShip { get { return _isShip; } set { _isShip = value; } }
    }
}
