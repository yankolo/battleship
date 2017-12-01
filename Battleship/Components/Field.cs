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
        private Ship _ship;

        public Field()
        {
            _isRevealed = false;
        }

        public bool IsRevealed { get { return _isRevealed; } set { _isRevealed = value; } }
        public Ship Ship { get { return _ship; } set { _ship = value; } }
    }
}
