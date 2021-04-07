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
using System.IO;
using SmartGenerator.Classes;

namespace SmartGenerator
{
    /// <summary>
    /// Logique d'interaction pour NewModel.xaml
    /// </summary>
    public partial class NewModel : Window
    {
        string Mode;
        Models EditedModel;
        public NewModel(string argMode="New", Models selectedToEditModel=null) 
        {
            this.Mode = argMode;
            this.EditedModel = selectedToEditModel;
            InitializeComponent();
            if(this.Mode != "New" && this.EditedModel != null)
            {
                NametextBox.Text = this.EditedModel.Name;
            }
        }

        private void MenuItemFile_Click(object sender, RoutedEventArgs e)
        {
            NewFile NewWindow = new NewFile();
            this.Close();
            NewWindow.Show();
        }
        private void MenuItemModel_Click(object sender, RoutedEventArgs e)
        {
            NewModel NewModelWindow = new NewModel();
            NewModelWindow.Show();
            Close();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainMenu Menu = new MainMenu();
            Menu.Show();
            this.Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Models MyModel = new Models(999, NametextBox.Text); //ID 999 est temporaire, il sera changé à la fin de la création du modèle
                NewModel2 NewModelWindow2 = new NewModel2(NametextBox.Text, this.Mode, this.EditedModel);
                NewModelWindow2.Show();
                Close();
            }
            catch(Exception MyEx)
            {
                MessageBox.Show("[Smart Generator] Error Occured: " + MyEx.Message);
            }
        }
    }
}
