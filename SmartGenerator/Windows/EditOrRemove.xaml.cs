using SmartGenerator.Classes;
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

namespace SmartGenerator.Windows
{
    /// <summary>
    /// Logique d'interaction pour EditOrRemove.xaml
    /// </summary>
    public partial class EditOrRemove : Window
    {
        ModelsMannager parent;
        Models SelectedModel;
        public EditOrRemove(Models SelectedModel, ModelsMannager Paren)
        {
            InitializeComponent();
            this.SelectedModel = SelectedModel;
            this.parent = Paren;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            /*SelectedModel.EditFromModelsGuide();
            MessageBox.Show("Successfully Edited");
            this.Close();
            Parent.InitListBox();*/
            NewModel Edit = new NewModel("Edit", SelectedModel);
            Edit.Show();
            this.Close();
            parent.Close();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("Voulez-vous vraiment supprimer ce modèle ? (Vous ne pouvez plus le récupérer après avoir été supprimé)", "Confirmation", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    SelectedModel.RemoveFromModelsGuide();
                    File.Delete(SelectedModel.Content);
                    MessageBox.Show("Succesfully Deleted");
                    this.Close();
                    parent.InitListBox();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error Occured: " + ex.Message);
            }
        }
    }
}
