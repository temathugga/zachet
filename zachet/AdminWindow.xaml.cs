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

namespace zachet
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();

            roleComboBox.ItemsSource = new[] { "Администратор", "Пользователь" };
            roleComboBox.SelectedIndex = 1;

            LoadUsers();

        }

        private void LoadUsers()
        {
            using (var db = new zachet_bdEntities())
            {
                usersDataGrid.ItemsSource = db.Users.ToList();
            }
        }

        private void usersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Users user = usersDataGrid.SelectedItem as Users;

            if (user == null)
            {
                return;
            }
            loginTextBox.Text = user.Login;
            passwordBox.Password = user.Password;
            roleComboBox.SelectedItem = user.Role;
        }
        private void addButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void unblockButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
