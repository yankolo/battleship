using battleships;
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
        public MainMenu()
        {
            InitializeComponent();

        }

       

        private void Start_Game_Btn_Click(object sender, RoutedEventArgs e)
        {
            StartGameWindow sw = new StartGameWindow();
            ((ContentControl)MainMenu_Grd.Parent).Content = sw.Difficulties_Grd;
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
