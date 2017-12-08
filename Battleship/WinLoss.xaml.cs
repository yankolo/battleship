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
    /// Interaction logic for WinLoss.xaml
    /// </summary>
    public partial class WinLoss : Window
    {
        
        public WinLoss()
        {
            InitializeComponent();
          
        }

        private void Leave_Btn_Click(object sender, RoutedEventArgs e)
        {
            
            MainMenu mainMenu = new MainMenu();
            ContentControl c = ((ContentControl)(WinLoss_Grd.Parent));
            Grid g = (Grid)c.Parent;
            Grid gParent = (Grid)g.Parent;
            ((ContentControl)gParent.Parent).Content = mainMenu.MainMenu_Grd;
        }
    }
}
