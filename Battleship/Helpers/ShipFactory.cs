using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    static class ShipFactory
    {
        public static void FillBoardRandomly(Board board, int numberOfSize5, int numberOfSize4, int numberOfSize3, int numberOfSize2)
        {
            try
            {
                int[] arrayNumberOfSize = { numberOfSize5, numberOfSize4, numberOfSize3, numberOfSize2 };
                for (int i = 0; i < arrayNumberOfSize.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                            for (int j = 0; j < arrayNumberOfSize[i]; j++)
                            {
                                ValidGenerateShip(board, 5);
                            }
                            break;
                        case 1:
                            for (int j = 0; j < arrayNumberOfSize[i]; j++)
                            {
                                GenerateShip(board, 4);
                            }
                            break;
                        case 2:
                            for (int j = 0; j < arrayNumberOfSize[i]; j++)
                            {
                                GenerateShip(board, 3);
                            }
                            break;
                        case 3:
                            for (int j = 0; j < arrayNumberOfSize[i]; j++)
                            {
                                GenerateShip(board, 2);
                            }
                            break;
                    }
                }
            } 
            // redo when failed to put ships
            catch (Exception)
            {
                ResetBoard(board);
                FillBoardRandomly(board, numberOfSize5, numberOfSize4, numberOfSize3, numberOfSize2);
            }
        }

        public static void ValidGenerateShip(Board board , int size)
        {
            try
            { 
                int trialCounter = 0;
                bool valid = GenerateShip(board, size);
                trialCounter++;
                while (!valid)
                {
                    valid = GenerateShip(board , size, trialCounter);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static bool GenerateShip(Board board, int size, int trialCounter = 0)
        {
            if (trialCounter > 50)
            {
                throw new Exception("Cannot Place Ship");
            }
            trialCounter++;
            
            Random rand = new Random();
            int num1 = rand.Next(0, 10);
            int num2 = rand.Next(0, 10);

            Direction direction;

            if (rand.Next(0, 2) == 0)
                direction = Direction.Horizontal;
            else
                direction = Direction.Vertical;
           

            for (int i = 0; i < size; i++)
            {
                if (direction.Equals(Direction.Horizontal))
                {
                    if (VerifyShipPlacement(board, new Coordinate(num1 + i, num2)) == false || VerifySurrounding(board, new Coordinate(num1 + i, num2)) == false)
                        return false;
                }
                else if (direction.Equals(Direction.Vertical))
                {
                    if (VerifyShipPlacement(board, new Coordinate(num1, num2 + i)) == false || VerifySurrounding(board, new Coordinate(num1, num2 + i)) == false)
                        return false;
                }

            }

            if (direction.Equals(Direction.Horizontal))
                {
                for (var i = 0; i < size; i++)
                    board.GetField(new Coordinate(num1 + i, num2)).IsShip = true;
            }
            else if (direction.Equals(Direction.Vertical))
            {
                for (var i = 0; i < size; i++)
                    board.GetField(new Coordinate(num1, num2 + i)).IsShip = true;
            }

            return true;
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

        private static bool VerifyShipPlacement(Board board, Coordinate coordinate)
        {
            if (board.GetField(coordinate).IsShip)
                return false;
            return true;
        }

        private static bool VerifySurrounding(Board board, Coordinate coordinate)
        {
            Coordinate left = new Coordinate(coordinate.X - 1, coordinate.Y);
            Coordinate up = new Coordinate(coordinate.X, coordinate.Y + 1);
            Coordinate right = new Coordinate(coordinate.X + 1, coordinate.Y);
            Coordinate down = new Coordinate(coordinate.X, coordinate.Y - 1);

            Coordinate[] surroundings = { left, up, right, down };

            foreach (Coordinate surroundingCoordinate in surroundings)
            {
                if (surroundingCoordinate.X < board.Width && surroundingCoordinate.Y < board.Height)
                    if (VerifyShipPlacement(board, surroundingCoordinate) == false)
                        return false;
            }
            return true;
        }

        private static void ResetBoard(Board board)
        {
            for (int x = 0; x < board.Width; x++)
                for (int y = 0; y < board.Height; y++)
                    board.GetField(new Coordinate(x, y)).IsShip = false;
        }
    }
}
