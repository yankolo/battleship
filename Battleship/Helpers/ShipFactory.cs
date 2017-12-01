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
            List<Ship> ships = CreateShips(numberOfSize5, numberOfSize4, numberOfSize3, numberOfSize2);

            foreach (Ship ship in ships)
                PlaceShip(board, ship);

        }

        public static List<Ship> CreateShips(int numberOfSize5, int numberOfSize4, int numberOfSize3, int numberOfSize2)
        {
            List<Ship> ships = new List<Ship>();
            for (int i = 0; i < numberOfSize5; i++)
                ships.Add(new Ship(5));
            for (int i = 0; i < numberOfSize4; i++)
                ships.Add(new Ship(4));
            for (int i = 0; i < numberOfSize3; i++)
                ships.Add(new Ship(3));
            for (int i = 0; i < numberOfSize2; i++)
                ships.Add(new Ship(2));

            return ships;
        }

        private static void PlaceShip(Board board, Ship ship)
        {
            Random rand = new Random();

            List<Coordinate> horizontalCoordinates = FindPossiblePlacements(board, ship, Direction.Horizontal);
            List<Coordinate> verticalCoordinates = FindPossiblePlacements(board, ship, Direction.Vertical);

            Direction direction = Direction.Horizontal; // Set to Horizontal by default

            if (horizontalCoordinates.Count == 0 && verticalCoordinates.Count == 0)
                throw new NoShipSpaceException();
            else if (horizontalCoordinates.Count == 0)
                direction = Direction.Vertical;
            else if (verticalCoordinates.Count == 0)
                direction = Direction.Horizontal;
            else
                if (rand.Next(0, 2) == 0)
                    direction = Direction.Horizontal;
                else
                    direction = Direction.Vertical;

            Coordinate coordinate;
            if (direction.Equals(Direction.Horizontal))
            {
                coordinate = horizontalCoordinates[rand.Next(horizontalCoordinates.Count)];
                for (int i = 0; i < ship.Size; i++)
                    board.GetField(new Coordinate(coordinate.X + i, coordinate.Y)).Ship = ship;
            }
            else
            {
                coordinate = verticalCoordinates[rand.Next(verticalCoordinates.Count)];
                for (int i = 0; i < ship.Size; i++)
                    board.GetField(new Coordinate(coordinate.X, coordinate.Y + i)).Ship = ship;
            }
        }

        private static bool VerifyShipPlacement(Board board, Coordinate coordinate, Ship ship, Direction direction)
        {
            if (direction.Equals(Direction.Horizontal))
            {
                for (int i = 0; i < ship.Size; i++)
                    if (VerifyField(board, new Coordinate(coordinate.X + i, coordinate.Y)) == false)
                        return false;
            }
            else if (direction.Equals(Direction.Vertical))
            {
                for (int i = 0; i < ship.Size; i++)
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
                    if (board.GetField(surroundingCoordinate).Ship != null)
                        return false;
            }

            if (center.X < board.Width && center.Y < board.Height && center.X >= 0 && center.Y >= 0)
            {
                if (board.GetField(coordinate).Ship != null)
                    return false;
            } 
            else
                return false;

            return true;
        }


        private static List<Coordinate> FindPossiblePlacements(Board board, Ship ship, Direction direction)
        {
            List<Coordinate> coordinateList = new List<Coordinate>();

            if (direction.Equals(Direction.Horizontal))
            {
                for (int y = 0; y < board.Height; y++)
                    for (int x = 0; x <= board.Width - ship.Size; x++)
                    {
                        Coordinate coordinate = new Coordinate(x, y);
                        if (VerifyShipPlacement(board, coordinate, ship, direction) == true)
                            coordinateList.Add(coordinate);
                    }
            }
            else if (direction.Equals(Direction.Vertical))
            {
                for (int y = 0; y <= board.Height - ship.Size; y++)
                    for (int x = 0; x < board.Width; x++)
                    {
                        Coordinate coordinate = new Coordinate(x, y);
                        if (VerifyShipPlacement(board, coordinate, ship, direction) == true)
                            coordinateList.Add(coordinate);
                    }
            }
            else
                throw new Exception("Invalid Direction: " + direction);

            return coordinateList;
        }
    }
}
