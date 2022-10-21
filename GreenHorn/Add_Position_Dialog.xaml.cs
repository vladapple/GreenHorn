using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Threading;

namespace GreenHorn
{
    /// <summary>
    /// Interaction logic for Add_Position_Dialog.xaml
    /// </summary>
    public partial class Add_Position_Dialog : Window
    {
        public Add_Position_Dialog()
        {
            InitializeComponent();
        }
        //private void Timer_Tick(object sender, EventArgs e)
        //{
        //    FillIndustry();
        //    FillCompany();
        //    Lv_Positions.ItemsSource = Globals.DbContext.positions.ToList();
        //}
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Globals.DbContext = new GreenHorneDBEntities();
                Lv_Positions.ItemsSource = Globals.DbContext.positions.ToList();
                FillIndustry();
                FillCompany();
                //DispatcherTimer timer = new DispatcherTimer();
                //timer.Interval = TimeSpan.FromSeconds(10);
                //timer.Tick += new EventHandler(Timer_Tick);
                //timer.Start();
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Fatal database error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }
        }
        public void FillIndustry()
        {
            try
            {
                Globals.DbContext = new GreenHorneDBEntities();
                List<industry> list = Globals.DbContext.industries.ToList(); 
                List<string> names = new List<string>();
                foreach(industry ind in list)
                {
                    string indust = ind.industryId.ToString() + " - " + ind.name;
                    names.Add(indust);
                }
                CmbBx_Industry.ItemsSource = names;
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void FillCompany()
        {
            try
            {
                Globals.DbContext = new GreenHorneDBEntities();
                List<company> list = Globals.DbContext.companies.ToList();
                List<string> names = new List<string>();
                foreach (company com in list)
                {
                    names.Add(com.companyId.ToString() + " - " + com.name);
                }
                CmbBx_Company.ItemsSource = names;
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Btn_Industries_Click(object sender, RoutedEventArgs e)
        {
            Add_Industry_Dialog createIndustry = new Add_Industry_Dialog();
            try
            {
                if (createIndustry.IsActive)
                {
                    MessageBox.Show("This window is already active");
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                createIndustry.ShowDialog();
            }
        }


        private void ResetFields()
        {
            TxtBx_Name.Text = "";
            CmbBx_WorkType.Text = "";
            CmbBx_Industry.Text = "";
            CmbBx_Company.Text = "";
            TxtBx_Requirements.Text = "";

        }

        private bool Validated()
        {
            string name = TxtBx_Name.Text;
            if (!Validations.IsPositionNameValid(name, out string errorPositionName))
            {
                MessageBox.Show(this, errorPositionName, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            string positionType = CmbBx_WorkType.Text;
            if (!Validations.IsPositionTypeValid(positionType, out string errorPositionType))
            {
                MessageBox.Show(this, errorPositionType, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;

        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            string name = TxtBx_Name.Text; 
            string cbxType = CmbBx_WorkType.Text;
            string cbxIndustryId = CmbBx_Industry.Text;
            string cbxCompanyId = CmbBx_Company.Text;
            string req = TxtBx_Requirements.Text;
            
            try
            {
                int.TryParse(stringCut(cbxIndustryId), out int industryId);
                int.TryParse(stringCut(cbxCompanyId), out int companyId);
                Globals.DbContext.positions.Add(new position
                    {
                        name = name,
                        type = cbxType,
                        industryId = industryId,
                        companyId = companyId,
                        requirements = req
                    });
                Globals.DbContext.SaveChanges();
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                ResetFields();
                
                Lv_Positions.ItemsSource = Globals.DbContext.positions.ToList();
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Validated()) return;
                position curSelPos = Lv_Positions.SelectedItem as position;
                if (curSelPos == null) return;
                string name = TxtBx_Name.Text;
                string cbxType = CmbBx_WorkType.Text;
                string cbxIndustryId = CmbBx_Industry.Text;
                string cbxCompanyId = CmbBx_Company.Text;
                string req = TxtBx_Requirements.Text;

                int.TryParse(stringCut(cbxIndustryId), out int industryId);
                int.TryParse(stringCut(cbxCompanyId), out int companyId);

                curSelPos.name = name;
                curSelPos.type = cbxType;
                curSelPos.industryId = industryId;
                curSelPos.companyId = companyId;
                curSelPos.requirements = req;

                Globals.DbContext.SaveChanges(); // ex
                Lv_Positions.ItemsSource = Globals.DbContext.positions.ToList(); // ex
                ResetFields();
                Lv_Positions.SelectedItem = null;
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                position curSelPos = Lv_Positions.SelectedItem as position;
                if (curSelPos != null)
                { // found the record to delete
                    MessageBoxResult result = MessageBox.Show($"Delete * {curSelPos.name} * ?", " Name", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                    switch (result)
                    {
                        case MessageBoxResult.No:
                            break;
                        case MessageBoxResult.Yes:
                            Globals.DbContext.positions.Remove(curSelPos); //ex
                            Globals.DbContext.SaveChanges(); //ex
                            break;
                    }
                    Lv_Positions.ItemsSource = Globals.DbContext.positions.ToList();
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

        private void Lv_Positions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
                BtnAdd.IsEnabled = false;
                BtnUpdate.IsEnabled = true;
                BtnDelete.IsEnabled = true;
                position curSelPos = (position)Lv_Positions.SelectedItem as position;
                if (curSelPos == null)
                {
                    ResetFields();
                    BtnAdd.IsEnabled = true;
                    BtnDelete.IsEnabled = false;
                    BtnUpdate.IsEnabled = false;
                }
                else
                {
                    TxtBx_Name.Text = curSelPos.name;
                    CmbBx_WorkType.Text = curSelPos.type;
                    CmbBx_Industry.Text = curSelPos.industryId.ToString();
                    CmbBx_Company.Text = curSelPos.companyId.ToString();
                    TxtBx_Requirements.Text = curSelPos.requirements;
                }
        }
        private string stringCut(string str)
        {
            string newString = "";
            foreach(char c in str)
            {
                if (c != ' ')
                {
                    newString += c;
                }
                else
                {
                    return newString;
                }
            }
            return newString;
        }

        private void Btn_Refresh_Click(object sender, RoutedEventArgs e)
        {
            Lv_Positions.ItemsSource = Globals.DbContext.positions.ToList();
            FillIndustry();
            FillCompany();
        }
    }
}
