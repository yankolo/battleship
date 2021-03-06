﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Battleship
{
    [Serializable]
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
        private String _userName;

        private Turn _currentTurn;

        private bool? _isGameWon;
        [NonSerialized]
        private bool _isGamePaused;

        [NonSerialized]
        private DispatcherTimer _delayBeforeAIShoot;
        [NonSerialized]
        private DispatcherTimer _timeForUserTurn;

        private TimeSpan _timeGivenForTurn;
        private TimeSpan _currentTurnTime;
        private bool _debug = false;
        private bool _timeForTurnActive;

        [NonSerialized]
        private bool _isCurrentlyDebug;

        // Delegate and Event for the event GameUpdated
        public delegate void GameUpdatedEventHandler(object sender, UpdatedFieldEventArgs args);
        public event GameUpdatedEventHandler GameUpdated;

        // Delegate and Evente for the event GameFinished
        public delegate void GameFinishedEventHandler(object sender, EventArgs e);
        public event GameFinishedEventHandler GameFinished;

        public Game(Difficulty difficulty, String userName , bool debug, TimeSpan timeGivenForTurn)
        {
            _debug = debug;
            _userBoard = new Board(10, 10, true);
            _radarBoard = new Board(10, 10, false);
            ShipFactory.FillBoardRandomly(_radarBoard, 1, 2, 3, 4);
            ShipFactory.FillBoardRandomly(_userBoard, 1, 2, 3, 4);
            _userName = userName.ToLower();

            if (timeGivenForTurn.Seconds == 0 && timeGivenForTurn.Minutes == 0)
                _timeForTurnActive = false;
            else
                _timeForTurnActive = true;

            if (_timeForTurnActive == true)
                _timeGivenForTurn = timeGivenForTurn;

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

            if (_timeForTurnActive)
            {
                _timeForUserTurn = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 500), DispatcherPriority.Normal, TimeForUserTurn_Tick, Dispatcher.CurrentDispatcher);
                _currentTurnTime = new TimeSpan();
                _timeForUserTurn.Start();
            }
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
                _timeForUserTurn.Start();

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

        /// <summary>
        /// Method to trigger the event GameFinished
        /// </summary>
        protected virtual void OnGameFinished()
        {
            if (GameFinished != null)
            {
                EventArgs e = new EventArgs();
                GameFinished(this, e);
            }
        }

        public void UpdateGame(Coordinate input)
        {
			if (_currentTurn.Equals(Turn.Player) && _isGamePaused == false)
            {
                Field fieldToShoot = RadarBoard.GetField(input);
                Field fieldToShootCopy = new Field(fieldToShoot); // Copy of field to compare changes of the field

                ShootOpponent(input);

                if (fieldToShoot.IsHit != fieldToShootCopy.IsHit)
                {
                    _currentTurn = Turn.Computer;
                    _delayBeforeAIShoot = new DispatcherTimer(new TimeSpan(0, 0, 0, 3, 0), DispatcherPriority.Normal, DelayBeforeAIShoot_Tick, Dispatcher.CurrentDispatcher);
                    _delayBeforeAIShoot.Start();
                }
            }
        }

        private void DelayBeforeAIShoot_Tick(object sender, EventArgs e)
        {
            _delayBeforeAIShoot.Stop();
			if (_timeForUserTurn != null)
				_timeForUserTurn.Stop();
            _timeForUserTurn = null;

            ShootUser();
            _currentTurn = Turn.Player;

            _delayBeforeAIShoot = null;

            if (_timeForTurnActive)
            {
                _timeForUserTurn = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 500), DispatcherPriority.Normal, TimeForUserTurn_Tick, Dispatcher.CurrentDispatcher);
                _currentTurnTime = new TimeSpan();
                _timeForUserTurn.Start();
            }
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

            if (field.Ship != null && !field.IsHit)
            {
                field.IsRevealed = true;
                _userShots++;
                _userHits++;
                field.Ship.Size--;
                if (field.Ship.Size ==0)
                {
                    field.Ship.IsSunk = true;
                    _displayedUserText = "That's a hit! This " + field.Ship.Name + " has been sunked.";
                    field.IsHit = true;
                }
                else
                {
                   _displayedUserText = "That's a hit!";
                    field.IsHit = true;
                }
                OnGameUpdated(field, copyField, _radarBoard);
            }
            else if (field.Ship == null && !field.IsHit)
            {
                field.IsRevealed = true;
                _userShots++;
                _displayedUserText = "That was a shot in the water!";
                field.IsHit = true;
                OnGameUpdated(field, copyField, _radarBoard);
            }
            else if (field.IsHit)
            {
                _displayedUserText = "You already shot there.";
                OnGameUpdated(field, copyField, _radarBoard);
            }

            if (_userHits >= 30)
            {
                _isGameWon = true;
                _displayedUserText = "Congratulations you sunk every ship";
                OnGameUpdated(field, copyField, _radarBoard);
                OnGameFinished();

                SaveScore();

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
                    field.Ship.IsSunk = true;
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
                OnGameFinished();

                SaveScore();

				// add text output and end game --> ask user to restart or end game.
			}
        }


		public void ResumeTimers()
		{
			if (_currentTurn.Equals(Turn.Computer))
			{
				_delayBeforeAIShoot = new DispatcherTimer(new TimeSpan(0, 0, 0, 3, 0), DispatcherPriority.Normal, DelayBeforeAIShoot_Tick, Dispatcher.CurrentDispatcher);
				_delayBeforeAIShoot.Start();
			}
			else
			{
                if (_timeForTurnActive)
                {
                    _timeForUserTurn = new DispatcherTimer(new TimeSpan(0, 0, 0, 3 , 0), DispatcherPriority.Normal, TimeForUserTurn_Tick, Dispatcher.CurrentDispatcher);
                    _timeForUserTurn.Start();
                }
			}

		}

		public void SaveScore()
		{
			bool isPlayerScoreFound = false;
			Score score;

			if (Directory.Exists("Scores"))
			{
				string[] files = Directory.GetFiles("Scores", "*.score", SearchOption.TopDirectoryOnly);

				for (int i = 0; i < files.Length; i++)
				{
					string playerName = files[i].Substring(files[i].LastIndexOf("\\") + 1);
					playerName = playerName.Substring(0, playerName.Length - playerName.LastIndexOf("."));

					if (_userName.Equals(playerName))
					{
						isPlayerScoreFound = true;
						break;
					}
				}
			}
			else
				Directory.CreateDirectory("Scores");

			if (isPlayerScoreFound)
			{
				IFormatter formatter1 = new BinaryFormatter();
				Stream stream1 = new FileStream("Scores/" + _userName + ".score", FileMode.Open, FileAccess.Read, FileShare.Read);
				score = (Score)formatter1.Deserialize(stream1);
				stream1.Close();
			}
			else
			{
				score = new Score(_userName, 0, 0, 0, 0);
			}

			if (_isGameWon == true)
			{
				switch (_AI.GetDifficutly())
				{
					case Difficulty.Easy:
						score.EasyWins++;
						break;
					case Difficulty.Medium:
						score.MediumWins++;
						break;
					case Difficulty.Hard:
						score.HardWins++;
						break;
				}
			}
			else if (_isGameWon == false)
			{
				score.Losses++;
			}

			IFormatter formatter2 = new BinaryFormatter();
			Stream stream2 = new FileStream("Scores/" + _userName + ".score", FileMode.Create, FileAccess.Write, FileShare.None);
			formatter2.Serialize(stream2, score);
			stream2.Close();

		}
        public void RevealAll()
        {
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    Coordinate cd = new Coordinate(x, y);
                    Field field = _radarBoard.GetField(cd);
                    field.IsRevealed = true;

                }
            }
        }
        public void HideAll()
        {
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    Coordinate cd = new Coordinate(x, y);
                    Field field = _radarBoard.GetField(cd);
                    if (field.IsHit == false)
                    {
                        field.IsRevealed = false;
                    }


                }
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
        public bool Debug { get { return _debug; } }
        public bool IsCurrentlyDebug { get { return _isCurrentlyDebug; } set { _isCurrentlyDebug = value; } }
        public Turn CurrentTurn { get { return _currentTurn; } }
    }
}
