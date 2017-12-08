using Battleship;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for StartGameWindow.xaml
    /// </summary>
    public partial class StartGameWindow : Window
    {
        public static int time = 0;
        public static String difficulty = "Easy";
        public static bool debug = false;
        public static String userName;
        public StartGameWindow()
        {
            InitializeComponent();
           
        }

        private void Difficulty_Btn_Click(object sender, RoutedEventArgs e)
        {
            Difficulties d = new Difficulties(this);
            
            ((ContentControl)CreateNewGame_Grd.Parent).Content = d.Difficulties_Grd;
        }

        private void UserName_TBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (UserName_TBox.Text.Equals("Enter your User Name")) {
                UserName_TBox.Text = "";
                UserName_TBox.Foreground = new System.Windows.Media.SolidColorBrush(Colors.Black);
            }
        }

        private void UserName_TBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(UserName_TBox.Text))
            {
                UserName_TBox.Text = "Enter your User Name";
                UserName_TBox.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFB0A7A7"));

            }
            else
            {
                userName = UserName_TBox.Text;
            }
        }

        private void Start_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(UserName_TBox.Text) || UserName_TBox.Text.Equals("Enter your User Name"))
            {
                // Create Default Name Player 1 2 3 4 5 
            }
            else
            {
                Difficulty d = UserChoice(difficulty);
                MainWindow dw = new MainWindow(d,userName,debug);
                StartWindow.mw = dw;
                userName = "";
                ((ContentControl)CreateNewGame_Grd.Parent).Content = dw.Game_Grid;
            }
        }

        private void Back_Btn_Click(object sender, RoutedEventArgs e)
        {
            MainMenu m = new MainMenu();
            ((ContentControl)CreateNewGame_Grd.Parent).Content = m.MainMenu_Grd;
        }

        private Difficulty UserChoice(String choice)
        {
            Difficulty d;
            switch (choice)
            {
                case "Easy":
                    d= Difficulty.Easy;
                    break;
                case "Medium":
                    d = Difficulty.Medium;
                    break;
                default:
                    d = Difficulty.Hard;
                    break;


            }
            return d;
        }

        private void Debug_Btn_Click(object sender, RoutedEventArgs e)
        {
            DebugWindow dw = new DebugWindow(this);
            ((ContentControl)CreateNewGame_Grd.Parent).Content = dw.Debug_Grd;
        }

        private void TIme_Btn_Click(object sender, RoutedEventArgs e)
        {
            LimitedTime lt = new LimitedTime(this);
            ((ContentControl)CreateNewGame_Grd.Parent).Content = lt.MainGrid_Grd;
        }
    }
}
