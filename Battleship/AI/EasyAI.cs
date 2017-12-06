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
        private List<Field> _notHitFieldsList;

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
            // Fill array list that contains all fields that were not hit,
            // generate random index to shoot on the board
            _notHitFieldsList = new List<Field>();

            foreach (Field field in _fieldList)
                if (field.IsHit == false)
                    _notHitFieldsList.Add(field);
            
            Random rand = new Random();
            Field randomField = _notHitFieldsList[rand.Next(0, _notHitFieldsList.Count)];

            return randomField;
        }
    }
}
