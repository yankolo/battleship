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
        private TextBlock output;

        private Game _game;

        /// <summary>
        /// Setup basics and create game window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            _game = new Game();
            InitializeGridCells();
            scoreboard.Content = "shots:\r\n" + _game.Shots;
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

                    BitmapImage bitmap = new BitmapImage(new Uri("pack://application:,,,/Assets/green.png"));
                    Image image = new Image();
                    image.Source = bitmap;
                    content.Content = image;

                    gameGrid.Children.Add(content);

                }
            }
        }

        private void Output_Loaded(object sender, RoutedEventArgs e)
        {
            this.output = sender as TextBlock;
        }

        /// <summary>
        /// Handles a click on a button from the field and will change color of it depending on the result
        /// </summary>
        public void HandleFieldClick(object sender, RoutedEventArgs e)
        {
            ContentControl b = (ContentControl)sender;
            Coordinate hitCoordinate = new Coordinate(Grid.GetColumn(b) - 1, Grid.GetRow(b) - 1);

            HitResult hitResult = _game.ShootOpponent(hitCoordinate);

            if (hitResult.Equals(HitResult.ShipShot))
            {
                //should be added in the game class
                this.output.Text = "That's a hit!";

                BitmapImage bitmap = new BitmapImage(new Uri("pack://application:,,,/Assets/red.png"));
                Image image = new Image();
                image.Source = bitmap;
                b.Content = image;
            }
            else if (hitResult.Equals(HitResult.WaterShot))
            {
                //should be added in the game class
                this.output.Text = "That was a shot in the water!";

                BitmapImage bitmap = new BitmapImage(new Uri("pack://application:,,,/Assets/blue.png"));
                Image image = new Image();
                image.Source = bitmap;
                b.Content = image;
            }
            else if (hitResult.Equals(HitResult.AlreadyRevealed))
            {
                //should be added in the game class 
                this.output.Text = "You already shot there.";
            }
            scoreboard.Content = "shots:\r\n" + _game.Shots;
            // To be done in the game class --> since the game should determine when a player won
            if (_game.IsGameWOn)
            {
                this.output.Text = "Congratulations you sunk every ship";
            }
        }
    }
}