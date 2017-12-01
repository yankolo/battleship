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
            for (int i = 0; i < numberOfSize5; i++)
                GenerateShip(board, 5);
            for (int i = 0; i < numberOfSize4; i++)
                GenerateShip(board, 4);
            for (int i = 0; i < numberOfSize3; i++)
                GenerateShip(board, 3);
            for (int i = 0; i < numberOfSize2; i++)
                GenerateShip(board, 2);
        }

        private static void GenerateShip(Board board, int size)
        {
            Random rand = new Random();

            List<Coordinate> horizontalCoordinates = FindPossiblePlacements(board, size, Direction.Horizontal);
            List<Coordinate> verticalCoordinates = FindPossiblePlacements(board, size, Direction.Vertical);

            Direction direction = Direction.Horizontal; // Set to Horizontal by default

            if (horizontalCoordinates.Count() == 0 && verticalCoordinates.Count() == 0)
                throw new NoShipSpaceException();
            else if (horizontalCoordinates.Count == 0)
                direction = Direction.Vertical;
            else if (verticalCoordinates.Count() == 0)
                direction = Direction.Horizontal;
            else
                if (rand.Next(0, 2) == 0)
                    direction = Direction.Horizontal;
                else
                    direction = Direction.Vertical;

            Coordinate coordinate;
            if (direction.Equals(Direction.Horizontal))
            {
                coordinate = horizontalCoordinates[rand.Next(horizontalCoordinates.Count())];
                for (int i = 0; i < size; i++)
                    board.GetField(new Coordinate(coordinate.X + i, coordinate.Y)).IsShip = true;
            }
            else
            {
                coordinate = verticalCoordinates[rand.Next(verticalCoordinates.Count())];
                for (int i = 0; i < size; i++)
                    board.GetField(new Coordinate(coordinate.X, coordinate.Y + i)).IsShip = true;
            }
        }

        private static bool VerifyShipPlacement(Board board, Coordinate coordinate, int size, Direction direction)
        {
            if (direction.Equals(Direction.Horizontal))
            {
                for (int i = 0; i < size; i++)
                    if (VerifyField(board, new Coordinate(coordinate.X + i, coordinate.Y)) == false)
                        return false;
            }
            else if (direction.Equals(Direction.Vertical))
            {
                for (int i = 0; i < size; i++)
                    if (VerifyField(board, new Coordinate(coordinate.X, coordinate.Y + i)) == false)
                        return false;
            }
            else
                throw new Exception("Invalid Direction: " + direction);

            return true;
        }

        private static bool VerifyField(Board board, Coordinate coordinate)
        {
            Coordinate center = new Coordinate(coordinate.X, coordinate.Y);
            Coordinate left = new Coordinate(coordinate.X - 1, coordinate.Y);
            Coordinate up = new Coordinate(coordinate.X, coordinate.Y + 1);
            Coordinate right = new Coordinate(coordinate.X + 1, coordinate.Y);
            Coordinate down = new Coordinate(coordinate.X, coordinate.Y - 1);

            Coordinate[] surroundings = { left, up, right, down };

            foreach (Coordinate surroundingCoordinate in surroundings)
            {
                if (surroundingCoordinate.X < board.Width && surroundingCoordinate.Y < board.Height && surroundingCoordinate.X >= 0 && surroundingCoordinate.Y >= 0)
                    if (board.GetField(surroundingCoordinate).IsShip)
                        return false;
            }

            if (center.X < board.Width && center.Y < board.Height && center.X >= 0 && center.Y >= 0)
            {
                if (board.GetField(coordinate).IsShip)
                    return false;
            } 
            else
                return false;

            return true;
        }


        private static List<Coordinate> FindPossiblePlacements(Board board, int size, Direction direction)
        {
            List<Coordinate> coordinateList = new List<Coordinate>();

            if (direction.Equals(Direction.Horizontal))
            {
                for (int y = 0; y < board.Height; y++)
                    for (int x = 0; x <= board.Width - size; x++)
                    {
                        Coordinate coordinate = new Coordinate(x, y);
                        if (VerifyShipPlacement(board, coordinate, size, direction) == true)
                            coordinateList.Add(coordinate);
                    }
            }
            else if (direction.Equals(Direction.Vertical))
            {
                for (int y = 0; y <= board.Height - size; y++)
                    for (int x = 0; x < board.Width; x++)
                    {
                        Coordinate coordinate = new Coordinate(x, y);
                        if (VerifyShipPlacement(board, coordinate, size, direction) == true)
                            coordinateList.Add(coordinate);
                    }
            }
            else
                throw new Exception("Invalid Direction: " + direction);

            return coordinateList;
        }
    }
}
