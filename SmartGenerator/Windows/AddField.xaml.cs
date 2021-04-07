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
    /// Logique d'interaction pour AddField.xaml
    /// </summary>
    public partial class AddField : Window
    {
        List<string> FieldsName = new List<string>();
        NewModel2 parent;
        public AddField(NewModel2 Parent, List<string> list)
        {
            InitializeComponent();
            parent = Parent;
            FieldsName = list;
            InitListBox(FieldsName);
        }

        private void InitListBox(List<string> list)
        {
            FieldsListBox.ItemsSource = list;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(!string.IsNullOrEmpty(FieldName.Text))
            {
                List<string> Results = new List<string>();
                for (int i = 0; i < FieldsName.Count; i++)
                {
                    if(FieldsName[i].Contains(FieldName.Text))
                    {
                        Results.Add(FieldsName[i]);
                    }
                }
                InitListBox(Results);
            }
            else
            {
                InitListBox(FieldsName);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(FieldName.Text) && !string.IsNullOrWhiteSpace(FieldName.Text))
            {
                if (!FieldName.Text.Contains(' ') && !FieldName.Text.Contains('#'))
                {
                    parent.InsertField(FieldName.Text);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Il est préférable que le nom du champs ne doit pas contenir des espaces ou des hashtags(#).");
                }
                
            }
            else
            {
                MessageBox.Show("Vous ne pouvez pas insérer un champs vide");
            }
        }

        private void SelectField(object sender, MouseButtonEventArgs e)
        {
            if(FieldsListBox.SelectedValue != null)
            {
                FieldName.Text = "";
                FieldName.Text = FieldsListBox.SelectedValue.ToString();
                InitListBox(FieldsName);
            }
        }
    }
}
