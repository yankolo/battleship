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
using Battleship;

namespace battleships
{
    /// <summary>
    /// Simple wpf battleships game
    /// </summary>
    public partial class MainWindow : Window
    {
        private Game _game;

        /// <summary>
        /// Setup basics and create game window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            _game = new Game();
            InitializeGridCells();
            UpdateGUI();
        }

        private void InitializeGridCells()
        {
            for (int x = 1; x < 11; x++)
            {
                for (int y = 1; y < 11; y++)
                {
                    ContentControl content = new ContentControl();
                    content.MouseDown += new MouseButtonEventHandler(HandleFieldClick);
                    Grid.SetColumn(content, x);
                    Grid.SetRow(content, y);

                    gameGrid.Children.Add(content);

                }
            }
        }

        /// <summary>
        /// Handles a click on a button from the field and will change color of it depending on the result
        /// </summary>
        public void HandleFieldClick(object sender, RoutedEventArgs e)
        {
            ContentControl b = (ContentControl)sender;
            Coordinate hitCoordinate = new Coordinate(Grid.GetColumn(b) - 1, Grid.GetRow(b) - 1);

            _game.ShootOpponent(hitCoordinate);
            UpdateGUI();
        }

        public void UpdateGUI()
        {
            maintext.Text = _game.DisplayedText;
            scoreboard.Content = "shots:\r\n" + _game.Shots;

            for (int i = 0; i < gameGrid.Children.Count; i++)
            {
                UIElement e = gameGrid.Children[i];
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
    }
}