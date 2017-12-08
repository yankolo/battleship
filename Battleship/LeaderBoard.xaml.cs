using System;
using System.Collections.Generic;
using System.IO;
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
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for LeaderBoard.xaml
    /// </summary>
    public partial class LeaderBoard : Window
    {
        public LeaderBoard()
        {
            InitializeComponent();
        }

        private void LeaderBoardGrid_Grd_Loaded(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists("Scores"))
            {
                string[] files = Directory.GetFiles("Scores", "*.score", SearchOption.TopDirectoryOnly);

                for (int i = 0; i < files.Length; i++)
                {
                    IFormatter formatter1 = new BinaryFormatter();
                    Stream stream1 = new FileStream(files[i], FileMode.Open, FileAccess.Read, FileShare.Read);
                    Score score = (Score)formatter1.Deserialize(stream1);
                    stream1.Close();
                    ScoreBoardData_Grd.Items.Add(score);



                }
            }
        }

        private void BackButton_Btn_Click(object sender, RoutedEventArgs e)
        {
            MainMenu m = new MainMenu();
            ((ContentControl)LeaderBoardGrid_Grd.Parent).Content = m.MainMenu_Grd;
        }

       
    }
}
