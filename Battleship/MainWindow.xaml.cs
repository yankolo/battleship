using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Timers;
using Battleship;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Battleship
{
    /// <summary>
    /// Simple wpf battleships game
    /// </summary>
    public partial class MainWindow : Window
    {
        public Game _game;

        /// <summary>
        /// Setup basics and create game window
        /// </summary>
        public MainWindow(Difficulty difficulty, String userName)
        {
            InitializeComponent();
            _game = new Game(difficulty, userName);
            _game.GameUpdated += OnGameUpdated;
            InitializeGridCells();
            UpdateAllGUI(true);
        }

        public MainWindow(Game game)
        {
            InitializeComponent();
            _game = game;
            _game.GameUpdated += OnGameUpdated;
            InitializeGridCells();
            UpdateAllGUI(true);
        }

        private void InitializeGridCells()
        {
            for (int x = 1; x < 11; x++)
            {
                for (int y = 1; y < 11; y++)
                {
                    ContentControl content = new ContentControl();
                    Grid.SetColumn(content, x);
                    Grid.SetRow(content, y);

                    userGrid.Children.Add(content);

                    content = new ContentControl();
                    content.MouseDown += new MouseButtonEventHandler(HandleFieldClick);
                    Grid.SetColumn(content, x);
                    Grid.SetRow(content, y);

                    computerGrid.Children.Add(content);
                }
            }
        }

        /// <summary>
        /// Event Handler for GameUpdated
        /// 
        /// Calls the methods to update the GUI (UpdateField() and UpdateAllGUI)
        /// 
        /// Before calling UpdateField, it finds the content control that represents the fields passed in the EventArgs
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGameUpdated(object sender, UpdatedFieldEventArgs e)
        {
            if (e.newField != null && e.oldField != null)
            {
                Coordinate coordinate; // The coordinate of the field
                Grid grid; // The grid that contains the content control representing the field
                ContentControl fieldContentControl; // the content control representing the field

                if (e.board == _game.RadarBoard)
                {
                    coordinate = _game.RadarBoard.GetCoordinates(e.newField);
                    grid = computerGrid;
                }
                else
                {
                    coordinate = _game.UserBoard.GetCoordinates(e.newField);
                    grid = userGrid;
                }

                // Finding the content control representing the field
                for (int i = 0; i < grid.Children.Count; i++)
                {
                    UIElement element = grid.Children[i];
                    if (element.GetType() == typeof(ContentControl))
                    {
                        ContentControl content = (ContentControl)element;

                        if (Grid.GetRow(content) == coordinate.Y + 1 && Grid.GetColumn(content) == coordinate.X + 1)
                        {
                            fieldContentControl = content;
                            UpdateField(fieldContentControl, e.newField, e.oldField);
                        }
                    }
                }
            }

            UpdateAllGUI(true); // SHOULD BE UpdateAllGUI(), IT IS  UpdateAllGUI(true) BECAUSE "X" ON HIT FIELDS ARE SET IN THIS METHOD
        }

        /// <summary>
        /// Handles a click on a button from the field and will change color of it depending on the result
        /// </summary>
        public void HandleFieldClick(object sender, RoutedEventArgs e)
        {
            if (_game.IsGamePaused == false)
            {
                ContentControl b = (ContentControl)sender;
                Coordinate hitCoordinate = new Coordinate(Grid.GetColumn(b) - 1, Grid.GetRow(b) - 1);

                _game.UpdateGame(hitCoordinate);
            }
        }

		public void UpdateField(ContentControl content, Field newField, Field oldField)
        {
            if (oldField.IsRevealed != newField.IsRevealed)
            {
                if (newField.Ship == null) // If water
                {
                    BitmapImage bitmap = new BitmapImage(new Uri("pack://application:,,,/Assets/blue.png"));
                    Image image = new Image();
                    image.Source = bitmap;
                    content.Content = image;

                    // Code to display animation and sound when ship is hit
                }
                else // If ship
                {
                    BitmapImage bitmap = new BitmapImage(new Uri("pack://application:,,,/Assets/red.png"));
                    Image image = new Image();
                    image.Source = bitmap;
                    content.Content = image;

                    // Code to display animation and sound when water is hit
                }
            }
            else
            {
                // Code to display sound (and animation if needed) when trying to hit
                // something that was already hit
            }

        }


        public void UpdateAllGUI(bool shouldUpdateFields = false)
        {
			userText.Text = _game.DisplayedUserText;
			cpuText.Text = _game.DisplayedCPUText;
			userScoreboard.Content = "shots:\r\n" + _game.UserShots;
			cpuScoreboard.Content = "shots:\r\n" + _game.CpuShots;

            if (shouldUpdateFields == true)
            {
                UpdateGUICompFields();
                UpdateGUIUserFields();
            }
        }

        private void UpdateGUICompFields()
        {
            for (int i = 0; i < computerGrid.Children.Count; i++)
            {
                UIElement e = computerGrid.Children[i];
                if (e.GetType() == typeof(ContentControl))
                {
                    ContentControl content = (ContentControl)e;
                    Coordinate coordinate = new Coordinate(Grid.GetColumn(content) - 1, Grid.GetRow(content) - 1);
                    Field field = _game.RadarBoard.GetField(coordinate);

                    if (field.IsRevealed == false)
                    {
                        BitmapImage bitmap = new BitmapImage(new Uri("pack://application:,,,/Assets/green.png"));
                        Image image = new Image();
                        image.Source = bitmap;
                        content.Content = image;
                    }
                    else if (field.IsRevealed == true)
                    {
                        if (field.Ship == null)
                        {
                            BitmapImage bitmap = new BitmapImage(new Uri("pack://application:,,,/Assets/blue.png"));
                            Image image = new Image();
                            image.Source = bitmap;
                            content.Content = image;
                        }
                        else
                        {
                            BitmapImage bitmap = new BitmapImage(new Uri("pack://application:,,,/Assets/red.png"));
                            Image image = new Image();
                            image.Source = bitmap;
                            content.Content = image;
                        }
                    }
                }
            }
        }

        private void UpdateGUIUserFields()
        {
            for (int i = 0; i < userGrid.Children.Count; i++)
            {
                UIElement e = userGrid.Children[i];
                if (e.GetType() == typeof(ContentControl))
                {
                    ContentControl content = (ContentControl)e;
                    Coordinate coordinate = new Coordinate(Grid.GetColumn(content) - 1, Grid.GetRow(content) - 1);
                    Field field = _game.UserBoard.GetField(coordinate);
                    if (field.IsRevealed == false)
                    {
                        BitmapImage bitmap = new BitmapImage(new Uri("pack://application:,,,/Assets/green.png"));
                        Image image = new Image();
                        image.Source = bitmap;
                        content.Content = image;
                    }
                    else if (field.IsRevealed == true)
                    {
                        if (field.Ship == null)
                        {
							if (field.IsHit == false)
							{
								BitmapImage bitmap = new BitmapImage(new Uri("pack://application:,,,/Assets/blue.png"));
								Image image = new Image();
								image.Source = bitmap;
								content.Content = image;
							}
							else
							{
								BitmapImage bitmap = new BitmapImage(new Uri("pack://application:,,,/Assets/Ic_close_48px.svg.png"));
								Image image = new Image();
								image.Source = bitmap;
								content.Content = image;
							}
                           
                        }
                        else
                        {
							if (field.IsHit == false)
							{
								BitmapImage bitmap = new BitmapImage(new Uri("pack://application:,,,/Assets/red.png"));
								Image image = new Image();
								image.Source = bitmap;
								content.Content = image;
							}
							else
							{
								BitmapImage bitmap = new BitmapImage(new Uri("pack://application:,,,/Assets/Ic_close_48px.svg.png"));
								Image image = new Image();
								image.Source = bitmap;
								content.Content = image;

							}
                           
                        }
                    }
                }
            }
        }

        public void SaveGame()
        {
            _game.GameUpdated -= OnGameUpdated;

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("LastSave.save", FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, _game);
            stream.Close();
        }
    }
}