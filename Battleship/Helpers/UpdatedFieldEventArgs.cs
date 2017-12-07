using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class UpdatedFieldEventArgs : EventArgs
    {
        public Field newField;
        public Field oldField;
        public Board board;
    }
}