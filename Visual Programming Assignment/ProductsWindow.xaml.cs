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
        const string kButtonSaveText = "Save changes";
        const string kButtonCreateText = "Add entry";

        public ProductsWindow()
        {
            InitializeComponent();
            db = new StoreDBEntities();
            setProducts(); 
        }

        /// <summary>
        /// Get the required product from the database
        /// </summary>
        /// <returns></returns>
        private Product getProduct(string productName)
        {
            var result = from prod in db.Products
                         where prod.Name == productName
                         select prod;

            return result.SingleOrDefault();
        }

        /// <summary>
        /// Method to get all products currently in the products table
        /// </summary>
        /// <returns></returns>
        private List<Product> getAllProducts()
        {
            var result = from prod in db.Products
                         select prod;

            return result.ToList();
        }

        /// <summary>
        /// Method to set the product names in the ListBox 
        /// </summary>
        private void setProducts()
        {
            //List<string> productNames = new List<string>();
            List<Product> products = getAllProducts();

            //foreach (var product in products)
            // {
            //    productNames.Add(product.Name);
            //}

            //this.productsListBox.ItemsSource = productNames;
            this.productsListBox.ItemsSource = products;

        }

        private void productsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Set button content
            this.updateButton.Content = kButtonSaveText;

            // Get the product from the event            
            string productName = (string)this.productsListBox.SelectedItem;
            

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
            // Check for item selection
            if (selectedProduct != null)
            {
                // Show confirmation dialog
                MessageBoxResult selectionChoice = MessageBox.Show("Are you sure you want to delete the selected item?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

                // Remove item from the DB if required
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

            // Update the button content
            this.updateButton.Content = kButtonCreateText;
        }

        /// <summary>
        /// Method to clear out the product fields
        /// </summary>
        private void clearFields()
        {
            // Clear the fields
            this.productNameTextbox.Text = "";
            this.productCategoryTextbox.Text = "";
            this.productPriceTextbox.Text = "";
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if product selected then update the product
            if (this.selectedProduct != null)
            {
                // Get the product reference
                Product productToBeUpdated = getProduct(this.selectedProduct.Name);

                // Update the product
                productToBeUpdated.Name = this.productNameTextbox.Text;
                productToBeUpdated.Category = this.productCategoryTextbox.Text;
                productToBeUpdated.Price = int.Parse(this.productPriceTextbox.Text);

                

            }
            // Create new product entry
            else
            {
                Product newProduct = new Product()
                {
                    Name = this.productNameTextbox.Text,
                    Category = this.productCategoryTextbox.Text,
                    Price = int.Parse(this.productPriceTextbox.Text)
                };


            }

            // Sync changes to DB
            db.SaveChanges();

            // Refresh the ListBox
            setProducts();
        }
    }
}
