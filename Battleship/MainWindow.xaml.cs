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

        Border selectionBorder; // When hovering over field

        /// <summary>
        /// Setup basics and create game window
        /// </summary>
        public MainWindow(Difficulty difficulty, String userName , bool debug, TimeSpan timeForUserTurn)
        {
            InitializeComponent();
            SetDebug(debug);
            _game = new Game(difficulty, userName,debug, timeForUserTurn);
            _game.GameUpdated += OnGameUpdated;
            _game.GameFinished += OnGameFinished;
            InitializeGridCells();
            selectionBorder = new Border();
            selectionBorder.Name = "SelectionBorder";
            UpdateAllGUI(true);
        }

        public MainWindow(Game game)
        {
            InitializeComponent();
            _game = game;
            _game.GameUpdated += OnGameUpdated;
            _game.GameFinished += OnGameFinished;
            selectionBorder = new Border();
            selectionBorder.Name = "SelectionBorder";
            InitializeGridCells();
            UpdateAllGUI(true);
        }


        private void InitializeGridCells()
        {
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    Border border = new Border();
                    border.BorderThickness = new Thickness(0.1);
                    border.BorderBrush = Brushes.Black;
                    Grid.SetColumn(border, x);
                    Grid.SetRow(border, y);

                    userGrid.Children.Add(border);

                    Grid grid = new Grid();
                    Grid.SetColumn(grid, x);
                    Grid.SetRow(grid, y);
                    grid.Background = Brushes.Transparent;

                    userGrid.Children.Add(grid);

                    if (x == 0)
                    {
                        border = new Border();
                        border.BorderThickness = new Thickness(2, 0, 0, 0);
                        border.BorderBrush = Brushes.Black;
                        Grid.SetColumn(border, x);
                        Grid.SetRow(border, y);

                        computerGrid.Children.Add(border);
                    }

                    border = new Border();
                    border.BorderThickness = new Thickness(0.1);
                    border.BorderBrush = Brushes.Black;
                    Grid.SetColumn(border, x);
                    Grid.SetRow(border, y);

                    computerGrid.Children.Add(border);

                    grid = new Grid();
                    grid.MouseDown += new MouseButtonEventHandler(HandleFieldClick);
                    //grid.MouseLeave += new MouseEventHandler(OnMouseLeave);
                    Grid.SetColumn(grid, x);
                    Grid.SetRow(grid, y); 
                    grid.Background = Brushes.Transparent;

                    computerGrid.Children.Add(grid);
                }
            }
        }

        private void OnGameFinished(object sender, EventArgs e)
        {
            if (_game.IsGameWOn == true)
            {
                WinLoss wl = new WinLoss();
                wl.Title.Text = "You won!";
                inGameMenu_Ctn.Content = wl.WinLoss_Grd;
            }
            else
            {
                WinLoss wl = new WinLoss();
                wl.Title.Text = "You Lost!";
                inGameMenu_Ctn.Content = wl.WinLoss_Grd;
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

                    Ship ship = newField.Ship;

                    if (ship.IsSunk)
                        for (int i = 0; i < computerGrid.Children.Count; i++)
                        {
                            UIElement e = computerGrid.Children[i];
                            if (e.GetType() == typeof(Grid))
                            {
                                Grid shipGrid = (Grid)e;
                                Coordinate coordinate = new Coordinate(Grid.GetColumn(shipGrid), Grid.GetRow(shipGrid));
                                Field field = _game.RadarBoard.GetField(coordinate);

                                if (field.Ship == ship)
                                {
                                    DisplayShip(field, shipGrid, true);
                                }
                            }
                        }
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
            //userText.Text = _game.DisplayedUserText;
            //cpuText.Text = _game.DisplayedCPUText;
            //userScoreboard.Content = "shots:\r\n" + _game.UserShots;
            //cpuScoreboard.Content = "shots:\r\n" + _game.CpuShots;
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

                    if (_game.IsCurrentlyDebug)
                    {
                        grid.Children.Clear();

                        if (field.Ship != null)
                            DisplayShip(field, grid);
                    }
                    else if (field.IsRevealed == false)
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
                            grid.Children.Clear();

                            if (field.Ship.IsSunk)
                                DisplayShip(field, grid);

                            if (field.IsHit == true)
                            {
                                // ADD LAST FRAME OF X ON TOP
                                Image secondImage = new Image();
                                secondImage.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/lastShipHitFrame.gif"));
                                grid.Children.Add(secondImage);
                            }
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
                            grid.Children.Clear();

                            DisplayShip(field, grid);

                            if (field.IsHit == true)
                            {
                                // ADD LAST FRAME OF X ON TOP
                                Image secondImage = new Image();
                                secondImage.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/lastShipHitFrame.gif"));
                                grid.Children.Add(secondImage);
                            }
                        }
                    }
                }
            }
        }

        private void DisplayShip(Field field, Grid grid, bool displayBehind = false)
        {
            switch (field.Ship.ActualSize)
            {
                //Submarine (Length 2)
                case 2:
                    if (field.ShipDirection.Equals(Direction.Horizontal))
                    {
                        switch (field.ShipFieldPosition)
                        {
                            case 0:
                                Image image = new Image();
                                image.Stretch = Stretch.Fill;
                                image.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Ships/SubmarineBack.png")); ;
                                if (displayBehind)
                                    grid.Children.Insert(0, image);
                                else
                                    grid.Children.Add(image);
                                break;
                            case 1:
                                image = new Image();
                                image.Stretch = Stretch.Fill;
                                image.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Ships/SubmarineFront.png")); ;
                                if (displayBehind)
                                    grid.Children.Insert(0, image);
                                else
                                    grid.Children.Add(image);
                                break;
                        }
                    }
                    else
                    {
                        switch (field.ShipFieldPosition)
                        {
                            case 0:
                                Image image = new Image();
                                image.Stretch = Stretch.Fill;
                                image.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Ships/VerSubmarineFront.png")); ;
                                if (displayBehind)
                                    grid.Children.Insert(0, image);
                                else
                                    grid.Children.Add(image);
                                break;
                            case 1:
                                image = new Image();
                                image.Stretch = Stretch.Fill;
                                image.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Ships/VerSubmarineBack.png")); ;
                                if (displayBehind)
                                    grid.Children.Insert(0, image);
                                else
                                    grid.Children.Add(image);
                                break;
                        }
                    }
                    break;

                //Destroyer (Length 3)
                case 3:
                    if (field.ShipDirection.Equals(Direction.Horizontal))
                    {
                        switch (field.ShipFieldPosition)
                        {
                            case 0:
                                Image image = new Image();
                                image.Stretch = Stretch.Fill;
                                image.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Ships/DestroyerBack.png")); ;
                                if (displayBehind)
                                    grid.Children.Insert(0, image);
                                else
                                    grid.Children.Add(image);
                                break;
                            case 1:
                                image = new Image();
                                image.Stretch = Stretch.Fill;
                                image.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Ships/DestroyerMiddle.png")); ;
                                if (displayBehind)
                                    grid.Children.Insert(0, image);
                                else
                                    grid.Children.Add(image);
                                break;
                            case 2:
                                image = new Image();
                                image.Stretch = Stretch.Fill;
                                image.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Ships/DestroyerFront.png")); ;
                                if (displayBehind)
                                    grid.Children.Insert(0, image);
                                else
                                    grid.Children.Add(image);
                                break;
                        }
                    }
                    else
                    {
                        switch (field.ShipFieldPosition)
                        {
                            case 0:
                                Image image = new Image();
                                image.Stretch = Stretch.Fill;
                                image.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Ships/VerDestroyerBack.png")); ;
                                if (displayBehind)
                                    grid.Children.Insert(0, image);
                                else
                                    grid.Children.Add(image);
                                break;
                            case 1:
                                image = new Image();
                                image.Stretch = Stretch.Fill;
                                image.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Ships/VerDestroyerMiddle.png")); ;
                                if (displayBehind)
                                    grid.Children.Insert(0, image);
                                else
                                    grid.Children.Add(image);
                                break;
                            case 2:
                                image = new Image();
                                image.Stretch = Stretch.Fill;
                                image.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Ships/VerDestroyerFront.png")); ;
                                if (displayBehind)
                                    grid.Children.Insert(0, image);
                                else
                                    grid.Children.Add(image);
                                break;
                        }
                    }
                    break;

                //Carrier (Length 4)
                case 4:
                    if (field.ShipDirection.Equals(Direction.Horizontal))
                    {
                        switch (field.ShipFieldPosition)
                        {
                            case 0:
                                Image image = new Image();
                                image.Stretch = Stretch.Fill;
                                image.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Ships/CarrierBack.png")); ;
                                if (displayBehind)
                                    grid.Children.Insert(0, image);
                                else
                                    grid.Children.Add(image);
                                break;
                            case 1:
                                image = new Image();
                                image.Stretch = Stretch.Fill;
                                image.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Ships/CarrierMiddleBack.png")); ;
                                if (displayBehind)
                                    grid.Children.Insert(0, image);
                                else
                                    grid.Children.Add(image);
                                break;
                            case 2:
                                image = new Image();
                                image.Stretch = Stretch.Fill;
                                image.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Ships/CarrierMiddleFront.png")); ;
                                if (displayBehind)
                                    grid.Children.Insert(0, image);
                                else
                                    grid.Children.Add(image);
                                break;
                            case 3:
                                image = new Image();
                                image.Stretch = Stretch.Fill;
                                image.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Ships/CarrierFront.png")); ;
                                if (displayBehind)
                                    grid.Children.Insert(0, image);
                                else
                                    grid.Children.Add(image);
                                break;
                        }
                    }
                    else
                    {
                        switch (field.ShipFieldPosition)
                        {
                            case 0:
                                Image image = new Image();
                                image.Stretch = Stretch.Fill;
                                image.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Ships/VerCarrierBack.png")); ;
                                if (displayBehind)
                                    grid.Children.Insert(0, image);
                                else
                                    grid.Children.Add(image);
                                break;
                            case 1:
                                image = new Image();
                                image.Stretch = Stretch.Fill;
                                image.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Ships/VerCarrierMiddleBack.png")); ;
                                if (displayBehind)
                                    grid.Children.Insert(0, image);
                                else
                                    grid.Children.Add(image);
                                break;
                            case 2:
                                image = new Image();
                                image.Stretch = Stretch.Fill;
                                image.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Ships/VerCarrierMiddleFront.png")); ;
                                if (displayBehind)
                                    grid.Children.Insert(0, image);
                                else
                                    grid.Children.Add(image);
                                break;
                            case 3:
                                image = new Image();
                                image.Stretch = Stretch.Fill;
                                image.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Ships/VerCarrierFront.png")); ;
                                if (displayBehind)
                                    grid.Children.Insert(0, image);
                                else
                                    grid.Children.Add(image);
                                break;
                        }
                    }
                    break;

                //Battleship
                case 5:
                    if (field.ShipDirection.Equals(Direction.Horizontal))
                    {
                        switch (field.ShipFieldPosition)
                        {
                            case 0:
                                Image image = new Image();
                                image.Stretch = Stretch.Fill;
                                image.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Ships/BattleshipBack.png")); ;
                                if (displayBehind)
                                    grid.Children.Insert(0, image);
                                else
                                    grid.Children.Add(image);
                                break;
                            case 1:
                                image = new Image();
                                image.Stretch = Stretch.Fill;
                                image.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Ships/BattleshipBack2.png")); ;
                                if (displayBehind)
                                    grid.Children.Insert(0, image);
                                else
                                    grid.Children.Add(image);
                                break;
                            case 2:
                                image = new Image();
                                image.Stretch = Stretch.Fill;
                                image.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Ships/BattleshipMiddle.png")); ;
                                if (displayBehind)
                                    grid.Children.Insert(0, image);
                                else
                                    grid.Children.Add(image);
                                break;
                            case 3:
                                image = new Image();
                                image.Stretch = Stretch.Fill;
                                image.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Ships/BattleshipFront2.png")); ;
                                if (displayBehind)
                                    grid.Children.Insert(0, image);
                                else
                                    grid.Children.Add(image);
                                break;
                            case 4:
                                image = new Image();
                                image.Stretch = Stretch.Fill;
                                image.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Ships/BattleshipFront.png")); ;
                                if (displayBehind)
                                    grid.Children.Insert(0, image);
                                else
                                    grid.Children.Add(image);
                                break;
                        }
                    }
                    else
                    {
                        switch (field.ShipFieldPosition)
                        {
                            case 0:
                                Image image = new Image();
                                image.Stretch = Stretch.Fill;
                                image.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Ships/VerBattleshipBack.png")); ;
                                if (displayBehind)
                                    grid.Children.Insert(0, image);
                                else
                                    grid.Children.Add(image);
                                break;
                            case 1:
                                image = new Image();
                                image.Stretch = Stretch.Fill;
                                image.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Ships/VerBattleshipBack2.png")); ;
                                if (displayBehind)
                                    grid.Children.Insert(0, image);
                                else
                                    grid.Children.Add(image);
                                break;
                            case 2:
                                image = new Image();
                                image.Stretch = Stretch.Fill;
                                image.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Ships/VerBattleshipMiddle.png")); ;
                                if (displayBehind)
                                    grid.Children.Insert(0, image);
                                else
                                    grid.Children.Add(image);
                                break;
                            case 3:
                                image = new Image();
                                image.Stretch = Stretch.Fill;
                                image.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Ships/VerBattleshipFront2.png")); ;
                                if (displayBehind)
                                    grid.Children.Insert(0, image);
                                else
                                    grid.Children.Add(image);
                                break;
                            case 4:
                                image = new Image();
                                image.Stretch = Stretch.Fill;
                                image.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Ships/VerBattleshipFront.png")); ;
                                if (displayBehind)
                                    grid.Children.Insert(0, image);
                                else
                                    grid.Children.Add(image);
                                break;
                        }
                    }
                    break;
            }
        }

        public void SaveGame()
        {
            _game.GameUpdated -= OnGameUpdated;
            _game.GameFinished -= OnGameFinished;

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
            if (_game.IsCurrentlyDebug == false)
            {
                _game.PauseGame();
                _game.RevealAll();
                _game.IsCurrentlyDebug = true;
                Debug_btn.Content = "Hide All";
                UpdateAllGUI(true);
            }
            else
            {
                _game.UnPauseGame();
                _game.HideAll();
                _game.IsCurrentlyDebug = false;
                Debug_btn.Content = "Reveal All";
                UpdateAllGUI(true);
            }
        }
    }
}