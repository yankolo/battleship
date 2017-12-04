using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class EasyAI : IArtificialIntelligence
    {
        private Board _userBoard;
        private List<Field> _fieldList;

        public EasyAI(Board userBoard)
        {
            _userBoard = userBoard;

            _fieldList = new List<Field>();
            for (int y = 0; y < _userBoard.Height; y++)
                for (int x = 0; x < _userBoard.Width; x++)
                {
                    Coordinate coordinate = new Coordinate(x, y);
                    _fieldList.Add(_userBoard.GetField(coordinate));
                }

        }

        public Field Hit()
        {
            // Make an array list that contains all fields of the user board 
            // generate random index to shoot on the board, once it hits remove that field from the list until every ship sunk.

            Random rand = new Random();
            Field randomField = _fieldList[rand.Next(0, _fieldList.Count)];
            _fieldList.Remove(randomField);

            return randomField;
        }
    }
}
