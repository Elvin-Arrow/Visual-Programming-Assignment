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

namespace Visual_Programming_Assignment
{
    /// <summary>
    /// Interaction logic for ProductsWindow.xaml
    /// </summary>
    public partial class ProductsWindow : Window
    {
        StoreDBEntities db;
        Product selectedProduct;

        public ProductsWindow()
        {
            InitializeComponent();
            db = new StoreDBEntities();
            setProducts(); 
        }

        private List<Product> GetProducts()
        {
            var result = from prod in db.Products
                         select prod;

            return result.ToList();
        }

        private void setProducts()
        {
            List<string> productNames = new List<string>();
            List<Product> products = GetProducts();

            foreach (var product in products)
            {
                productNames.Add(product.Name);
            }

            this.productsListBox.ItemsSource = productNames;

        }

        private void productsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            // Get the product from the event
            
            string productName = (string)e.AddedItems[0];

            // Find the product from the DB
            var result = from prod in db.Products
                         where prod.Name == productName
                         select prod;

            Product product = result.SingleOrDefault();
            this.selectedProduct = product;

            // Display the product info
            this.productNameTextbox.Text = product.Name;
            this.productCategoryTextbox.Text = product.Category;
            this.productPriceTextbox.Text = product.Price.ToString();

        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedProduct != null)
            {
                MessageBoxResult selectionChoice = MessageBox.Show("Are you sure you want to delete the selected item?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

                if (selectionChoice == MessageBoxResult.Yes)
                {
                    clearFields();
                    this.selectedProduct = null;
                }
            }
            else
            {
                MessageBox.Show("No item selected", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            
           
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear the fields
            clearFields();
        }

        private void clearFields()
        {
            // Clear the fields
            this.productNameTextbox.Text = "";
            this.productCategoryTextbox.Text = "";
            this.productPriceTextbox.Text = "";
        }
    }
}
