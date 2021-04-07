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

namespace SmartGenerator
{
    /// <summary>
    /// Logique d'interaction pour HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        public HelpWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string mail = "mourad.elmrabet97@gmail.com";
            MessageBox.Show("Veuillez envoyer vos question et vos problèmes à notre Support:\r\n          " + mail, "Support");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Smart Generator. Version 0.1 Beta, réalisé par El Mrabet Mourad", "Smart Generator - Info");
        }
    }
}
