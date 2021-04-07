using SmartGenerator.Windows;
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
    /// Logique d'interaction pour MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        StackPanel MainMenuStack = new StackPanel();
        StackPanel ModelMannagerStack = new StackPanel();

        public MainMenu()
        {
            InitializeComponent();
        }
        
        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow Help = new HelpWindow();
            Help.Show();
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("Voulez-vous vraiment quitter l'application ?", "Quit Confirmation", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    Close();
                }
            }
            catch(Exception MyEx)
            {
                MessageBox.Show("[Smart Generator] Error Occured:" + MyEx.Message);
            }
            
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            NewFile NewFileWindow = new NewFile();
            NewFileWindow.Show();
            Close();
        }

        private void NewModelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ModelsMannager ModelMannager = new ModelsMannager();
                ModelMannager.Show();
                Close();
            }
            catch(Exception)
            {
                MessageBox.Show("Impossible d'accéder au gestionnaire des modèles, le fichier guide des modèles est introuvable ou inaccéssible.");
            }
        }
    }
}
