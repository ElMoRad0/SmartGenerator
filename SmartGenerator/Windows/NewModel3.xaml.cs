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

namespace SmartGenerator
{
    /// <summary>
    /// Logique d'interaction pour NewModel3.xaml
    /// </summary>
    public partial class NewModel3 : Window
    {
        Models CurrentModel;
        string ModelName;
        string ModelContent;
        int ModelNbImplementations;

        int CurrentNum = 0;

        public NewModel3(string Name, string Contenu, int NbImplementations)
        {
            InitializeComponent();
            ModelName = Name;
            ModelContent = Contenu;
            ModelNbImplementations = NbImplementations;
            CurrentModel = new Models(999, ModelName);
            CurrentModel.Content = ModelContent;
            CurrentModel.NbImplementations = NbImplementations;
            IncreaseNum();
            InitListBox();
        }

        private void InitListBox()
        {
            ListBox.ItemsSource = CurrentModel.ModelImplementations.ToList();
        }

        private void IncreaseNum()
        {
            CurrentNum++;
            ChampsNum.Content = CurrentNum.ToString() + '/' + CurrentModel.NbImplementations.ToString();
        }

        string LastVaue = "";

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(NameTextBox.Text))
            {
                if(NameTextBox.Text != LastVaue)
                {
                    CurrentModel.AddToModel(NameTextBox.Text);
                    IncreaseNum();
                    LastVaue = NameTextBox.Text;
                    ErrTextBlock.Text = "";
                    NameTextBox.Text = "";
                    if (CurrentNum == CurrentModel.NbImplementations+1)
                    {
                        Ajouter.IsEnabled = false;
                        Finish.IsEnabled = true;
                    }
                }
                else
                {
                    ErrTextBlock.Text = "Erreur: vous ne pouvez pas entrer deux fois le même nom de champ.";
                }
                InitListBox();
            }
        }

        private void Finish_Click(object sender, RoutedEventArgs e)
        {
            int LineNb = 0;
            string StartPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName ;
            string EndingPath = @"\Resources\src\ModelsGuide.txt";
            string FullCurrentProjectPath = StartPath + EndingPath;
            if (!string.IsNullOrEmpty(CurrentModel.Name))
            {
                if (!File.Exists(FullCurrentProjectPath))
                {
                    using (StreamWriter sw = File.CreateText(FullCurrentProjectPath))
                    {
                        sw.WriteLine("0" + ';' + CurrentModel.Name + '\n');
                    }
                }
                else
                {
                    string[] line = File.ReadAllLines(FullCurrentProjectPath);
                    foreach (var l in line)
                    {
                        if (l.Length != 0)
                        {
                            LineNb++;
                        }
                    }
                    using (StreamWriter sw = File.AppendText(FullCurrentProjectPath))
                    {
                        string LineToAdd = LineNb.ToString() + ';' + CurrentModel.Name + ';' + CurrentModel.Content + ';';
                        string ListLine = "";
                        for (int i = 0; i < CurrentModel.ModelImplementations.Count; i++)
                        {
                            if(i==0)
                            {
                                ListLine = ListLine + CurrentModel.ModelImplementations[i];
                            }
                            else
                            {
                                ListLine = ListLine + ',' + CurrentModel.ModelImplementations[i];
                            }
                            
                        }
                        MessageBox.Show(ListLine);
                        string AllLine = LineToAdd + ListLine;
                        MessageBox.Show(AllLine);
                        sw.WriteLine(AllLine);
                        MessageBox.Show("Modèle ajoutée avec succès ! Last ID: " + LineNb.ToString());
                    }
                }
                CurrentModel.ModelID = LineNb;
                MainMenu Main = new MainMenu();
                Main.Show();
                Close();
            }
        }
    }
}
