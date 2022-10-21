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
using System.IO;
using Path = System.IO.Path;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace GreenHorn
{
    /// <summary>
    /// User can create, read, update and delete Candidates from database
    /// </summary>
    public partial class Candidates_Dialog : Window
    {
        
        public Candidates_Dialog()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Globals.DbContext = new GreenHorneDBEntities();
                LvCandidates.ItemsSource = Globals.DbContext.candidates.ToList();
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Fatal database error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Validated()) return;
                candidate curSelCan = LvCandidates.SelectedItem as candidate;
                if (curSelCan == null) return;
                string name = Tbx_Name.Text;
                string address = Tbx_Address.Text;
                string email = Tbx_Email.Text;
                string phone = Tbx_Phone.Text;
                int.TryParse(Txb_CVFileId.Text, out int cvfileId); 
                ComboBoxItem cbx = (ComboBoxItem)CbxStatus.SelectedItem;
                DateTime date = (DateTime)DueDate.SelectedDate;

                curSelCan.name = name;
                curSelCan.address = address;
                curSelCan.email = email;
                curSelCan.phone = phone;
                curSelCan.status = cbx.Content.ToString();
                curSelCan.statusDate = date;
                curSelCan.cvfileId = cvfileId;

                Globals.DbContext.SaveChanges(); // ex
                LvCandidates.ItemsSource = Globals.DbContext.candidates.ToList(); // ex    
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(this, ex.Message, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            ResetFields();
            LvCandidates.SelectedItem = null;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                candidate curSelCan = LvCandidates.SelectedItem as candidate;
                if (curSelCan != null)
                { // found the record to delete
                    MessageBoxResult result = MessageBox.Show($"Delete * {curSelCan.name} * ?", " Name", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                    switch (result)
                    {
                        case MessageBoxResult.No:
                            break;
                        case MessageBoxResult.Yes:
                            Globals.DbContext.candidates.Remove(curSelCan); //ex
                            Globals.DbContext.SaveChanges(); //ex
                            break;
                    }
                    LvCandidates.ItemsSource = Globals.DbContext.candidates.ToList();
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
                int.TryParse(Txb_CVFileId.Text, out int cvfileId);
                ComboBoxItem cbx = (ComboBoxItem)CbxStatus.SelectedItem;
                DateTime date = (DateTime)DueDate.SelectedDate;
                Globals.DbContext.candidates.Add(new candidate()
                {
                    name = name,
                    address = address,
                    email = email,
                    phone = phone,
                    status = cbx.Content.ToString(),
                    statusDate = date,
                    cvfileId = cvfileId
                }); //ex
                Globals.DbContext.SaveChanges(); //ex
                LvCandidates.ItemsSource = Globals.DbContext.candidates.ToList(); //ex
                BtnAdd.IsEnabled = false;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(this, ex.Message, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
            ResetFields();
            
        }


        private void LvCandidates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BtnAdd.IsEnabled = false;
            BtnDelete.IsEnabled = true;
            BtnUpdate.IsEnabled = true;
            candidate curSelCan = (candidate)LvCandidates.SelectedItem as candidate;
            if (curSelCan == null)
            {
                ResetFields();
                BtnAdd.IsEnabled = false;
                BtnDelete.IsEnabled = false;
                BtnUpdate.IsEnabled = false;
            }
            else
            {
                Tbx_Name.Text = curSelCan.name;
                Tbx_Address.Text = curSelCan.address;
                Tbx_Email.Text = curSelCan.email;
                Tbx_Phone.Text = curSelCan.phone;
                CbxStatus.Text = curSelCan.status;
                Txb_CVFileId.Text = curSelCan.cvfileId.ToString();
                DueDate.SelectedDate = curSelCan.statusDate;
                Lbl_Uploaded.Content = "Double click to fetch CV";
            }
        }

        private void CbxStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbxStatus.SelectedItem != null)
            {
                ComboBoxItem cbx = (ComboBoxItem)CbxStatus.SelectedItem;

            }
        }

        private void FileDrop_Drop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                    string filename = Path.GetFileName(files[0]);
                    byte[] bytes = File.ReadAllBytes(files[0]);
                    string ext = Path.GetExtension(filename);
                    if(ext != ".pdf")
                    {
                        MessageBox.Show("Please use PDF format files only!", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
                    Globals.DbContext.cvfiles.Add(new cvfile()
                    {
                        fileData = bytes,
                    }); //ex
                    Globals.DbContext.SaveChanges();

                    List<cvfile> listOfFiles = new List<cvfile>();
                    listOfFiles = Globals.DbContext.cvfiles.ToList();
                    listOfFiles = listOfFiles.OrderByDescending(f => f.cvfileId).ToList();
                    Lbl_Uploaded.Content = filename;
                    Txb_CVFileId.Text = listOfFiles[0].cvfileId.ToString();
                    BtnAdd.IsEnabled = true;
                }
            }
            catch(ArgumentNullException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LblFetchCV_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<cvfile> listOfCVFiles = new List<cvfile>();
                listOfCVFiles = Globals.DbContext.cvfiles.ToList();
                string cvfileId = Txb_CVFileId.Text;
                byte[] bytes;
                foreach (cvfile cvfile in listOfCVFiles)
                {
                    if (cvfile.cvfileId.ToString() == cvfileId)
                    {
                        bytes = cvfile.fileData;
                        string outputFolder = @"C:\temp\cvfileId_" + cvfileId + ".pdf";
                        File.WriteAllBytes(outputFolder, bytes);
                        wb.Navigate(outputFolder);
                        break;
                    }
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void ResetFields()
        {
            Tbx_Name.Text = "";
            Tbx_Address.Text = "";
            Tbx_Email.Text = "";
            Tbx_Phone.Text = "";
            CbxItem.IsSelected = true;
            DueDate.SelectedDate = DateTime.Today;
            Lbl_Uploaded.Content = "";
            Txb_CVFileId.Text = "";
        }

        private bool Validated()
        {
            string name = Tbx_Name.Text;
            if (!Validations.IsNameValid(name, out string error))
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
            DateTime date = (DateTime)DueDate.SelectedDate;
            if (!Validations.IsYearValid(date, out string errorYear))
            {
                MessageBox.Show(this, errorYear, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;

        }
    }
}
