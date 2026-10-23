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

namespace zachet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string login = loginTextBox.Text.Trim();
                string password = passwordBox.Password;

                if (login == "" || password == "")
                {
                    MessageBox.Show("Заполните оба поля: Логин и Пароль.", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                using (var db = new zachet_bdEntities())
                {
                    Users user = db.Users.FirstOrDefault(u => u.Login == login);

                    if (user == null)
                    {
                        MessageBox.Show("Неправильный логин или пароль", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (user.IsBlocked == 1)
                    {
                        MessageBox.Show("Вы заблокированы, обратитесь к администратору", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Stop);
                        return;
                    }

                    bool passwordOk = (user.Password == password);

                    if (passwordOk)
                    {

                        user.FailedAttempts = 0;
                        db.SaveChanges();
                        MessageBox.Show("Вы успешно авторизовались!", "Успешная авторизация", MessageBoxButton.OK, MessageBoxImage.Information);

                        if (user.Role == "Администратор")
                        {
                            AdminWindow adminWindow = new AdminWindow();
                            adminWindow.Show();
                        }
                        else
                        {
                            UserWindow userWindow = new UserWindow();
                            userWindow.Show();
                        }
                        Close();
                        return;
                    }

                    user.FailedAttempts = user.FailedAttempts + 1;

                    if (user.FailedAttempts >= 3)
                    {
                        user.IsBlocked = 1;
                        db.SaveChanges();
                        MessageBox.Show("Вы заблокированы, обратитесь к администратору", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Stop);
                        return;
                    }

                    db.SaveChanges();
                    MessageBox.Show("Неправильный логин или пароль", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }




            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения к БД: " + ex.Message, "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
