using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.AI
{
    public class EasyAI
    {
        private Board _userBoard;
        public EasyAI(Board userBoard)
        {
            _userBoard = userBoard;
        }

        public void RandomHit()
        {
            // Make an array list that contains all fields of the user board 
            // generate random index to shoot on the board, once it hits remove that field from the list until every ship sunk.
        }


    }
}
