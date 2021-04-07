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
using System.Windows.Controls.Primitives;
using System.IO;
using SmartGenerator.Windows;
using System.ComponentModel;

namespace SmartGenerator
{
    /// <summary>
    /// Logique d'interaction pour NewFile2.xaml
    /// </summary>
    

    public partial class NewFile2 : Window
    {
        #region Declare
        Models SelectedModel;
        UserFile CurrentFile;
        List<StackPanel> Panels = new List<StackPanel>();
        List<TextBlock> TextBlocks = new List<TextBlock>();
        List<TextBox> TextBoxes = new List<TextBox>();
        int DisplayedPanelIndex = 0;
        #endregion

        #region constructeur
        public NewFile2(Models MySelectedModel)
        {
            InitializeComponent();
            SelectedModel = MySelectedModel;
            CurrentFile = new UserFile(ArgName:SelectedModel.Name, ArgTitle: SelectedModel.Name, ArgModel:SelectedModel);
            //CurrentFile.SetImplementation();
            InitTextBloxs(SelectedModel);

        }
        #endregion

        #region Logic
        private void InitTextBloxs(Models MySelectedModel)
        {
            for (int i = 0; i < MySelectedModel.ModelImplementations.Count ; i++)
            {
                TextBlock TextBlock1 = new TextBlock
                {
                    Margin = new Thickness(120, 10, 0, 0), // Left, top, right, bottom
                    Text = MySelectedModel.ModelImplementations[i] + "   :"
                };
                TextBlocks.Add(TextBlock1);
                //////////////////////////////////////////////////
                TextBox TextBox1 = new TextBox
                {
                    TextAlignment = TextAlignment.Center,
                    Margin = new Thickness(100, 10, 0, 0),
                    Text = "",
                    Width = 150
                };
                TextBoxes.Add(TextBox1);
            }
            ComputeStack();
        }

        private void ComputeStack()
        {
            int Cpt5 = 0;
            int Count5 = 0;
            if (TextBlocks.Count <= 5)
            {
                StackPanel OnePanel = new StackPanel
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(0, 0, 0, 0),
                    Height = 270,
                    VerticalAlignment = VerticalAlignment.Top,
                    Width = 852
                };

                for (int i = 0; i < TextBlocks.Count; i++) // remplir le Panel
                {
                    OnePanel.Children.Add(TextBlocks[i]);
                    OnePanel.Children.Add(TextBoxes[i]);
                }
                Panels.Add(OnePanel);
                Finish.Opacity = 100;
                Finish.IsEnabled = true;
            }
            else
            {
                Next.Opacity = 100;
                Next.IsEnabled = true;
                Previous.Opacity = 100;
                //Previous.IsEnabled = true;
                int NbPanels = (int)(TextBlocks.Count / 5);
                for (int i = 0; i < TextBlocks.Count + 1; i++)
                {
                    if (Cpt5 == 5)
                    {
                        StackPanel Panel = new StackPanel
                        {
                            HorizontalAlignment = HorizontalAlignment.Left,
                            Margin = new Thickness(0, 0, 0, 0),
                            Height = 270,
                            VerticalAlignment = VerticalAlignment.Top,
                            Width = 852
                        };
                        for (int j = 4; j >= 0; j--)
                        {
                            Panel.Children.Add(TextBlocks[((i - j) - 1)]);
                            Panel.Children.Add(TextBoxes[((i - j) - 1)]);
                        }
                        Panels.Add(Panel);
                        Cpt5 = 0;
                        Count5++;
                    }
                    if (NbPanels == Count5)
                    {
                        if (TextBlocks.Count - i != 0)
                        {
                            StackPanel Panel2 = new StackPanel
                            {
                                HorizontalAlignment = HorizontalAlignment.Left,
                                Margin = new Thickness(0, 0, 0, 0),
                                Height = 270,
                                VerticalAlignment = VerticalAlignment.Top,
                                Width = 852
                            };
                            for (int j = i + 1; j <= TextBlocks.Count; j++)
                            {
                                //MessageBox.Show("j: " + j.ToString() + "i: " + i.ToString() + "TextBlocks Size:" + TextBlocks.Count.ToString() );
                                Panel2.Children.Add(TextBlocks[j - 1]);
                                Panel2.Children.Add(TextBoxes[j - 1]);
                            }
                            Panels.Add(Panel2);
                            break;
                        }
                    }
                    Cpt5++;
                }
            }
            DisplayedPanelIndex = 0;
            BasePanel.Children.Add(Panels[DisplayedPanelIndex]);
        }

