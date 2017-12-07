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
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Battleship;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        private MediaPlayer player;
        public static MainWindow mw;
        public StartWindow()
        {
            InitializeComponent();
           
            player = new MediaPlayer();
            player.Open(new Uri(System.IO.Path.Combine(Environment.CurrentDirectory, "Sound/Two Steps From Hell - Archangel.mp3")));
            player.Play();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainMenu m = new MainMenu();
            Window_Ctn.Content = m.MainMenu_Grd;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {


            Grid grid = null;
            if (Window_Ctn.Content.GetType() == typeof(Grid))
            {
                grid = (Grid)Window_Ctn.Content;
                
            }

            if(grid != null)
                if (grid.Name == "Game_Grid" && e.Key == Key.Escape)
                {
                    for (int i = 0; i < grid.Children.Count; i++)
                    {
                        UIElement ue = grid.Children[i];

                        if (ue.GetType() == typeof(Grid))
                        {
                            Grid g = (Grid)ue;
                            if(g.Name == "inGameMenu_Grd")
                            {
                            for(int j = 0; j < g.Children.Count; j++)
                                    {
                                        UIElement uie = g.Children[j];
                                        if (uie.GetType() == typeof(ContentControl))
                                        {
                                            ContentControl content = (ContentControl)uie;
                                            if (content.Name == "inGameMenu_Ctn")
                                            {
                                                InGameMenu igm = new InGameMenu(content,grid);
                                                 content.Content = igm.InGameMenu_Grd;
                                                mw._game.PauseGame();


                                     }
                                }
                            }
                            
                                   
                          }

                            
                        }
                    }

                }
        }
    }
}
