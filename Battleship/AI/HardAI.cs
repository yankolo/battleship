using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    [Serializable]
    public class HardAI : IArtificialIntelligence
    {
        private Board _userBoard;
        private List<Field> _fieldList;

        private List<Field> _notHitFieldsList;
        private List<Field> _hitShipList;
        private List<Field> _hitWaterList;
        private List<Ship> _destroyedShipsList;

        private List<Field> _mayHaveShipsAnySize;
        private List<Field> _mayHaveShipsSize2;
        private List<Field> _mayHaveShipsSize3;
        private List<Field> _mayHaveShipsSize4;
        private List<Field> _mayHaveShipsSize5;

        private List<Field> _mayHaveRestOfShip;


        public HardAI(Board userBoard)
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
            // Creating lists of fields that contain fields that were not hit
            // and fields that were hit and contain ships or water
            _notHitFieldsList = new List<Field>();
            _hitShipList = new List<Field>();
            _hitWaterList = new List<Field>();
            foreach (Field field in _fieldList)
            {
                if (field.IsHit == false)
                    _notHitFieldsList.Add(field);

                if (field.IsHit == true && field.Ship != null)
                    _hitShipList.Add(field);
                else if (field.IsHit == true && field.Ship == null)
                    _hitWaterList.Add(field);
            }

            // Creating list that contains of ships that were destroyed
            _destroyedShipsList = new List<Ship>();
            foreach (Field field in _hitShipList)
            {
                Ship shipInField = field.Ship;

                if (shipInField.Size == 0 && _destroyedShipsList.Contains(shipInField) == false)
                    _destroyedShipsList.Add(shipInField);
            }

            // Creating list that contains the fields that may contain the rest of any ship
            _mayHaveRestOfShip = new List<Field>();
            foreach (Field field in _hitShipList)
            {
                if (field.Ship.Size != 0)
                {
                    List<Field> shipFields = new List<Field>();
                    shipFields.Add(field);

                    for (int i = 0; i < shipFields.Count; i++)
                    {
                        List<Field> surroundingFields = GetSurroundingFields(shipFields[i]);

                        foreach (Field surrounding in surroundingFields)
                            if (surrounding.IsHit == true && surrounding.Ship != null && shipFields.Contains(surrounding) == false)
                                shipFields.Add(surrounding);
                    }

                    if (shipFields.Count > 1)
                    {
                        Direction direction;
                        Coordinate coordinateField1 = _userBoard.GetCoordinates(shipFields[0]);
                        Coordinate coordinateField2 = _userBoard.GetCoordinates(shipFields[1]);

                        if (coordinateField1.X - coordinateField2.X == 0)
                        {
                            direction = Direction.Vertical;
                        }
                        else
                            direction = Direction.Horizontal;

                        Field beginningOfShip = field; // By default
                        if (direction.Equals(Direction.Horizontal))
                        {
                            int minX = _userBoard.GetCoordinates(shipFields[0]).X;
                            int y = _userBoard.GetCoordinates(shipFields[0]).Y;
                            for (int i = 0; i < shipFields.Count; i++)
                            {
                                int x = _userBoard.GetCoordinates(shipFields[i]).X;
                                if (x < minX)
                                    minX = x;
                            }
                            beginningOfShip = _userBoard.GetField(new Coordinate(minX, y));
                        }
                        else if (direction.Equals(Direction.Vertical))
                        {
                            int x = _userBoard.GetCoordinates(shipFields[0]).X;
                            int minY = _userBoard.GetCoordinates(shipFields[0]).Y;
                            for (int i = 0; i < shipFields.Count; i++)
                            {
                                int y = _userBoard.GetCoordinates(shipFields[i]).Y;
                                if (y < minY)
                                    minY = y;
                            }
                            beginningOfShip = _userBoard.GetField(new Coordinate(x, minY));
                        }

                        Field endOfShip = field; // By default
                        if (direction.Equals(Direction.Horizontal))
                        {
                            int maxX = _userBoard.GetCoordinates(shipFields[0]).X;
                            int y = _userBoard.GetCoordinates(shipFields[0]).Y;
                            for (int i = 0; i < shipFields.Count; i++)
                            {
                                int x = _userBoard.GetCoordinates(shipFields[i]).X;
                                if (x > maxX)
                                    maxX = x;
                            }
                            endOfShip = _userBoard.GetField(new Coordinate(maxX, y));
                        }
                        else if (direction.Equals(Direction.Vertical))
                        {
                            int x = _userBoard.GetCoordinates(shipFields[0]).X;
                            int maxY = _userBoard.GetCoordinates(shipFields[0]).Y;
                            for (int i = 0; i < shipFields.Count; i++)
                            {
                                int y = _userBoard.GetCoordinates(shipFields[i]).Y;
                                if (y > maxY)
                                    maxY = y;
                            }
                            endOfShip = _userBoard.GetField(new Coordinate(x, maxY));
                        }

                        if (direction.Equals(Direction.Horizontal))
                        {
                            Field leftField = GetLeftField(beginningOfShip);
                            if (_hitShipList.Contains(GetLeftField(leftField)) == false && _hitShipList.Contains(GetDownField(leftField)) == false && _hitShipList.Contains(GetUpField(leftField)) == false)
                                if (_notHitFieldsList.Contains(leftField) == true)
                                    if (_mayHaveRestOfShip.Contains(leftField) == false)
                                        _mayHaveRestOfShip.Add(leftField);

                            Field rightField = GetRightField(endOfShip);
                            if (_hitShipList.Contains(GetRightField(rightField)) == false && _hitShipList.Contains(GetDownField(rightField)) == false && _hitShipList.Contains(GetUpField(rightField)) == false)
                                if (_notHitFieldsList.Contains(rightField) == true)
                                    if (_mayHaveRestOfShip.Contains(rightField) == false)
                                        _mayHaveRestOfShip.Add(rightField);
                        }
                        else if (direction.Equals(Direction.Vertical))
                        {
                            Field upField = GetDownField(beginningOfShip);
                            if (_hitShipList.Contains(GetDownField(upField)) == false && _hitShipList.Contains(GetLeftField(upField)) == false && _hitShipList.Contains(GetRightField(upField)) == false)
                                if (_notHitFieldsList.Contains(upField) == true)
                                    if (_mayHaveRestOfShip.Contains(upField) == false)
                                        _mayHaveRestOfShip.Add(upField);

                            Field downField = GetUpField(endOfShip);
                            if (_hitShipList.Contains(GetUpField(downField)) == false && _hitShipList.Contains(GetLeftField(downField)) == false && _hitShipList.Contains(GetRightField(downField)) == false)
                                if (_notHitFieldsList.Contains(downField) == true)
                                    if (_mayHaveRestOfShip.Contains(downField) == false)
                                        _mayHaveRestOfShip.Add(downField);
                        }
                    }
                    else // if ship size is 1
                    {
                        Field leftField = GetLeftField(field);
                        if (_hitShipList.Contains(GetLeftField(leftField)) == false && _hitShipList.Contains(GetDownField(leftField)) == false && _hitShipList.Contains(GetUpField(leftField)) == false)
                            if (_notHitFieldsList.Contains(leftField) == true)
                                if (_mayHaveRestOfShip.Contains(leftField) == false)
                                    _mayHaveRestOfShip.Add(leftField);

                        Field rightField = GetRightField(field);
                        if (_hitShipList.Contains(GetRightField(rightField)) == false && _hitShipList.Contains(GetDownField(rightField)) == false && _hitShipList.Contains(GetUpField(rightField)) == false)
                            if (_notHitFieldsList.Contains(rightField) == true)
                                if (_mayHaveRestOfShip.Contains(rightField) == false)
                                    _mayHaveRestOfShip.Add(rightField);

                        Field upField = GetUpField(field);
                        if (_hitShipList.Contains(GetUpField(upField)) == false && _hitShipList.Contains(GetLeftField(upField)) == false && _hitShipList.Contains(GetRightField(upField)) == false)
                            if (_notHitFieldsList.Contains(upField) == true)
                                if (_mayHaveRestOfShip.Contains(upField) == false)
                                    _mayHaveRestOfShip.Add(upField);

                        Field downField = GetDownField(field);
                        if (_hitShipList.Contains(GetDownField(downField)) == false && _hitShipList.Contains(GetLeftField(downField)) == false && _hitShipList.Contains(GetRightField(downField)) == false)
                            if (_notHitFieldsList.Contains(downField) == true)
                                if (_mayHaveRestOfShip.Contains(downField) == false)
                                    _mayHaveRestOfShip.Add(downField);
                    }

                }
            }

            // Creating list of fields that may contain fields with new ships
            _mayHaveShipsAnySize = new List<Field>();

            foreach (Field field in _notHitFieldsList)
            {
                List<Field> surroundingFields = GetSurroundingFields(field);

                bool isShipAround = false;

                foreach (Field surrounding in surroundingFields)
                    if (_hitShipList.Contains(surrounding))
                        isShipAround = true;

                if (isShipAround == false)
                    _mayHaveShipsAnySize.Add(field);
            }

            // Creating list of fields that may contain fields with ships of size 2, 3, 4, 5
            _mayHaveShipsSize2 = GetMayHaveShips(2);
            _mayHaveShipsSize3 = GetMayHaveShips(3);
            _mayHaveShipsSize4 = GetMayHaveShips(4);
            _mayHaveShipsSize5 = GetMayHaveShips(5);

            Random rand = new Random();

            if (_mayHaveRestOfShip.Count > 0)
            {
                Field field = _mayHaveRestOfShip[rand.Next(0, _mayHaveRestOfShip.Count)];
                return field;
            }
            else if (_mayHaveShipsSize5.Count > 0)
            {
                Field field = _mayHaveShipsSize5[rand.Next(0, _mayHaveShipsSize5.Count)];
                return field;
            }
            else if (_mayHaveShipsSize4.Count > 0)
            {
                Field field = _mayHaveShipsSize4[rand.Next(0, _mayHaveShipsSize4.Count)];
                return field;
            }
            else if (_mayHaveShipsSize3.Count > 0)
            {
                Field field = _mayHaveShipsSize3[rand.Next(0, _mayHaveShipsSize3.Count)];
                return field;
            }
            else if (_mayHaveShipsSize2.Count > 0)
            {
                Field field = _mayHaveShipsSize2[rand.Next(0, _mayHaveShipsSize2.Count)];
                return field;
            }
            else
            {
                Field field = _mayHaveShipsAnySize[rand.Next(0, _mayHaveShipsAnySize.Count)];
                return field;
            }
        }

        private List<Field> GetSurroundingFields(Field field)
        {
            Coordinate fieldCoordinate = _userBoard.GetCoordinates(field);

            Coordinate left = new Coordinate(fieldCoordinate.X - 1, fieldCoordinate.Y);
            Coordinate up = new Coordinate(fieldCoordinate.X, fieldCoordinate.Y + 1);
            Coordinate right = new Coordinate(fieldCoordinate.X + 1, fieldCoordinate.Y);
            Coordinate down = new Coordinate(fieldCoordinate.X, fieldCoordinate.Y - 1);

            Coordinate[] surroundings = { left, up, right, down };

            List<Field> surroundingFields = new List<Field>();

            foreach (Coordinate surroundingCoordinate in surroundings)
            {
                if (surroundingCoordinate.X < _userBoard.Width && surroundingCoordinate.Y < _userBoard.Height && surroundingCoordinate.X >= 0 && surroundingCoordinate.Y >= 0)
                    surroundingFields.Add(_userBoard.GetField(surroundingCoordinate));
            }

            return surroundingFields;
        }

        private Field GetLeftField(Field field)
        {
            if (field != null)
            {
                Coordinate coordinate = _userBoard.GetCoordinates(field);
                coordinate = new Coordinate(coordinate.X - 1, coordinate.Y);

                if (coordinate.X >= 0)
                    return _userBoard.GetField(coordinate);
            }

            return null;
        }

        private Field GetUpField(Field field)
        {
            if (field != null)
            {
                Coordinate coordinate = _userBoard.GetCoordinates(field);
                coordinate = new Coordinate(coordinate.X, coordinate.Y + 1);

                if (coordinate.Y < _userBoard.Height)
                    return _userBoard.GetField(coordinate);
            }

            return null;
        }

        private Field GetRightField(Field field)
        {
            if (field != null)
            {
                Coordinate coordinate = _userBoard.GetCoordinates(field);
                coordinate = new Coordinate(coordinate.X + 1, coordinate.Y);

                if (coordinate.X < _userBoard.Width)
                    return _userBoard.GetField(coordinate);
            }

            return null;
        }

        private Field GetDownField(Field field)
        {
            if (field != null)
            {
                Coordinate coordinate = _userBoard.GetCoordinates(field);
                coordinate = new Coordinate(coordinate.X, coordinate.Y - 1);

                if (coordinate.Y >= 0)
                    return _userBoard.GetField(coordinate);
            }

            return null;
        }

        private List<Field> GetMayHaveShips(int size)
        {
            List<Field> mayHaveShips = new List<Field>();

            int numOfDestroyedShips = 0;

            foreach (Ship ship in _destroyedShipsList)
            {
                if (ship.ActualSize == size)
                    numOfDestroyedShips++;
            }

            bool isShipsLeft = true;

            switch (size)
            {
                case 2:
                    if (numOfDestroyedShips == 4)
                        isShipsLeft = false;
                    break;
                case 3:
                    if (numOfDestroyedShips == 3)
                        isShipsLeft = false;
                    break;
                case 4:
                    if (numOfDestroyedShips == 2)
                        isShipsLeft = false;
                    break;
                case 5:
                    if (numOfDestroyedShips == 1)
                        isShipsLeft = false;
                    break;
            }
            if (isShipsLeft)
            {
                foreach (Field field in _mayHaveShipsAnySize)
                {
                    List<Field> largestPossibleHorizontalShip = GetLargestPossibleShipFields(field, Direction.Horizontal);
                    List<Field> largestPossibleVerticalShip = GetLargestPossibleShipFields(field, Direction.Vertical);

                    if (largestPossibleHorizontalShip.Count >= size || largestPossibleVerticalShip.Count >= size)
                        mayHaveShips.Add(field);
                }
            }

            return mayHaveShips;
        }

        private List<Field> GetLargestPossibleShipFields(Field field, Direction direction)
        {
            List<Field> largestPossibleShipFields = new List<Field>();
            largestPossibleShipFields.Add(field);

            if (direction.Equals(Direction.Horizontal))
            {
                Coordinate fieldCoordinate = _userBoard.GetCoordinates(field);

                Coordinate coordinate = new Coordinate(fieldCoordinate.X - 1, fieldCoordinate.Y);
                while (coordinate.X >= 0 && _mayHaveShipsAnySize.Contains(_userBoard.GetField(coordinate)))
                {
                    largestPossibleShipFields.Add(_userBoard.GetField(coordinate));
                    coordinate.X--;
                }

                coordinate = new Coordinate(fieldCoordinate.X + 1, fieldCoordinate.Y);
                while (coordinate.X < _userBoard.Width && _mayHaveShipsAnySize.Contains(_userBoard.GetField(coordinate)))
                {
                    largestPossibleShipFields.Add(_userBoard.GetField(coordinate));
                    coordinate.X++;
                }
            }
            else if (direction.Equals(Direction.Vertical))
            {
                Coordinate fieldCoordinate = _userBoard.GetCoordinates(field);

                Coordinate coordinate = new Coordinate(fieldCoordinate.X, fieldCoordinate.Y - 1);
                while (coordinate.Y >= 0 && _mayHaveShipsAnySize.Contains(_userBoard.GetField(coordinate)))
                {
                    largestPossibleShipFields.Add(_userBoard.GetField(coordinate));
                    coordinate.Y--;
                }

                coordinate = new Coordinate(fieldCoordinate.X, fieldCoordinate.Y + 1);
                while (coordinate.Y < _userBoard.Height && _mayHaveShipsAnySize.Contains(_userBoard.GetField(coordinate)))
                {
                    largestPossibleShipFields.Add(_userBoard.GetField(coordinate));
                    coordinate.Y++;
                }
            }

            return largestPossibleShipFields;
        }

		public Difficulty GetDifficutly()
		{
			return Difficulty.Hard;
		}
	}
}

