using SmartGenerator.Classes;
using SmartGenerator.Source;
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

namespace SmartGenerator.Windows
{
    /// <summary>
    /// Logique d'interaction pour ModelsMannager.xaml
    /// </summary>
    public partial class ModelsMannager : Window
    {
        public ModelsMannager()
        {
            InitializeComponent();
            InitListBox();
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

        public void InitListBox()
        {
            try
            {
                List<Models> ModelList = Treatments.InitModelsCB();
                ModelsListBox.ItemsSource = ModelList;
            }
            catch (Exception MyEx)
            {
                MessageBox.Show("[Smart Generator] Error Occured:" + MyEx.Message);
            }

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NewModel newModel = new NewModel();
            newModel.Show();
            this.Close();
        }

        private void Edit_Or_Remove(object sender, MouseButtonEventArgs e)
        {
            if (ModelsListBox.SelectedValue == null) return;
            var MyModel = (Models)ModelsListBox.SelectedValue;
            if (MyModel != null)
            {
                EditOrRemove window = new EditOrRemove(MyModel, this);
                window.Show();
            }
        }
    }
}