        private void DisplayPanel(StackPanel CurrentPanel, StackPanel NewPanel, StackPanel Base)
        {
            CurrentPanel.IsEnabled = false;
            CurrentPanel.Opacity = 0;
            Base.Children.Clear();
            NewPanel.IsEnabled = true;
            NewPanel.Opacity = 100;
            Base.Children.Add(NewPanel);
        }
        #endregion

        #region MenuClick
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
        #endregion

        #region events
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            Previous.IsEnabled = true;
            if (DisplayedPanelIndex != Panels.Count - 1)
            {
                int CurrentPanelINDEX = DisplayedPanelIndex;
                int NextPanel = CurrentPanelINDEX + 1;
                DisplayPanel(Panels[CurrentPanelINDEX], Panels[NextPanel], BasePanel);
                CurrentPanelINDEX = NextPanel;
                DisplayedPanelIndex = CurrentPanelINDEX;
                if (DisplayedPanelIndex == Panels.Count - 1)
                {
                    Next.IsEnabled = false;
                }
            }
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            Next.IsEnabled = true;
            if (DisplayedPanelIndex != 0)
            {
                int CurrentPanelINDEX = DisplayedPanelIndex;
                int NextPanel = CurrentPanelINDEX - 1;
                DisplayPanel(Panels[CurrentPanelINDEX], Panels[NextPanel], BasePanel);
                CurrentPanelINDEX = NextPanel;
                DisplayedPanelIndex = CurrentPanelINDEX;
                if (DisplayedPanelIndex == 0)
                {
                    Previous.IsEnabled = false;
                }
            }
        }
        
        private void button_Click(object sender, RoutedEventArgs e)
        {
            bool FieldsAreFilled = true;
            for (int i = 0; i < TextBoxes.Count(); i++)
            {
                if (string.IsNullOrEmpty(TextBoxes[i].Text))
                {
                    TextBlocks[i].Text = TextBlocks[i].Text + "[Peut Pas être vide]";
                    FieldsAreFilled = false;
                }
            }
            
            if (FieldsAreFilled)
            {
                for (int i = 0; i < TextBoxes.Count(); i++)
                {
                    CurrentFile.FileImplementationsValue[CurrentFile.BaseModel.ModelImplementations[i]] = TextBoxes[i].Text;
                }
                GenerateContent();
            }
            else
            {
                MessageBox.Show("Tout les champs sont des champs obligatoire ! ils ne peuvent pas rester vide.");
            }
        }

        public void GenerateContent()
        {
            try
            {
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "Rich Text Format (*.rtf)|*.rtf|Tout les fichiers (*.*)|*.*"
                };
                if (dlg.ShowDialog() == true)
                {
                    MessageBox.Show("Veuillez patienter pendant que le programme genère votre fichier.", "Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                    CheckFinal checkFinalResult = new CheckFinal(this);
                    checkFinalResult.LoadText(this.CurrentFile.FileImplementationsValue, this.CurrentFile.BaseModel.Content);

                    FileStream fileStream = new FileStream(dlg.FileName, FileMode.Create);
                    TextRange range = new TextRange(checkFinalResult.rtbEditor.Document.ContentStart, checkFinalResult.rtbEditor.Document.ContentEnd);
                    range.Save(fileStream, DataFormats.Rtf);
                    fileStream.Close();
                    checkFinalResult.Close();
                    MessageBox.Show("Fichier Génerée avec succès", "Opération Terminé !");
                    MainMenu Main = new MainMenu();
                    Main.Show();
                    this.Close();

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("[SmartGenerator] Error. Cannot save file, error: " + ex.Message);
            }
        }
        private void LoadView()
        {
            try
            {
                CheckFinal checkFinalResult = new CheckFinal(this);
                checkFinalResult.LoadText(this.CurrentFile.FileImplementationsValue, this.CurrentFile.BaseModel.Content);
                checkFinalResult.Show();
            }
            catch(Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
        }

        private void ViewResult_Click(object sender, RoutedEventArgs e)
        {
            bool FieldsAreFilled = true;
            for (int i = 0; i < TextBoxes.Count(); i++)
            {
                if (string.IsNullOrEmpty(TextBoxes[i].Text))
                {
                    FieldsAreFilled = false;
                }
            }
            if (FieldsAreFilled)
            {
                for (int i = 0; i < TextBoxes.Count(); i++)
                {
                    CurrentFile.FileImplementationsValue[CurrentFile.BaseModel.ModelImplementations[i]] = TextBoxes[i].Text;
                }
                LoadView();
            }
            else
            {
                MessageBox.Show("Vous ne pouvez visualiser le resultat qu'après avoir remplis tout les champs");
            }
        }
        
        private void NewFile2_Closing(object sender, CancelEventArgs e)
        {
            /*string resultat = "";
            MessageBoxResult result = MessageBox.Show("Vous risquez de perdre les valeurs de vos champs si vous quittez. \r\n Voulez-vous vraiment quitter la page ?", "Smart Generator", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if(result == MessageBoxResult.No)
            {
                e.Cancel = true;
                resultat = ""
            }*/
        }

        #endregion
    }
}











