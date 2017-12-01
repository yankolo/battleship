using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    class Ship
    {
        private int _size;
        private bool _isSunk;
        private String _name;

        public Ship(int size)
        {
            _size = size;
            _isSunk = false;
            _name = ShipName(size);
        }

        public int Size { get { return _size; } set { _size = value; } }
        public bool IsSunk { get { return _isSunk; } set { _isSunk = value; } }
        public String NameShip { get { return _name; } }


        private String ShipName(int size)
        {
            String name = "";
            switch (size)
            {
                case 2:
                    name = "Submarine";
                    break;
                case 3:
                    name = "Destroyer";
                    break;
                case 4:
                    name = "Heavy Cruiser";
                    break;
                case 5:
                    name = "Nuclear Aircraft Carrier";
                    break;
            }
            return name;
        }
    }
}
