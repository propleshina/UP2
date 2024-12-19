using System;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace UP2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Closing += MainWindowClosing;
        }
        private void MainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы действительно хотите выйти из приложения?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No) e.Cancel = true;
        }
        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {

            if (!(e.Content is Page page)) return;
            this.Title = $"ProjectByOgilko - {page.Title}";

            if (page is Pages.Menu)
                BackButton.Visibility = Visibility.Hidden;
            else
                BackButton.Visibility = Visibility.Visible;
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.GoBack();
        }
    }
}
