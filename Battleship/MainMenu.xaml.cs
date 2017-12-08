using Battleship;
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
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

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
            CheckIfLoadExists();
        }

        private void CheckIfLoadExists()
        {
            if (File.Exists("LastSave.save") == false)
                Load_Btn.IsEnabled = false;
        }
       

        private void Start_Game_Btn_Click(object sender, RoutedEventArgs e)
        {
            StartGameWindow sw = new StartGameWindow();
            ((ContentControl)MainMenu_Grd.Parent).Content = sw.CreateNewGame_Grd;
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

        private void Load_Btn_Click(object sender, RoutedEventArgs e)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("LastSave.save", FileMode.Open, FileAccess.Read, FileShare.Read);
            Game game = (Game)formatter.Deserialize(stream);
            stream.Close();

            MainWindow dw = new MainWindow(game);
            StartWindow.mw = dw;
            ((ContentControl)MainMenu_Grd.Parent).Content = dw.Game_Grid;
			StartWindow.mw._game.ResumeTimers();
        }

        private void LeaderBoard_Click(object sender, RoutedEventArgs e)
        {
            LeaderBoard lb = new LeaderBoard();
            ((ContentControl)MainMenu_Grd.Parent).Content = lb.LeaderBoardGrid_Grd;
        }
    }
}
