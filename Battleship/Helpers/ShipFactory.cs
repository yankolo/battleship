using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    static class ShipFactory
    {
        public static void FillBoardRandomly(Board board)
        {
            try {
            GenerateShip(board, 5);

            GenerateShip(board, 4);
            GenerateShip(board, 4);

            GenerateShip(board, 3);
            GenerateShip(board, 3);
            GenerateShip(board, 3);

            GenerateShip(board, 2);
            GenerateShip(board, 2);
            GenerateShip(board, 2);
            GenerateShip(board, 2);
            }
            // redo when failed to put ships
            catch (Exception)
            {
                ResetBoard(board);
            }
        }

        private static void GenerateShip(Board board, int size, int trialCounter = 0)
        {
            if (trialCounter > 50)
            {
                throw new Exception("Cannot Place Ship");
            }
            trialCounter++;
            Random rand = new Random();
            int num1 = rand.Next(0, 10);
            int num2 = rand.Next(0, 10);
            bool horizontal = rand.Next(0, 2) == 0;
            try
            {
                EnsureIsValidField(board, num1, num2);
                for (int i = 0; i < size; i++)
                {
                    if (horizontal)
                    {
                        EnsureIsValidField(board, num1 + i, num2);
                        EnsureIsValidField(board, num1 + i + 1, num2, true);
                        EnsureIsValidField(board, num1 + i - 1, num2, true);
                        EnsureIsValidField(board, num1 + i, num2 + 1, true);
                        EnsureIsValidField(board, num1 + i, num2 - 1, true);
                    }
                    else
                    {
                        EnsureIsValidField(board, num1, num2 + i);
                        EnsureIsValidField(board, num1, num2 + i + 1, true);
                        EnsureIsValidField(board, num1, num2 + i - 1, true);
                        EnsureIsValidField(board, num1 + 1, num2 + i, true);
                        EnsureIsValidField(board, num1 - 1, num2 + i, true);
                    }

                }
            }
            catch (Exception)
            {
                GenerateShip(board, size, trialCounter);
                return;
            }

            if (horizontal)
            {
                for (var i = 0; i < size; i++)
                    board.GetField(new Coordinate(num1 + i, num2)).IsShip = true;
            }
            else
            {
                for (var i = 0; i < size; i++)
                    board.GetField(new Coordinate(num1, num2 + i)).IsShip = true;
            }
        }

        private static void EnsureIsValidField(Board board, int x, int y, bool isSurrounding = false)
        {
            if (isSurrounding)
            {
                if (x > board.Width || y > board.Height)
                    throw new Exception("Not Valid");
            }
            else if (!isSurrounding)
            {
                if (x >= board.Width || y >= board.Height)
                    throw new Exception("Not Valid");
            }
            if (x < board.Width && y < board.Height)
            {
                if (board.GetField(new Coordinate(x, y)).IsShip == true)
                    throw new Exception("Not Valid");
            }
        }

        private static void ResetBoard(Board board)
        {
            for (int x = 0; x < board.Width; x++)
                for (int y = 0; y < board.Height; y++)
                    board.GetField(new Coordinate(x, y)).IsShip = false;
        }
    }
}
