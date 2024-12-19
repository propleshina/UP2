using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    public class PartnerWithDiscount
    {
        public string type { get; set; }
        public string company_name { get; set; }
        public string director_name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public decimal TotalSales { get; set; }
        public string rating { get; set; }
        public decimal Discount { get; set; }

        public void CalculateDiscount()
        {
            if (TotalSales <= 10000) { Discount = 0; }
            else if (TotalSales <= 50000) { Discount = 5; }
            else if (TotalSales <= 300000) { Discount = 10; }
            else { Discount = 15; }
        }
    }




    public partial class List_of_partners : Page
    {

        private void AddPartnerButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPartner(null));
        }




        public List_of_partners()
        {
            InitializeComponent();
            var currentUsers = Entities.GetContext().Partners.ToList();
            ListUser.ItemsSource = currentUsers;
            var skidkiPachet = Entities.GetContext().Partner_product.ToList();

            var groupedByPartner = skidkiPachet.GroupBy(x => x.Partners);
            var partners = Entities.GetContext().Partners.ToList();

            var partnerProducts = Entities.GetContext().Partner_product.ToList();

            var partnerSales = partnerProducts
                .GroupBy(pp => pp.ID)
                .Select(g => new
                {
                    PartnerId = g.Key,
                    TotalSales = g.Sum(pp => pp.quantity_of_products)
                })
                .ToList();

            var partnersWithDiscount = partners.Select(partner =>
            {

                var totalSales = partnerSales
                    .FirstOrDefault(ps => ps.PartnerId == partner.ID)?.TotalSales ?? 0;

                var partnerWithDiscount = new PartnerWithDiscount
                {
                    type = partner.type,
                    company_name = partner.company_name,
                    director_name = partner.director_name,
                    email = partner.email,
                    phone = partner.email,
                    TotalSales = totalSales,
                    rating = partner.rating.ToString(),
                };

                partnerWithDiscount.CalculateDiscount();

                return partnerWithDiscount;
            }).ToList();

            ListUser.ItemsSource = partnersWithDiscount;
            ListUser.Items.Refresh();

        }



        private void ListUser_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (ListUser.SelectedItem != null)
            {
                var selectedPartner = (PartnerWithDiscount)ListUser.SelectedItem;
                var partner = Entities.GetContext().Partners.First(p => p.company_name == selectedPartner.company_name);
                NavigationService.Navigate(new AddPartner(partner));
            }
        }

        private void Is_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                var currentUsers = Entities.GetContext().Partners.ToList();
                ListUser.ItemsSource = currentUsers;
                var skidkiPachet = Entities.GetContext().Partner_product.ToList();

                var groupedByPartner = skidkiPachet.GroupBy(x => x.Partners);
                var partners = Entities.GetContext().Partners.ToList();

                var partnerProducts = Entities.GetContext().Partner_product.ToList();

                var partnerSales = partnerProducts
                    .GroupBy(pp => pp.ID)
                    .Select(g => new
                    {
                        PartnerId = g.Key,
                        TotalSales = g.Sum(pp => pp.quantity_of_products)
                    })
                    .ToList();
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                var partnersWithDiscount = partners.Select(partner =>
                {

                    var totalSales = partnerSales
                        .FirstOrDefault(ps => ps.PartnerId == partner.ID)?.TotalSales ?? 0;

                    var partnerWithDiscount = new PartnerWithDiscount
                    {
                        type = partner.type,
                        company_name = partner.company_name,
                        director_name = partner.director_name,
                        email = partner.email,
                        phone = partner.email,
                        TotalSales = totalSales,
                        rating = partner.rating.ToString(),
                    };

                    partnerWithDiscount.CalculateDiscount();

                    return partnerWithDiscount;
                }).ToList();

                ListUser.ItemsSource = partnersWithDiscount;
                ListUser.Items.Refresh();
            }
        }
    }
}
