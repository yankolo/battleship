using battleships;
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
    /// Interaction logic for InGameMenu.xaml
    /// </summary>
    public partial class InGameMenu : Window
    {
        private ContentControl _contentControl;
        private Grid _parent;
        
        public InGameMenu(ContentControl content , Grid parent )
        {
            InitializeComponent();
            _contentControl = content;
            _parent = parent;
            
            
        }

        private void Resume_Btn_Click(object sender, RoutedEventArgs e)
        {
            _contentControl.Content = null;
            
        }

        private void Quit_BtnBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ReturnToMainMenu_Btn_Click(object sender, RoutedEventArgs e)
        {

            MainMenu mainMenu = new MainMenu();
            ((ContentControl)_parent.Parent).Content = mainMenu.MainMenu_Grd;


        }
    }
}
