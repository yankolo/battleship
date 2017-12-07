using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
	[Serializable]
	class Score
	{
		private string _playerName;
		private int _easyWins;
		private int _mediumWins;
		private int _hardWins;
		private int _losses;

		public Score(string playerName, int easyWins, int mediumWins, int hardWins, int losses)
		{
			_playerName = playerName;
			_easyWins = easyWins;
			_mediumWins = mediumWins;
			_hardWins = hardWins;
			_losses = losses;
		}

		public string PlayerName { get { return _playerName; } }
		public int EasyWins { get { return _easyWins; } set { _easyWins = value; } }
		public int MediumWins { get { return _mediumWins; } set { _mediumWins = value; } }
		public int HardWins { get { return _hardWins; } set { _hardWins = value; } }
		public int Losses { get { return _losses; } set { _losses = value; } }
	}
}
