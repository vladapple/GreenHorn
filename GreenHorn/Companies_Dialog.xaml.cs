using Microsoft.Win32;
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
    /// Interaction logic for Companies_Dialog.xaml
    /// </summary>
    public partial class Companies_Dialog : Window
    {
        public Companies_Dialog()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Globals.DbContext = new GreenHorneDBEntities(); 
                LvCompanies.ItemsSource = Globals.DbContext.companies.ToList(); 
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Fatal database error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string name = Tbx_Name.Text;
            string address = Tbx_Address.Text;
            string email = Tbx_Email.Text;
            string phone = Tbx_Phone.Text;
            try
            {
                if (!Validated()) return;

                if (!(LvCompanies.SelectedItem is company curSelCom)) return;

                curSelCom.name = name;
                curSelCom.address = address;
                curSelCom.email = email;
                curSelCom.phone = phone;
                Globals.DbContext.SaveChanges();   
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                LvCompanies.ItemsSource = Globals.DbContext.companies.ToList(); // ex
                ResetFields();
                LvCompanies.SelectedItem = null;
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                company curSelCom = LvCompanies.SelectedItem as company;
                if (curSelCom != null)
                { // found the record to delete
                    MessageBoxResult result = MessageBox.Show($"Delete * {curSelCom.name} * ?", "Company name", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                    switch (result)
                    {
                        case MessageBoxResult.No:
                            break;
                        case MessageBoxResult.Yes:
                            Globals.DbContext.companies.Remove(curSelCom); //ex
                            Globals.DbContext.SaveChanges(); //ex
                            break;
                    }
                    LvCompanies.ItemsSource = Globals.DbContext.companies.ToList();
                    ResetFields();
                }
                else
                {
                    Console.WriteLine("record to delete not found");
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Validated()) return;
                string name = Tbx_Name.Text;
                string address = Tbx_Address.Text;
                string email = Tbx_Email.Text;
                string phone = Tbx_Phone.Text;
                Globals.DbContext.companies.Add(new company()
                {
                    name = name,
                    address = address,
                    email = email,
                    phone = phone
                }); //ex
                Globals.DbContext.SaveChanges(); //ex
                LvCompanies.ItemsSource = Globals.DbContext.companies.ToList(); //ex
                ResetFields();
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void LvCompanies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BtnAdd.IsEnabled = false;
            BtnDelete.IsEnabled = true;
            BtnUpdate.IsEnabled = true;
            company curSelCom = (company)LvCompanies.SelectedItem as company;
            if (curSelCom == null)
            {
                ResetFields();
                BtnAdd.IsEnabled = true;
                BtnDelete.IsEnabled = false;
                BtnUpdate.IsEnabled = false;
            }
            else
            {
                Tbx_Name.Text = curSelCom.name;
                Tbx_Address.Text = curSelCom.address;
                Tbx_Email.Text = curSelCom.email;
                Tbx_Phone.Text = curSelCom.phone;
            }
        }

        private void ResetFields()
        {
            Tbx_Name.Text = "";
            Tbx_Address.Text = "";
            Tbx_Email.Text = "";
            Tbx_Phone.Text = "";
        }

        private bool Validated()
        {
            string name = Tbx_Name.Text;
            if (!Validations.IsCompanyNameValid(name, out string error))
            {
                MessageBox.Show(this, error, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            string address = Tbx_Address.Text;
            if (!Validations.IsAddressValid(address, out string errorAddress))
            {
                MessageBox.Show(this, errorAddress, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            string email = Tbx_Email.Text;
            if (!Validations.IsEmailValid(email, out string errorEmail))
            {
                MessageBox.Show(this, errorEmail, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            string phone = Tbx_Phone.Text;
            if (!Validations.IsPhoneValid(phone, out string errorPhone))
            {
                MessageBox.Show(this, errorPhone, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        
        }
    }
}