/*public void InitScrollView(Models MySelectedModel)
        {
            
            scrollBarComputeMax();
            for (int i = 0; i < MySelectedModel.ModelImplementations.Count(); i++)
            {
                //listBox.Items.Add(MySelectedModel.ModelImplementations[i]);
                TextBlock TextBlock1 = new TextBlock();
                //TextBlock1.TextAlignment = TextAlignment.Center;
                TextBlock1.PointFromScreen(new System.Windows.Point(Xname, Y) );
                //TextBlock1.Margin = new Thickness(Xname, Y, 0, 0); // Left, top, right, bottom
                TextBlock1.Text = MySelectedModel.ModelImplementations[i] + "   :";
                ///////////////////////////////////////////////////////////////
                TextBox TextBox1 = new TextBox();
                TextBox1.Margin = new Thickness(Xvalue, Y, 0, 0);
                TextBox1.Text = "";
                TextBox1.Width = 150;
                Champs.Add(TextBox1);
                TextBlocks.Add(TextBlock1);
                if(Y>= 0 && Y<=200)
                {
                    TextBox1.Visibility = (System.Windows.Visibility)0;
                    TextBlock1.Visibility = (System.Windows.Visibility)0;
                    MessageBox.Show("I printed it");
                }
                else
                {
                    TextBox1.Visibility = (System.Windows.Visibility)100;
                    TextBlock1.Visibility = (System.Windows.Visibility)100;
                    MessageBox.Show("I didn't print it");
                }
                MyPanel.Children.Add(TextBlock1);
                MyPanel.Children.Add(TextBox1);
                Y += 20;
            }
        }
        private void scrollBar_Scroll (object sender, ScrollEventArgs e)
        {
            //MessageBox.Show("vScrollBar Value:(OnValueChanged Event) " + scrollBar.Value.ToString() );
            
            if (scrollBar.Value + scrollBar.Maximum / SelectedModel.ModelImplementations.Count() < scrollBar.Maximum)
            {
                scrollBar.Value = scrollBar.Value + scrollBar.Maximum / SelectedModel.ModelImplementations.Count();
                Y -= 20;
                for (int i = 0; i < Champs.Count; i++)
                {
                    Champs[i].Margin = new Thickness(Xvalue, Y, 0, 0);
                    TextBlocks[i].Margin = new Thickness(Xname, Y, 0, 0);
                    if (Y >= 0 && Y <= 200)
                    {
                        Champs[i].PointToScreen(new Point(15, 50));
                        Champs[i].Visibility = (System.Windows.Visibility)100;
                        Champs[i].Visibility = (System.Windows.Visibility)100;
                        //MessageBox.Show("I printed it");
                    }
                    else
                    {
                        Champs[i].Visibility = (System.Windows.Visibility)0;
                        Champs[i].Visibility = (System.Windows.Visibility)0;
                        //MessageBox.Show("I didn't print it");
                    }
                    
                    //MyPanel.Children.Add(Champs[i]);
                    //MyPanel.Children.Add(TextBlocks[i]);
                }
                
            }
        }*/
