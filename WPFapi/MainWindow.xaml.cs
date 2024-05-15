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

namespace WPFapi
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        User selectedUser = new User();
        public MainWindow()
        {
            InitializeComponent();
            InitializeRoles();
            Refresh();
        }

        private void InitializeRoles()
        {
            List<string> roles = new List<string> { "Работник специального подразделения", "Агент ФБР", "Кибербезопасник", "Секретный агент", "Секретарь" };
            RoleCB.ItemsSource = roles;
        }

        private async void Refresh()
        {
            var users = await NetManager.Get<List<User>>("api/Users/GetAllUsers");
            UsersLV.ItemsSource = users;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private async void AddBTN_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTB.Text) || string.IsNullOrWhiteSpace(AgeTB.Text) || RoleCB.SelectedItem == null)
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }
            else
            {
                var user = new User();
                user.Name = NameTB.Text.Trim();
                user.Age = int.Parse(AgeTB.Text.Trim());
                user.IDRole = RoleCB.SelectedIndex + 1;
                await NetManager.Post("api/Users/Add", user);
                MessageBox.Show("Пользователь добавлен!");
                Refresh();
                NameTB.Text = String.Empty;
                AgeTB.Text = String.Empty;
                RoleCB.SelectedIndex = -1;
            }
        }

        private void ChangeBTN_Click(object sender, RoutedEventArgs e)
        {
            if (UsersLV.SelectedItem == null)
            {
                MessageBox.Show("Выберите пользователя для изменения!");
                return;
            }
            else
            {
                selectedUser.Name = NameTB.Text;
                selectedUser.Age = int.Parse(AgeTB.Text);
                selectedUser.IDRole = RoleCB.SelectedIndex + 1;
                _ = NetManager.Put($"api/Users/Edit/{selectedUser.IDUser}", selectedUser);
                MessageBox.Show("Данные пользователя изменены");
                Refresh();
                NameTB.Text = String.Empty;
                AgeTB.Text = String.Empty;
                RoleCB.SelectedIndex = -1;
            }
        }

        private async void DeleteBTN_Click(object sender, RoutedEventArgs e)
        {
            if (UsersLV.SelectedItem == null)
            {
                MessageBox.Show("Выберите пользователя для удаления");
                return;
            }
            else
            {
                string str = $"api/Users/Delete/{selectedUser.Name}";
                await NetManager.Delete<string>(str);
                MessageBox.Show("Пользователь удален");
                Refresh();
                NameTB.Text = String.Empty;
                AgeTB.Text = String.Empty;
                RoleCB.SelectedIndex = -1;
            }
        }

        private void UsersLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedUser = UsersLV.SelectedItem as User;
            if (selectedUser != null)
            {
                NameTB.Text = selectedUser.Name;
                AgeTB.Text = Convert.ToString(selectedUser.Age);
                RoleCB.Text = selectedUser.RoleName;
            }
        }
    }
}
