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

            this.productsListBox.ItemsSource = products;
        }

        private void productsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Set button content
            this.updateButton.Content = kButtonSaveText;

            // Get the product from the event            
            Product selectedItem = (Product)this.productsListBox.SelectedItem;

            if (selectedItem != null)
            {

                // Find the product from the DB
                var result = from prod in db.Products
                             where prod.Name == selectedItem.Name
                             select prod;

                Product product = result.SingleOrDefault();
                this.selectedProduct = product;

                // Display the product info
                this.productNameTextbox.Text = product.Name;
                this.productCategoryTextbox.Text = product.Category;
                this.productPriceTextbox.Text = product.Price.ToString();
            }
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
                    // Look the product reference in DB model
                    var result = from prod in db.Products
                                 where prod.Name == this.selectedProduct.Name
                                 select prod;

                    Product productToBeRemoved = result.SingleOrDefault();

                    // Revome item from DB
                    db.Products.Remove(productToBeRemoved);

                    // Save changes
                    db.SaveChanges();

                    this.selectedProduct = null;

                    // Refresh products ListBox
                    setProducts();

                    // Show success message
                    MessageBox.Show("Product removed successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);


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

            // Remove the current selection
            this.selectedProduct = null;
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

                // Display an updation message
                MessageBox.Show("Product updated successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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

                // Add product to model
                db.Products.Add(newProduct);

                // Clear the fields
                clearFields();

                // Display an insertion message
                MessageBox.Show("Product added successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            // Sync changes to DB
            db.SaveChanges();

            // Refresh the ListBox
            setProducts();
        }

        /// <summary>
        /// Method to make the borderless window draggable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        /// <summary>
        /// Method to close the current window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseApp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }
    }
}
