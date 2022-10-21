using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GreenHorn
{
    /// <summary>
    /// Interaction logic for Applications_Dialog.xaml
    /// </summary>
    public partial class Applications_Dialog : Window
    {
        public Applications_Dialog()
        {
            InitializeComponent();
            
        }
        //private void Timer_Tick(object sender, EventArgs e)
        //{
        //    Lv_Applications.ItemsSource = Globals.DbContext.applications.ToList();
        //    FillCandidates();
        //    FillPositions();
        //}
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Globals.DbContext = new GreenHorneDBEntities();
                Lv_Applications.ItemsSource = Globals.DbContext.applications.ToList();
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
            finally
            {
                FillCandidates();
                FillPositions();
            }
        }

        public void ResetFields()
        {
            CmbBx_PositionID.SelectedValue = null;
            CmbBx_CandidateID.SelectedValue = null;
            CmbBx_Status.SelectedValue = null;
        }
        public void FillCandidates()
        {
            try
            {
                Globals.DbContext = new GreenHorneDBEntities();
                List <candidate> candidatesList = Globals.DbContext.candidates.ToList();
                
                List<string> canNames = new List<string>();
                foreach (var candidate in candidatesList)
                {
                    canNames.Add(candidate.candidateId.ToString()+ " - " + candidate.name);
                }
                CmbBx_CandidateID.ItemsSource = canNames;
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void FillPositions()
        {
            try
            {
                Globals.DbContext = new GreenHorneDBEntities();
                List<position> positionList = Globals.DbContext.positions.ToList();

                List <string> posNames = new List<string>();
                foreach (var position in positionList)
                {
                    posNames.Add(position.positionId.ToString()+" - "+position.name);
                }
                CmbBx_PositionID.ItemsSource = posNames;
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            string cbxInputCan = CmbBx_CandidateID.Text;
            string cbxInputPos= CmbBx_PositionID.Text;
            string status = CmbBx_Status.Text;
            try
            {
                if(!int.TryParse(stringCut(cbxInputCan), out int candId ))
                {
                    MessageBox.Show("Invalid Candidate Selection");
                    return;
                }
                if(!int.TryParse(stringCut(cbxInputPos), out int posId))
                {
                    MessageBox.Show("Invalid Position Selection");
                    return;
                }
                if (status == "")
                {
                    MessageBox.Show("All Fields are Required");
                    return;
                }
                Globals.DbContext.applications.Add(new application
                {
                    candidateId = candId,
                    positionId = posId,
                    status = status,
                    createdAt = DateTime.Now,
                    updatedAt = DateTime.Now,
                });
                Globals.DbContext.SaveChanges();
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Lv_Applications.ItemsSource = Globals.DbContext.applications.ToList();
                ResetFields();
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                string CbxCandId = CmbBx_CandidateID.Text;
                string CbxPosId = CmbBx_PositionID.Text;
                string status = CmbBx_Status.Text;
                if (!(Lv_Applications.SelectedItem is application curSelAp))
                {
                    return;
                }
                curSelAp.status = status;
                int.TryParse(stringCut(CbxPosId), out int posId);
                int.TryParse(stringCut(CbxCandId), out int candId);
                curSelAp.candidateId = candId;
                curSelAp.positionId = posId;
                curSelAp.updatedAt = DateTime.Now;
                Globals.DbContext.SaveChanges();
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
            {
                MessageBox.Show(ex.Message, ex.HelpLink);
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                ResetFields();
                Lv_Applications.SelectedItem = null;
                Lv_Applications.ItemsSource = Globals.DbContext.applications.ToList();
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                application curSelAp = Lv_Applications.SelectedItem as application;
                if (curSelAp != null)
                {
                    MessageBoxResult result = MessageBox.Show($"Delete Application ID# {curSelAp.applicationId} ?", "Company name", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                    switch (result)
                    {
                        case MessageBoxResult.No:
                            break;
                        case MessageBoxResult.Yes:
                            Globals.DbContext.applications.Remove(curSelAp);
                            Globals.DbContext.SaveChanges();
                            Lv_Applications.SelectedItem = null;
                            break;
                    }
                    Lv_Applications.ItemsSource = Globals.DbContext.applications.ToList();
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
        private string stringCut(string str)
        {
            string newString = "";
            foreach (char c in str)
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

        private void Lv_Applications_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BtnAdd.IsEnabled = false;
            BtnUpdate.IsEnabled = true;
            BtnDelete.IsEnabled = true;
            application curSelAp = Lv_Applications.SelectedItem as application;
            try
            {
                if((Lv_Applications.SelectedItem == null))
                {
                    BtnAdd.IsEnabled = true;
                    BtnDelete.IsEnabled = false;
                    BtnUpdate.IsEnabled = false;
                    ResetFields();
                }
                else
                {
                    CmbBx_CandidateID.Text = curSelAp.candidateId.ToString();
                    CmbBx_PositionID.Text = curSelAp.positionId.ToString();
                    CmbBx_Status.Text = curSelAp.status.ToString();
                }
            }
            catch (System.NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Btn_Refresh_Click(object sender, RoutedEventArgs e)
        {
            FillCandidates();
            FillPositions();
            Lv_Applications.ItemsSource = Globals.DbContext.applications.ToList();
        }
    }
}
