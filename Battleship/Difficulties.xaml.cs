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
    /// Interaction logic for Difficulties.xaml
    /// </summary>
    public partial class Difficulties : Window
    {
        private StartGameWindow _sgw;
        public Difficulties(StartGameWindow sgw)
        {
            InitializeComponent();
            _sgw = sgw;
            
            
        }

       

        private void MediumDifficulty_Btnn_Click(object sender, RoutedEventArgs e)
        {
            
            StartGameWindow.difficulty = "Medium";
            _sgw.Difficulty_Btn.Content = "Difficulty:Medium";
            ((ContentControl)Difficulties_Grd.Parent).Content = _sgw.CreateNewGame_Grd;
        }

        private void HardDifficulty_Btn_Click(object sender, RoutedEventArgs e)
        {
            
            StartGameWindow.difficulty = "Hard";
            _sgw.Difficulty_Btn.Content = "Difficulty:Hard";
            ((ContentControl)Difficulties_Grd.Parent).Content = _sgw.CreateNewGame_Grd;
        }

        private void EasyDifficulty_Btn_Click(object sender, RoutedEventArgs e)
        {
            
            StartGameWindow.difficulty = "Easy";
            _sgw.Difficulty_Btn.Content = "Difficulty:Easy";
            ((ContentControl)Difficulties_Grd.Parent).Content = _sgw.CreateNewGame_Grd;
        }
        
    }
}
