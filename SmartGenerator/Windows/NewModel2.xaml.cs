using SmartGenerator.Classes;
using SmartGenerator.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SmartGenerator
{
    /// <summary>
    /// Logique d'interaction pour NewModel2.xaml
    /// </summary>
    public partial class NewModel2 : Window
    {
        Models CurrentModel;
        string Mode;
        string OldContent;
        List<string> TempFields = new List<string>();
        static List<string> FinalFields = new List<string>();

        public NewModel2(string Nom, string argMode="New", Models selectedToEditedModel=null)
        {
            this.Mode = argMode;
            InitializeComponent();
            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            if(this.Mode == "New" && selectedToEditedModel == null)
            {
                CurrentModel = new Models(999, Nom);
            }
            else if(this.Mode != "New" && selectedToEditedModel != null)
            {
                OldContent = selectedToEditedModel.Content;
                selectedToEditedModel.Name = Nom;
                CurrentModel = selectedToEditedModel;
                LoadText();
            }
            object temp = rtbEditor.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            cmbFontFamily.SelectedItem = temp;
            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontSizeProperty);
            cmbFontSize.Text = temp.ToString();
        }

        private void LoadText()
        {
            if (this.Mode != "New")
            {
                try
                {
                    FileStream fileStream = new FileStream(OldContent, FileMode.Open);
                    TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                    range.Load(fileStream, System.Windows.DataFormats.Rtf);
                    fileStream.Close();
                    ComputeFields();
                }
                catch (Exception)
                {
                    System.Windows.Forms.MessageBox.Show("Le programme n'arrive pas à trouver le fichier: " + CurrentModel.Content + ". Assurez-vous que vous ne l'avez pas supprimé ou déplacé.", "Erreur de chargement");
                }
            }
            else
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
                {
                    Filter = "Rich Text Format (*.rtf)|*.rtf|Tout les fichiers (*.*)|*.*"
                };
                if (dlg.ShowDialog() == true)
                {
                    try // comment lire d'un fichier RTF (un stream et un dataformat)
                    {
                        FileStream fileStream = new FileStream(dlg.FileName, FileMode.Open);
                        TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                        range.Load(fileStream, System.Windows.DataFormats.Rtf);
                        fileStream.Close();
                        ComputeFields();
                    }
                    catch
                    {
                        System.Windows.Forms.MessageBox.Show("Vous ne pouvez charger que des fichiers de type RTF (.rtf)", "Erreur de chargement");
                    }
                }
            }
        }

        private void MenuItem_KeyUp(object sender, RoutedEventArgs e)
        {
            ComputeFields();
            AddField AddWindow = new AddField(this, FinalFields);
            AddWindow.Show();
        }

        public void InsertField(string fieldName)
        {
            TextRange tempRange = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Selection.Start);
            rtbEditor.Selection.Text = '#' + fieldName + '#';
            ComputeFields();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TextRange Range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
            if (!string.IsNullOrEmpty(Range.Text))
            {
                ComputeFields();
                OpenSaveDialog();
            }
            else
            {
                System.Windows.MessageBox.Show("Error: vous devez obligatoirement remplir le champs du contenu de votre modèle.");
            }
        }

        public void ComputeFields()
        {
            FinalFields.Clear();
            TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
            string[] Lines = range.Text.Split('\n');
            for (int l = 0; l < Lines.Count(); l++)
            {
                if (Lines[l].Contains('#'))
                {
                    if (Lines[l].Contains(' '))
                    {
                        string[] Words = Lines[l].Split(' ');
                        for (int w = 0; w < Words.Length; w++)
                        {
                            if (Words[w].Contains('#'))
                            {
                                string[] fieldname = Words[w].Split('#');
                                addfield(fieldname);
                            }
                        }
                    }
                    else
                    {
                        string[] fieldname = Lines[l].Split('#');
                        addfield(fieldname);
                    }
                }
            }
        }

        private void addfield(string[] WhereToCheck)
        {
            if (WhereToCheck.Length > 2 && !string.IsNullOrEmpty(WhereToCheck[1]))
            {
                if (!FinalFields.Contains(WhereToCheck[1]))
                {
                    FinalFields.Add(WhereToCheck[1]);
                }
            }
        }

        private void OpenSaveDialog()
        {
            try
            {
                if(this.Mode != "New")
                {
                    File.Delete(OldContent);
                    CurrentModel.ModelImplementations.Clear();
                    /*ComputeFields();
                    string StartPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                    string EndingPath_Temporary = @"\Resources\src\Generated\"+this.CurrentModel.Name+".rtf";
                    CurrentModel.Content = StartPath + EndingPath_Temporary;
                    
                    for (int i = 0; i < FinalFields.Count; i++)
                    {
                        CurrentModel.AddToModel(FinalFields[i]);
                    }
                    FileStream fileStream = new FileStream(CurrentModel.Content, FileMode.CreateNew);
                    TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                    range.Save(fileStream, System.Windows.DataFormats.Rtf);
                    fileStream.Close();
                    
                    System.Windows.MessageBox.Show("Modèle Modifié avec Succès; ID:" + CurrentModel.ModelID);*/
                }
                ComputeFields();
                string StartPath = Directory.GetCurrentDirectory(); //Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                string EndingPath_Temporary = @"\Resources\src\Generated\" + this.CurrentModel.Name + ".rtf";
                CurrentModel.Content = StartPath + EndingPath_Temporary;
                for (int i = 0; i < FinalFields.Count; i++)
                {
                    CurrentModel.AddToModel(FinalFields[i]);
                }
                FileStream fileStream = new FileStream(CurrentModel.Content, FileMode.CreateNew);
                TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                range.Save(fileStream, System.Windows.DataFormats.Rtf);
                fileStream.Close();
                if(this.Mode == "New")
                {
                    CurrentModel.AddToModelsguide();
                    System.Windows.MessageBox.Show("Modèle ajoutée avec Succès; ID:" + CurrentModel.ModelID);
                }
                else
                {
                    CurrentModel.EditFromModelsGuide();
                    System.Windows.MessageBox.Show("Modèle Modifié avec Succès; ID:" + CurrentModel.ModelID , "Smart Generator");
                }
                MainMenu menu = new MainMenu();
                menu.Show();
                this.Close();
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la sauvegarde du fichier" + CurrentModel.Content + "\rError:" + ex.Message , "Erreur de Sauvegarde");
            }
        }
        
        private void RtbEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                object temp = rtbEditor.Selection.GetPropertyValue(Inline.FontWeightProperty);
                btnBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));
                temp = rtbEditor.Selection.GetPropertyValue(Inline.FontStyleProperty);
                btnItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));
                temp = rtbEditor.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
                btnUnderline.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

                temp = rtbEditor.Selection.GetPropertyValue(Inline.FontFamilyProperty);
                cmbFontFamily.SelectedItem = temp;
                temp = rtbEditor.Selection.GetPropertyValue(Inline.FontSizeProperty);
                cmbFontSize.Text = temp.ToString();
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Une erreur est survenu, veuillez refaire l'opération", "Erreur");

            }
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            LoadText();
        }

        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontFamily.SelectedItem != null) 
                rtbEditor.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFontFamily.SelectedItem);
        }

        private void cmbFontSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                rtbEditor.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cmbFontSize.Text);
            }
            catch(Exception Ex)
            {
                System.Windows.Forms.MessageBox.Show("Erreur:" + Ex.Message + " Vous devez entrer un nombre valide");
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}