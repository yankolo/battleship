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
        private int _shipFieldPosition;
        private Direction _shipDirection;
		private bool _isHit;

        public Field(bool isRevealed)
        {
            _isRevealed = isRevealed;
			_isHit = false;
            _shipFieldPosition = 0;
        }
        
        public Field (Field copy)
        {
            _isRevealed = copy.IsRevealed;
			_isHit = copy._isHit;
            if (copy.Ship != null)
                _ship = new Ship(copy.Ship);
            _shipDirection = copy.ShipDirection;
            _shipFieldPosition = copy.ShipFieldPosition;
        }


        public bool IsRevealed { get { return _isRevealed; } set { _isRevealed = value; } }
		public bool IsHit { get { return _isHit; } set { _isHit = value; } }
		public Ship Ship { get { return _ship; } set { _ship = value; } }
        public int ShipFieldPosition { get { return _shipFieldPosition; } set { _shipFieldPosition = value; } }
        public Direction ShipDirection { get { return _shipDirection; } set { _shipDirection = value; } }
    }
}
