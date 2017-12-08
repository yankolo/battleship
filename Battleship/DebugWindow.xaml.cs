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
    /// Interaction logic for DebugWindow.xaml
    /// </summary>
    public partial class DebugWindow : Window
    {
        private StartGameWindow _sgw;
        public DebugWindow(StartGameWindow sgw)
        {
            InitializeComponent();
            _sgw = sgw;
        }

        private void DebugOn_Btn_Click(object sender, RoutedEventArgs e)
        {
            StartGameWindow.debug = true;
            _sgw.Debug_Btn.Content = "Debug:On";
            ((ContentControl)Debug_Grd.Parent).Content = _sgw.CreateNewGame_Grd;
        }

        private void DebugOff_Btn_Click(object sender, RoutedEventArgs e)
        {
            StartGameWindow.debug = false;
            _sgw.Debug_Btn.Content = "Debug:Off";
            ((ContentControl)Debug_Grd.Parent).Content = _sgw.CreateNewGame_Grd;
        }
    }
}
