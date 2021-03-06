﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    [Serializable]
    public class Ship
    {
        private int _size;
        private int _actualSize;
        private bool _isSunk;
        private String _name;

        public Ship(int size)
        {
            _size = size;
            _actualSize = size;
            _isSunk = false;
            _name = ShipName(size);
        }

        public Ship(Ship copy)
        {
            _size = copy.Size;
            _actualSize = copy.ActualSize;
            _isSunk = copy.IsSunk;
            _name = copy.Name;
        }

        public int Size { get { return _size; } set { _size = value; } }
        public int ActualSize { get { return _actualSize; } }
        public bool IsSunk { get { return _isSunk; } set { _isSunk = value; } }
        public String Name { get { return _name; } }


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
