using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace Battleship
{
    [Serializable]
    public class Field
    {
        private bool _isRevealed;
        private Ship _ship;
		private bool _isHit;

        public Field(bool isRevealed)
        {
            _isRevealed = isRevealed;
			_isHit = false;
        }
        
        public Field (Field copy)
        {
            _isRevealed = copy.IsRevealed;
			_isHit = copy._isHit;
            if (copy.Ship != null)
                _ship = new Ship(copy.Ship);
        }

        public bool IsRevealed { get { return _isRevealed; } set { _isRevealed = value; } }
		public bool IsHit { get { return _isHit; } set { _isHit = value; } }
		public Ship Ship { get { return _ship; } set { _ship = value; } }
    }
}
