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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SmartGenerator.Classes;

namespace SmartGenerator.Windows
{
    /// <summary>
    /// Logique d'interaction pour CheckFinal.xaml
    /// </summary>
    public partial class CheckFinal : Window
    {
        NewFile2 parent;
        string source;
        List<string> ModelsField = new List<string>();

        public CheckFinal(NewFile2 parentwindow)
        {
            InitializeComponent();
            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            this.parent = parentwindow;
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
            catch (Exception Ex)
            {
                System.Windows.Forms.MessageBox.Show("Erreur:" + Ex.Message + " Vous devez entrer un nombre valide");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.parent.GenerateContent();
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ComputeFields()
        {
            ModelsField.Clear();
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
                if (!ModelsField.Contains(WhereToCheck[1]))
                {
                    ModelsField.Add(WhereToCheck[1]);
                }
            }
        }
        

        public void LoadText(Dictionary<string, string> fields, string source)
        {
           try
            {
                this.source = source;
                FileStream fileStream = new FileStream(source, FileMode.Open);
                TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                range.Load(fileStream, DataFormats.Rtf);
                fileStream.Close();

                ComputeFields();


                for (int i = 0; i < ModelsField.Count; i++)
                {
                    string keyword = '#' + ModelsField[i] + '#';
                    string newString = fields[ModelsField[i]];
                    TextRange trange = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                    TextPointer current = trange.Start.GetInsertionPosition(LogicalDirection.Forward);
                    while (current != null)
                    {
                        string textInRun = current.GetTextInRun(LogicalDirection.Forward);
                        if (!string.IsNullOrWhiteSpace(textInRun))
                        {
                            int index = textInRun.IndexOf(keyword);
                            if (index != -1)
                            {
                                TextPointer start = current.GetPositionAtOffset(index, LogicalDirection.Forward);
                                TextPointer end = start.GetPositionAtOffset(keyword.Length, LogicalDirection.Forward);
                                TextRange selection = new TextRange(start, end);
                                bool underlined = false;

                                object fontWeight = selection.GetPropertyValue(Inline.FontWeightProperty);
                                object fontStyle = selection.GetPropertyValue(Inline.FontStyleProperty);

                                object fontUnderline = selection.GetPropertyValue(Inline.TextDecorationsProperty);
                                if(fontUnderline.Equals(TextDecorations.Underline))
                                {
                                    underlined = true;
                                }
                                

                                object fontFamily = selection.GetPropertyValue(Inline.FontFamilyProperty);
                                object fontSize = selection.GetPropertyValue(Inline.FontSizeProperty);

                                selection.Text = newString;

                                selection.ApplyPropertyValue(TextElement.FontWeightProperty, fontWeight);
                                selection.ApplyPropertyValue(TextElement.FontStyleProperty, fontStyle);

                                if(underlined)
                                    selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
                                
                                selection.ApplyPropertyValue(TextElement.FontFamilyProperty, fontFamily);
                                selection.ApplyPropertyValue(TextElement.FontSizeProperty, fontSize);
                                
                            }
                        }
                        current = current.GetNextContextPosition(LogicalDirection.Forward);
                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

    }
}
