using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace UP2.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddPartner.xaml
    /// </summary>
    public partial class AddPartner : Page
    {
        private Partners _currentPartner = new Partners();
        private static readonly Regex FIOregex = new Regex(@"^[А-ЯЁ][а-яё]+(?: [А-ЯЁ][а-яё]+)*$");
        private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        private static readonly Regex INNRegex = new Regex(@"^\d{10}$");
        private static readonly Regex PhoneNumberRegex = new Regex(@"^\+?[1-9]\d{9,14}$");
        public AddPartner(Partners selectedPartner)
        {
            InitializeComponent();
            TypeComboBox.ItemsSource = new[] { "ЗАО", "ООО", "ПАО", "ОАО" }; // Здесь укажите реальные типы
            if (selectedPartner != null)
            {
                _currentPartner = selectedPartner;
                if (_currentPartner.type.ToString() == "ЗАО")
                {
                    TypeComboBox.SelectedItem = TypeComboBox.Items[0];
                }
                if (_currentPartner.type.ToString() == "ООО")
                {
                    TypeComboBox.SelectedItem = TypeComboBox.Items[1];
                }
                if (_currentPartner.type.ToString() == "ПАО")
                {
                    TypeComboBox.SelectedItem = TypeComboBox.Items[2];
                }
                else { TypeComboBox.SelectedItem = TypeComboBox.Items[3]; }
            }
            DataContext = _currentPartner;

            DataContext = _currentPartner;
        }



        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }



        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();


            if (string.IsNullOrWhiteSpace(_currentPartner.company_name))
                errors.AppendLine("Укажите наименование!");
            if (string.IsNullOrWhiteSpace(_currentPartner.phone))
                errors.AppendLine("Укажите телефон!");
            if (string.IsNullOrWhiteSpace(_currentPartner.email))
                errors.AppendLine("Укажите почту!");
            if (string.IsNullOrWhiteSpace(_currentPartner.director_name))
                errors.AppendLine("Укажите ФИО директора!");
            if (string.IsNullOrWhiteSpace(_currentPartner.legal_address))
                errors.AppendLine("Укажите адрес");

            if (!FIOregex.IsMatch(DirectorTextBox.Text))
            {
                MessageBox.Show("ФИО директора введено неверно.", "Ошибка: Некорректное ФИО", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!PhoneNumberRegex.IsMatch(PhoneTextBox.Text))
            {
                MessageBox.Show("Номер телефона введён в неверном формате.", "Ошибка: Некорректный номер телефона", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!EmailRegex.IsMatch(EmailTextBox.Text))
            {
                MessageBox.Show("Электронная почта введена неверно.", "Ошибка: Некорректная электронная почта", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!INNRegex.IsMatch(innTextBox.Text))
            {
                MessageBox.Show("ИНН введён неверно.", "Ошибка: Некорректный ИНН", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(RatingTextBox.Text, out int ratingValue) || ratingValue < 1 || ratingValue > 10)
            {
                MessageBox.Show("Рейтинг должен быть числом от 1 до 10.", "Ошибка: Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            if (_currentPartner.ID == 0)
                Entities.GetContext().Partners.Add(_currentPartner);
            try
            {
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Данные успешно сохранены!");
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            NavigationService.GoBack();
        }
    }
}


