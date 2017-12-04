using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
	public class MediumAI : IArtificialIntelligence
	{
		private Board _userBoard;
		private List<Field> _fieldList;

		public MediumAI(Board userBoard)
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
			//Use an algorithm that will determine if the field is a ship
			//if a ship look around as soon as it is finds the another part of the ship continu hitting on the line until there's nothing 
			return null;

		}
		public void DestroyBoat(Coordinate hitBoat)
		{
			//send a coordinate where the boat has been hit, and find where the the next ship is there.


		}
	
	}
}
