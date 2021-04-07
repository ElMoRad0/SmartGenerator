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
using SmartGenerator.Classes;
using SmartGenerator.Source;
using System.IO;

namespace SmartGenerator
{
    /// <summary>
    /// Logique d'interaction pour NewFile.xaml
    /// </summary>
    public partial class NewFile : Window
    {
        Models ChoosenModel;

        public NewFile()
        {
            InitializeComponent();
            BindModelCB();
        }


        public void BindModelCB()
        {
            List<Models> AllModels = Treatments.InitModelsCB();
            ModelCB.ItemsSource = AllModels;
            ModelCB.DisplayMemberPath = "Name";
            ModelCB.SelectedValuePath = "ModelID";
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

        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ChoosenModel != null)
                {
                    NewFile2 Continue = new NewFile2(ChoosenModel);
                    Continue.Show();
                    Close();
                }
                else
                {
                    ErrtextBlock.Text = "Vous devez choisir un modèle.";
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error Occured:" + ex.Message);
            }
        }

        private void ModelCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ModelCB.SelectedItem == null) return;
            var MyModel = (Models)ModelCB.SelectedItem;
            if (MyModel != null)
            {
                ChoosenModel = MyModel;
            }
        }
    }
}
