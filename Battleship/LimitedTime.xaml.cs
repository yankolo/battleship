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
    /// Interaction logic for LimitedTime.xaml
    /// </summary>
    public partial class LimitedTime : Window
    {
        private StartGameWindow _sgw;
        public LimitedTime(StartGameWindow sgw)
        {
            InitializeComponent();
            _sgw = sgw;
        }

        private void LimitedOff_Click(object sender, RoutedEventArgs e)
        {
            StartGameWindow.time = 0;
            _sgw.TIme_Btn.Content = "Limited Time:Off";
            ((ContentControl)MainGrid_Grd.Parent).Content = _sgw.CreateNewGame_Grd;
        }

        private void Limited20_Click(object sender, RoutedEventArgs e)
        {
            StartGameWindow.time = 20;
            _sgw.TIme_Btn.Content = "Limited Time:20 sec";
            ((ContentControl)MainGrid_Grd.Parent).Content = _sgw.CreateNewGame_Grd;
        }

        private void Limited40_Click(object sender, RoutedEventArgs e)
        {
            StartGameWindow.time = 40;
            _sgw.TIme_Btn.Content = "Limited Time:40 sec";
            ((ContentControl)MainGrid_Grd.Parent).Content = _sgw.CreateNewGame_Grd;
        }

        private void Limited60_Click(object sender, RoutedEventArgs e)
        {
            StartGameWindow.time = 60;
            _sgw.TIme_Btn.Content = "Limited Time:60 sec";
            ((ContentControl)MainGrid_Grd.Parent).Content = _sgw.CreateNewGame_Grd;
        }
    }
}
