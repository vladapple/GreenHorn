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

namespace GreenHorn
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            MessageBox.Show("Thanks for using Green Horn app!");
        }

        private void TxtBx_Username_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TxtBx_Password_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Btn_Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Login_Diagol myLogin = new Login_Diagol();
                myLogin.Show();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "We must add additional validation (window_loaded error, MainWindow.xaml.cs line 27");
            }
        }

        private void Btn_Companies_Click(object sender, RoutedEventArgs e)
        {
            Companies_Dialog company = new Companies_Dialog();
            try
            {
                if (company.IsActive)
                {
                    MessageBox.Show("This window is already open.");
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

            finally
            {
                company.ShowDialog();
            }
        }

        private void Btn_Candidates_Click(object sender, RoutedEventArgs e)
        {
            Candidates_Dialog candidate = new Candidates_Dialog();
            try
            {
                if (candidate.IsActive)
                {
                    MessageBox.Show("This window is already open.");
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
           
            finally
            {   
                candidate.ShowDialog();
            }
        }

        private void Btn_Positions_Click(object sender, RoutedEventArgs e)
        {
            Add_Position_Dialog positions = new Add_Position_Dialog();
            try
            {
                if (positions.IsActive)
                {
                    MessageBox.Show("This window is already open.");
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                positions.ShowDialog();
            }
        }

        private void Btn_Applications_Click(object sender, RoutedEventArgs e)
        {
            Applications_Dialog applications = new Applications_Dialog();
            try
            {
                if (applications.IsActive)
                {
                    MessageBox.Show("This window is already open.");
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                applications.ShowDialog();

            }
        }

        private void Btn_Statistics_Click(object sender, RoutedEventArgs e)
        {
            Statistics statistics = new Statistics();
            try
            {
                if (statistics.IsActive)
                {
                    MessageBox.Show("This window is already open.");
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                statistics.ShowDialog();

            }
        }
        private void Btn_Reports_Click(object sender, RoutedEventArgs e)
        {
            Reports reports = new Reports();
            try
            {
                if (reports.IsActive)
                {
                    MessageBox.Show("This window is already open.");
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                reports.ShowDialog();
            }
        }
    }
}
