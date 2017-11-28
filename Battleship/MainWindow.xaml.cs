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
using System.Text.RegularExpressions;

// number code:
// 0 - water
// 1 - ship
// 2 - hit ship
// 3 - hit water

namespace battleships
{
    /// <summary>
    /// Simple wpf battleships game
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random rand;
        private int[,] field;
        private Regex reg;
        private TextBlock output;
        private int shots = 0;
        private int trialCounter;
        private bool failedToCreate;
        private int hits = 0;


        /// <summary>
        /// Setup basics and create game window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.rand = new Random();
            this.reg = new Regex(@"^\d,\d$", RegexOptions.IgnoreCase);
            this.InitGame();
            scoreboard.Content = "shots:\r\n" + this.shots;
        }

        private void InitGame()
        {
            this.failedToCreate = false;
            this.trialCounter = 0;
            this.CreateField();
            this.FillField();
        }

        private void Output_Loaded(object sender, RoutedEventArgs e)
        {
            this.output = sender as TextBlock;
        }

        /// <summary>
        /// Handles a click on a button from the field and will change color of it depending on the result
        /// </summary>
        public void handleFieldClick(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            string[] coords = b.Name.Split('y');
            coords[0] = coords[0].Replace("x", "");
            // buttons start at 1,1 array at 0,0
            int x = int.Parse(coords[0]);
            int y = int.Parse(coords[1]);

            if (this.field[y, x] == 1)
            {
                this.output.Text = "That's a hit!";
                b.Background = Brushes.Red;
                this.field[y, x] = 2;
                this.shots += 1;
                this.hits += 1;
            }
            else if (this.field[y, x] == 0)
            {
                b.Background = Brushes.Blue;
                this.output.Text = "That was a shot in the water!";
                this.field[y, x] = 3;
                this.shots += 1;
            }
            else if (this.field[y, x] == 2 || this.field[y, x] == 3)
            {
                this.output.Text = "You already shot there.";
            }
            scoreboard.Content = "shots:\r\n" + this.shots;

            if (this.hits >= 30)
            {
                this.output.Text = "Congratulations you sunk every ship";
            }
        }

        /// <summary>
        /// Setup empty field
        /// </summary>
        private void CreateField()
        {
            this.field = new int[12, 12];
        }

        /// <summary>
        /// Place the ships the player is supposed to shoot at in the end 
        /// </summary>
        private void FillField()
        {
            this.GenerateShip(5);

            this.GenerateShip(4);
            this.GenerateShip(4);

            this.GenerateShip(3);
            this.GenerateShip(3);
            this.GenerateShip(3);

            this.GenerateShip(2);
            this.GenerateShip(2);
            this.GenerateShip(2);
            this.GenerateShip(2);

            // redo when failed to create game
            if (this.failedToCreate)
            {
                this.InitGame();
            }
        }

        /// <summary>
        /// Place a ship randomly on the field checking if the position is valid beforehand
        /// </summary>
        /// <param name="size"></param>
        private void GenerateShip(int size)
        {
            if (this.trialCounter > 50)
            {
                this.failedToCreate = true;
                return;
            }
            this.trialCounter++;
            int num1 = this.rand.Next(1, 11);
            int num2 = this.rand.Next(1, 11);
            bool horizontal = this.rand.Next(0, 2) == 0;
            try
            {
                this.EnsureIsValidField(num1, num2);
                for (int i = 0; i < size; i++)
                {
                    if (horizontal)
                    {
                        this.EnsureIsValidField(num1 + i, num2);
                        this.EnsureIsValidField(num1 + i + 1, num2, true);
                        this.EnsureIsValidField(num1 + i - 1, num2, true);
                        this.EnsureIsValidField(num1 + i, num2 + 1, true);
                        this.EnsureIsValidField(num1 + i, num2 - 1, true);
                    }
                    else
                    {
                        this.EnsureIsValidField(num1, num2 + i);
                        this.EnsureIsValidField(num1, num2 + i + 1, true);
                        this.EnsureIsValidField(num1, num2 + i - 1, true);
                        this.EnsureIsValidField(num1 + 1, num2 + i, true);
                        this.EnsureIsValidField(num1 - 1, num2 + i, true);
                    }

                }
            }
            catch (IndexOutOfRangeException)
            {
                this.GenerateShip(size);
                return;
            }

            if (horizontal)
            {
                for (var i = 0; i < size; i++)
                    this.field[num1 + i, num2] = 1;
            }
            else
            {
                for (var i = 0; i < size; i++)
                    this.field[num1, num2 + i] = 1;
            }
        }

        /// <summary>
        /// Validates a field on the game to make sure a ship can be placed there
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="isSurrouding"></param>
        private void EnsureIsValidField(int x, int y, bool isSurrouding = false)
        {
            if (isSurrouding && this.field[x, y] == 1 || x > 12 || y > 12)
            {
                throw new IndexOutOfRangeException();
            }
            if (this.field[x, y] == 1 || x > 11 || y > 11)
            {
                throw new IndexOutOfRangeException();
            }

        }
    }
}