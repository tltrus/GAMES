using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;


namespace Snake_Wave
{
    public partial class MainWindow : Window
    {
        Game game;

        public MainWindow()
        {
            InitializeComponent();

            game = new Game(g, 20, (int)g.Width, (int)g.Height);
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e) => game.Start();
    }
}
