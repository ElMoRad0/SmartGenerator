﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SmartGenerator.Classes;
using SmartGenerator.Source;
 

namespace SmartGenerator
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            User User = Treatments.CreateSession();
            if (!string.IsNullOrEmpty(PassBox.Password))
            {
                bool CorrectCredentials = Treatments.CheckCredentials(PassBox.Password, User);
                if (CorrectCredentials)
                {
                    MainMenu Menu = new MainMenu();
                    Menu.Show();
                    this.Close();
                }
                else
                {
                    ErrorTBlck.Text = "Error: Wrong Password";
                }
            }
            else
            {
                ErrorTBlck.Text = "Error: No Password typed.";
            }
            
        }
    }
}
