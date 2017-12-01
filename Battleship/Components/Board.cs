using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace Battleship
{
    class Board
    {
        private Field[,] _fields;
        private int _width;
        private int _height;

        public Board(int width, int height)
        {
            _width = width;
            _height = height;

            _fields = new Field[width, height];
            InitializeFields();
        }

        public int Width { get { return _width; } }
        public int Height { get { return _height; } }

        private void InitializeFields()
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    _fields[x, y] = new Field();
                }
            }
        }

        public Field GetField(Coordinate coordinate)
        {
            return _fields[coordinate.X, coordinate.Y];
        }

        public Coordinate GetCoordinates(Field field)
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    if (_fields[x, y] == field)
                        return new Coordinate(x, y);
                }
            }
            throw new Exception("Field not found!");
        }
    }
}
