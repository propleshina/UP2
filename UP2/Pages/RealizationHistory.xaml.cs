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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UP2.Pages
{
    /// <summary>
    /// Логика взаимодействия для RealizationHistory.xaml
    /// </summary>
    public partial class RealizationHistory : Page
    {
        public RealizationHistory()
        {
            InitializeComponent();
            LoadPartners();
            LoadHistory();
        }


        private void LoadPartners()
        {
            var partners = Entities.GetContext().Partners.Select(p => new
            {
                p.ID,
                p.company_name
            }).ToList();

            PartnerComboBox.ItemsSource = partners;
            PartnerComboBox.SelectedValuePath = "ID";

        }


        private void LoadHistory(int? partnerId = null)
        {
            var query = Entities.GetContext().Partner_product.AsQueryable();

            if (partnerId.HasValue)
            {
                query = query.Where(pp => pp.partner_id == partnerId.Value);
            }

            var partnerProducts = query.Select(pp => new
            {
                ProductName = pp.Product.name,
                PartnerName = pp.Partners.company_name,
                pp.quantity_of_products,
                pp.sale_date
            }).ToList();

            if (partnerProducts.Count == 0)
            {
                MessageBox.Show("Данные не найдены.");
            }
            else
            {
                HistoryDataGrid.ItemsSource = partnerProducts;
            }
        }

        private void PartnerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PartnerComboBox.SelectedValue != null)
            {
                int partnerId = (int)PartnerComboBox.SelectedValue;
                LoadHistory(partnerId);
            }
            else
            {
                LoadHistory();
            }
        }
    }
}



