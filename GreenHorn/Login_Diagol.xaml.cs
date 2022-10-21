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

namespace GreenHorn
{
    /// <summary>
    /// Interaction logic for Login_Diagol.xaml
    /// </summary>
    public partial class Login_Diagol : Window
    {
        List<user> users = new List<user>();
        int wrongPassword = 0;
        public Login_Diagol()
        {
            InitializeComponent();
            Globals.DbContext = new GreenHorneDBEntities();
            users = Globals.DbContext.users.ToList();
            ((MainWindow)System.Windows.Application.Current.MainWindow).Hide();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string username = TxtBx_Username.Text;
            string password = TxtBx_Password.Password.ToString();
            Boolean found = false;
            foreach(user u in users)
            {
                if(username == u.username && password == u.password)
                {
                    found = true;
                    break;
                }
            }
            if (found)
            {
                ((MainWindow)System.Windows.Application.Current.MainWindow).Btn_Companies.IsEnabled = true;
                ((MainWindow)System.Windows.Application.Current.MainWindow).Btn_Candidates.IsEnabled = true;
                ((MainWindow)System.Windows.Application.Current.MainWindow).Btn_Positions.IsEnabled = true;
                ((MainWindow)System.Windows.Application.Current.MainWindow).Btn_Applications.IsEnabled = true;
                ((MainWindow)System.Windows.Application.Current.MainWindow).Btn_Statistics.IsEnabled = true;
                ((MainWindow)System.Windows.Application.Current.MainWindow).Btn_Reports.IsEnabled = true;
                ((MainWindow)System.Windows.Application.Current.MainWindow).Btn_Login.Visibility = Visibility.Hidden;
                ((MainWindow)System.Windows.Application.Current.MainWindow).Lbl_Username.Content = "Logged:  " + username;
                Close();
                ((MainWindow)System.Windows.Application.Current.MainWindow).Show();
            }
            else
            {
                MessageBox.Show("wrong username or password!", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                TxtBx_Username.Text = "";
                TxtBx_Password.Password = "";
                wrongPassword++;
                if (wrongPassword == 3)
                {
                    Close();
                    ((MainWindow)System.Windows.Application.Current.MainWindow).Show();
                }
            } 
        }
    }
}
