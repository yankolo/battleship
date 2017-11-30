using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    class NoShipSpaceException : Exception
    {
        public NoShipSpaceException() : base("No more space for ships on board")
        {

        }
    }
}
