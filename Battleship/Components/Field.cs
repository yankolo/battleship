using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace Battleship
{
    public class Field
    {
        private bool _isRevealed;
        private Ship _ship;

        public Field(bool isRevealed)
        {
            _isRevealed = isRevealed;
        }
        
        public Field (Field copy)
        {
            _isRevealed = copy.IsRevealed;
            if (copy.Ship != null)
                _ship = new Ship(copy.Ship);
        }

        public bool IsRevealed { get { return _isRevealed; } set { _isRevealed = value; } }
        public Ship Ship { get { return _ship; } set { _ship = value; } }
    }
}
