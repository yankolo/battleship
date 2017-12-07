using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Battleship
{
   public class Game
    {
        private Board _userBoard;
        private Board _radarBoard;
        private int _userShots = 0;
        private int _userHits = 0;
        private int _cpuShots = 0;
        private int _cpuHits = 0;
        private String _displayedUserText;
        private String _displayedCPUText;
        private IArtificialIntelligence _AI;

        private Turn _currentTurn;

        private bool? _isGameWon;
        private bool _isGamePaused;

        private DispatcherTimer _delayBeforeAIShoot;
        private DispatcherTimer _timeForUserTurn;

        private TimeSpan _timeGivenForTurn;
        private TimeSpan _currentTurnTime;


        // Delegate and Event for the event handler GameUpdated
        public delegate void GameUpdatedEventHandler(object sender, UpdatedFieldEventArgs args);
        public event GameUpdatedEventHandler GameUpdated;

        public Game(Difficulty difficulty)
        {
            _userBoard = new Board(10, 10, true);
            _radarBoard = new Board(10, 10, false);
            ShipFactory.FillBoardRandomly(_radarBoard, 1, 2, 3, 4);
            ShipFactory.FillBoardRandomly(_userBoard, 1, 2, 3, 4);

            _timeGivenForTurn = new TimeSpan(0, 0, 0, 10, 0);

            switch (difficulty)
            {
                case Difficulty.Easy:
                    _AI = new EasyAI(_userBoard);
                    break;
                case Difficulty.Medium:
                    _AI = new MediumAI(_userBoard);
                    break;
                case Difficulty.Hard:
                    _AI = new HardAI(_userBoard);
                    break;
            }

            _currentTurn = Turn.Player;

            _timeForUserTurn = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 500), DispatcherPriority.Normal, TimeForUserTurn_Tick, Dispatcher.CurrentDispatcher);
            _currentTurnTime = new TimeSpan();
            _timeForUserTurn.Start();
        }

        public void PauseGame()
        {
            _isGamePaused = true;
            if (_delayBeforeAIShoot != null)
                _delayBeforeAIShoot.Stop();
            if (_timeForUserTurn != null)
                _timeForUserTurn.Stop();
        }

        public void UnPauseGame()
        {
            _isGamePaused = false;
            if (_delayBeforeAIShoot != null)
                _delayBeforeAIShoot.Start();
            if (_timeForUserTurn != null)
                _timeForUserTurn.Stop();
        }

        /// <summary>
        /// Method to trigger the event GameUpdated
        /// </summary>
        protected virtual void OnGameUpdated(Field newField, Field oldField, Board board)
        {
            if (GameUpdated != null)
            {
                UpdatedFieldEventArgs e = new UpdatedFieldEventArgs();
                e.newField = newField;
                e.oldField = oldField;
                e.board = board;
                GameUpdated(this, e);
            }
        }

        public void UpdateGame(Coordinate input)
        {
            if (_currentTurn.Equals(Turn.Player) && _isGamePaused == false)
            {
                Field fieldToShoot = RadarBoard.GetField(input);
                Field fieldToShootCopy = new Field(fieldToShoot); // Copy of field to compare changes of the field

                ShootOpponent(input);

                if (fieldToShoot.IsRevealed != fieldToShootCopy.IsRevealed)
                {
                    _currentTurn = Turn.Computer;
                    _delayBeforeAIShoot = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 500), DispatcherPriority.Normal, DelayBeforeAIShoot_Tick, Dispatcher.CurrentDispatcher);
                    _delayBeforeAIShoot.Start();
                }
            }
        }

        private void DelayBeforeAIShoot_Tick(object sender, EventArgs e)
        {
            _delayBeforeAIShoot.Stop();
            _timeForUserTurn.Stop();
            _timeForUserTurn = null;

            ShootUser();
            _currentTurn = Turn.Player;

            _delayBeforeAIShoot = null;

            _timeForUserTurn = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 500), DispatcherPriority.Normal, TimeForUserTurn_Tick, Dispatcher.CurrentDispatcher);
            _currentTurnTime = new TimeSpan();
            _timeForUserTurn.Start();
        }

        private void TimeForUserTurn_Tick(object sender, EventArgs e)
        {
            _currentTurnTime += new TimeSpan(0, 0, 0, 0, 500);

            if (_currentTurnTime > _timeGivenForTurn)
            {
                _timeForUserTurn.Stop();

                _currentTurn = Turn.Computer;
                ShootUser();
                _currentTurn = Turn.Player;

                _timeForUserTurn = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 500), DispatcherPriority.Normal, TimeForUserTurn_Tick, Dispatcher.CurrentDispatcher);
                _currentTurnTime = new TimeSpan();
                _timeForUserTurn.Start();
            }
        }

        private void ShootOpponent(Coordinate hitCoordinate)
        {
            if (_isGameWon != null)
                return;

            Field field = _radarBoard.GetField(hitCoordinate);
            Field copyField = new Field(field); // Copy of field to be able to see how the field changed

            if (field.Ship != null && !field.IsRevealed)
            {
                field.IsRevealed = true;
                _userShots++;
                _userHits++;
                field.Ship.Size--;
                if (field.Ship.Size ==0)
                {
                    _displayedUserText = "That's a hit! This " + field.Ship.Name + " has been sunked.";
                }
                else
                {
                   _displayedUserText = "That's a hit!";

                }
                OnGameUpdated(field, copyField, _radarBoard);
            }
            else if (field.Ship == null && !field.IsRevealed)
            {
                field.IsRevealed = true;
                _userShots++;
                _displayedUserText = "That was a shot in the water!";
                OnGameUpdated(field, copyField, _radarBoard);
            }
            else if (field.IsRevealed)
            {
                _displayedUserText = "You already shot there.";
                OnGameUpdated(field, copyField, _radarBoard);
            }

            if (_userHits >= 30)
            {
                _isGameWon = true;
                _displayedUserText = "Congratulations you sunk every ship";
                OnGameUpdated(field, copyField, _radarBoard);

                // add text output and end game --> ask user to restart or end game.
            }
        }
        private void ShootUser()
        {
            if (_isGameWon != null)
                return;

            Field field = _AI.Hit();
            Field copyField = new Field(field); // Copy of field to be able to see how the field changed

            if (field.Ship != null )
            {
                _cpuShots++;
                _cpuHits++;
                field.Ship.Size--;
                if (field.Ship.Size == 0)
                {
					field.IsHit = true;
                    _displayedCPUText = "You have been hit!" + field.Ship.Name + " has been sunked.";
                }
                else
                {

					field.IsHit = true;
				   _displayedCPUText = "You have been hit!";

                }
                OnGameUpdated(field, copyField, _userBoard);
            }
            else if (field.Ship == null )
            {
				//should add X over the water/field where the cpu made a hit. 
                _cpuShots++;
				field.IsHit = true;
				_displayedCPUText = "You have not been hit!";
                OnGameUpdated(field, copyField, _userBoard);
            }
            if (_cpuHits >= 30)
            {
                _isGameWon = false;
                _displayedCPUText = "Sorry the cpu destroyed every ship you have :(";
                OnGameUpdated(field, copyField, _userBoard);

                // add text output and end game --> ask user to restart or end game.
            }
        }


        public Board RadarBoard { get { return _radarBoard; } }
        public Board UserBoard { get { return _userBoard; } }
        public int UserShots { get { return _userShots; } }
        public int UserHits { get { return _userHits; } }
        public int CpuShots { get { return _cpuShots; } }
        public int CpuHits { get { return _cpuHits; } }
        public bool? IsGameWOn { get { return _isGameWon; } }
        public bool IsGamePaused { get { return _isGamePaused; } }
        public String DisplayedUserText { get { return _displayedUserText; } }
        public String DisplayedCPUText { get { return _displayedCPUText; } }
    }
}
