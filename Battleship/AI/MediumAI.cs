using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    [Serializable]
    public class MediumAI : IArtificialIntelligence
	{
        private Board _userBoard;
        private IArtificialIntelligence _easyAI;
        private IArtificialIntelligence _hardAI;
        private List<Field> _fieldList;
        private List<Difficulty> _difficltyToUse;

        public MediumAI(Board userBoard)
		{
            _userBoard = userBoard;

            _easyAI = new EasyAI(userBoard);
            _hardAI = new HardAI(userBoard);

            _fieldList = new List<Field>();
            for (int y = 0; y < _userBoard.Height; y++)
                for (int x = 0; x < _userBoard.Width; x++)
                {
                    Coordinate coordinate = new Coordinate(x, y);
                    _fieldList.Add(_userBoard.GetField(coordinate));
                }

            _difficltyToUse = new List<Difficulty>();
            _difficltyToUse.Add(Difficulty.Hard);
            _difficltyToUse.Add(Difficulty.Hard);
            _difficltyToUse.Add(Difficulty.Hard);
            _difficltyToUse.Add(Difficulty.Hard);
            _difficltyToUse.Add(Difficulty.Easy);
        }

		public Difficulty GetDifficutly()
		{
			return Difficulty.Medium;
		}

		public Field Hit()
        {
            Random rand = new Random();
            Difficulty difficutly = _difficltyToUse[rand.Next(0, _difficltyToUse.Count)];

            if (difficutly.Equals(Difficulty.Hard))
                return _hardAI.Hit();
            else
                return _easyAI.Hit();
        }
	}
}
