using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        private MediaPlayer player = new MediaPlayer();
        public MainMenu()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            player.Open(new Uri("Audio/Strength of a Thousand Men - Two Steps from Hell.mp3"));

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            player.Stop();
        }

        private void Start_Game_Btn_Click(object sender, RoutedEventArgs e)
        {
            Start_Game_Btn.Visibility = Visibility.Hidden;
            Difficulty_Txt.Visibility = Visibility.Visible;
            EasyDifficulty_Btn.Visibility = Visibility.Visible;
            MediumDifficulty_Btn.Visibility = Visibility.Visible;
            HardDifficulty_Btn.Visibility = Visibility.Visible;
        }

        private void Quit_Btn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Help_Btn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("To start a game of Battleship, PRESS START. You will then be prompted to choose from 3 different levels of difficulty. Choose one " +
                "and you will be able to play and enjoy our version of Battleship.");
        }
    }
}
