using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;


namespace UP2.Pages
{
    public partial class Menu : Page
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void HistoryOfRealization_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RealizationHistory());
        }

        private void Partners_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new List_of_partners());
        }
    }
}

