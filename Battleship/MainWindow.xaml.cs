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
using WpfAnimatedGif;
using System.Windows.Media.Animation;

namespace Battleship
{
    /// <summary>
    /// Simple wpf battleships game
    /// </summary>
    public partial class MainWindow : Window
    {
        public Game _game;
        private bool _useDebug;
        /// <summary>
        /// Setup basics and create game window
        /// </summary>
        public MainWindow(Difficulty difficulty, String userName , bool debug)
        {
            InitializeComponent();
            _useDebug = debug;
            SetDebug(debug);
            _game = new Game(difficulty, userName,debug);
            _game.GameUpdated += OnGameUpdated;
            InitializeGridCells();
            UpdateAllGUI(true);
        }

        public MainWindow(Game game)
        {
            InitializeComponent();
            _game = game;
            _useDebug = _game.Debug;
            _game.GameUpdated += OnGameUpdated;
            InitializeGridCells();
            UpdateAllGUI(true);
        }


        private void InitializeGridCells()
        {
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    Grid grid = new Grid();
                    Grid.SetColumn(grid, x);
                    Grid.SetRow(grid, y);
                    grid.Background = Brushes.Transparent;

                    userGrid.Children.Add(grid);

                    grid = new Grid();
                    grid.MouseDown += new MouseButtonEventHandler(HandleFieldClick);
                    Grid.SetColumn(grid, x);
                    Grid.SetRow(grid, y);
                    grid.Background = Brushes.Transparent;

                    computerGrid.Children.Add(grid);
                }
            }
        }


        /// <summary>
        /// Event Handler for GameUpdated
        /// 
        /// Calls the methods to update the GUI (UpdateField() and UpdateAllGUI)
        /// 
        /// Before calling UpdateField, it finds the grid that represents the fields passed in the EventArgs
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGameUpdated(object sender, UpdatedFieldEventArgs e)
        {
            if (e.newField != null && e.oldField != null)
            {
                Coordinate coordinate; // The coordinate of the field
                Grid grid; // The grid that contains the grid representing the field
                Grid fieldGrid; // the grid representing the field

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

                // Finding the grid representing the field
                for (int i = 0; i < grid.Children.Count; i++)
                {
                    UIElement element = grid.Children[i];
                    if (element.GetType() == typeof(Grid))
                    {
                        Grid g = (Grid)element;

                        if (Grid.GetRow(g) == coordinate.Y && Grid.GetColumn(g) == coordinate.X)
                        {
                            fieldGrid = g;
                            UpdateField(fieldGrid, e.newField, e.oldField);
                        }
                    }
                }
            }

            UpdateAllGUI(); // SHOULD BE UpdateAllGUI(), IT IS  UpdateAllGUI(true) BECAUSE "X" ON HIT FIELDS ARE SET IN THIS METHOD
        }

        /// <summary>
        /// Handles a click on a button from the field and will change color of it depending on the result
        /// </summary>
        public void HandleFieldClick(object sender, RoutedEventArgs e)
        {
            if (_game.IsGamePaused == false)
            {
                Grid b = (Grid)sender;
                Coordinate hitCoordinate = new Coordinate(Grid.GetColumn(b), Grid.GetRow(b));

                _game.UpdateGame(hitCoordinate);
            }
        }

        public void UpdateField(Grid grid, Field newField, Field oldField)
        {
            if ((oldField.IsHit != newField.IsHit))
            {
                if (newField.Ship == null) // If water
                {
                    Image img = new Image();
                    grid.Children.Add(img);
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource = new Uri("pack://application:,,,/Assets/waterhit.gif");
                    image.EndInit();
                    ImageBehavior.SetAnimatedSource(img, image);
                    ImageBehavior.SetRepeatBehavior(img, new RepeatBehavior(1));

                    // Code to display animation and sound when ship is hit
                }
                else // If ship
                {
                    Image img = new Image();
                    grid.Children.Add(img);
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource = new Uri("pack://application:,,,/Assets/shiphit.gif");
                    image.EndInit();
                    ImageBehavior.SetAnimatedSource(img, image);
                    ImageBehavior.SetRepeatBehavior(img, new RepeatBehavior(1));

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
            if (_game.Debug == true)
            {
                Debug_btn.Visibility = Visibility.Visible;
            }
            else
            {
                Debug_btn.Visibility = Visibility.Hidden;
            }

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
                if (e.GetType() == typeof(Grid))
                {
                    Grid grid = (Grid)e;
                    Coordinate coordinate = new Coordinate(Grid.GetColumn(grid), Grid.GetRow(grid));
                    Field field = _game.RadarBoard.GetField(coordinate);

                    if (field.IsRevealed == false)
                    {
                        grid.Children.Clear();

                        BitmapImage bitmap = new BitmapImage(new Uri("pack://application:,,,/Assets/transparent.png"));
                        Image image = new Image();
                        image.Source = bitmap;
                        grid.Children.Add(image);
                    }
                    else if (field.IsRevealed == true)
                    {
                        if (field.Ship == null)
                        {
                            grid.Children.Clear();

                            BitmapImage bitmap = new BitmapImage(new Uri("pack://application:,,,/Assets/lastWaterHitFrame.gif"));
                            Image image = new Image();
                            image.Source = bitmap;
                            grid.Children.Add(image);
                        }
                        else
                        {
                            grid.Children.Clear();

                            BitmapImage bitmap = new BitmapImage(new Uri("pack://application:,,,/Assets/lastShipHitFrame.gif"));
                            Image image = new Image();
                            image.Source = bitmap;
                            grid.Children.Add(image);
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
                if (e.GetType() == typeof(Grid))
                {
                    Grid grid = (Grid)e;
                    Coordinate coordinate = new Coordinate(Grid.GetColumn(grid), Grid.GetRow(grid));
                    Field field = _game.UserBoard.GetField(coordinate);
                    if (field.IsRevealed == false)
                    {
                        grid.Children.Clear();

                        BitmapImage bitmap = new BitmapImage(new Uri("pack://application:,,,/Assets/transparent.png"));
                        Image image = new Image();
                        image.Source = bitmap;
                        grid.Children.Add(image);
                    }
                    else if (field.IsRevealed == true)
                    {
                        if (field.Ship == null)
                        {
                            if (field.IsHit == false)
                            {
                                grid.Children.Clear();

                                BitmapImage bitmap = new BitmapImage(new Uri("pack://application:,,,/Assets/transparent.png"));
                                Image image = new Image();
                                image.Source = bitmap;
                                grid.Children.Add(image);
                            }
                            else
                            {
                                grid.Children.Clear();

                                BitmapImage bitmap = new BitmapImage(new Uri("pack://application:,,,/Assets/lastWaterHitFrame.gif"));
                                Image image = new Image();
                                image.Source = bitmap;
                                grid.Children.Add(image);
                            }

                        }
                        else
                        {
                            if (field.IsHit == false)
                            {
                                grid.Children.Clear();

                                BitmapImage bitmap = new BitmapImage(new Uri("pack://application:,,,/Assets/red.png"));
                                Image image = new Image();
                                image.Source = bitmap;
                                grid.Children.Add(image);


                            }
                            else
                            {
                                grid.Children.Clear();

                                BitmapImage bitmap = new BitmapImage(new Uri("pack://application:,,,/Assets/red.png"));
                                Image image = new Image();
                                image.Source = bitmap;
                                grid.Children.Add(image);

                                Image secondImage = new Image();
                                secondImage.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/lastShipHitFrame.gif"));
                                grid.Children.Add(secondImage);
                                // ADD LAST FRAME OF X ON TOP

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
        private void SetDebug(bool debug)
        {
            if (debug)
            {
                Debug_btn.Visibility = Visibility.Visible;
            }

        }

        private void Debug_btn_Click(object sender, RoutedEventArgs e)
        {
            if (_useDebug)
            {

                _game.RevealAll();
                _useDebug = false;
                Debug_btn.Content = "Hide All";
                UpdateAllGUI(true);
            }
            else
            {
                _game.HideAll();
                _useDebug = true;
                Debug_btn.Content = "Reveal All";
                UpdateAllGUI(true);
            }
        }
    }
}