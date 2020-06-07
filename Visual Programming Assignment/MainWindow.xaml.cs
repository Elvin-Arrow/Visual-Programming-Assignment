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

namespace Visual_Programming_Assignment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        StoreDBEntities db;

        public MainWindow()
        {
            InitializeComponent();
            db = new StoreDBEntities();

        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the username and password
            User sessionUser = new User() 
            { 
                username = this.usernameTextBox.Text,
                password = this.passwordBox.Password
            };

            // Authenticate the user credentials against the DB
            var result = from user in db.Users
                         where user.username == sessionUser.username && user.password == sessionUser.password
                         select user;

            // Fetch the results
            User authenticatedUser = result.SingleOrDefault();

            if (authenticatedUser != null)
            {
                ProductsWindow products = new ProductsWindow();
                products.Show();
                this.Close();
            } else
            {
                MessageBox.Show("Invalid user credentials, please enter valid user credentails and try again", "Authentication Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
