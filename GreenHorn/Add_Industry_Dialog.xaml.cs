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
    /// Interaction logic for Add_Industry_Dialog.xaml
    /// </summary>
    public partial class Add_Industry_Dialog : Window
    {
        public Add_Industry_Dialog()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Globals.DbContext = new GreenHorneDBEntities();
                Lv_Industry.ItemsSource = Globals.DbContext.industries.ToList();
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Fatal database error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }
        }

        private void Btn_AddIndustry_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Validated())
                {
                    return;
                }
                string name = Txtbx_Industy_Name.Text;
                Globals.DbContext.industries.Add(new industry()
                {
                    name = name,
                });
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Globals.DbContext.SaveChanges();
                Lv_Industry.ItemsSource = Globals.DbContext.industries.ToList();
                ResetFields();

            }
        }
        private void ResetFields()
        {
            Txtbx_Industy_Name.Text = "";
        }
        private bool Validated()
        {
            return true; //TODO ADD VALIDATION
        }

        private void Btn_UpdateIndustry_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Validated())
                {
                    return;
                }
                industry curSelInd = Lv_Industry.SelectedItem as industry;
                if (curSelInd == null)
                {
                    return;
                }
                string name = Txtbx_Industy_Name.Text;
                curSelInd.name = name;

                Globals.DbContext.SaveChanges();
                Lv_Industry.ItemsSource = Globals.DbContext.industries.ToList();
                ResetFields();
                Lv_Industry.SelectedItem = null;
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Btn_DeleteIndustry_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Lv_Industry.SelectedItem is industry curSelInd)
                {
                    MessageBoxResult result = MessageBox.Show($"Delete * {curSelInd.name} * ?", "Industry", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                    switch (result)
                    {
                        case MessageBoxResult.No:
                            break;
                        case MessageBoxResult.Yes:
                            Globals.DbContext.industries.Remove(curSelInd); //ex
                            Globals.DbContext.SaveChanges(); //ex
                            break;
                    }
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
            finally
            {
                Lv_Industry.ItemsSource = Globals.DbContext.industries.ToList();
                ResetFields();
                Lv_Industry.SelectedItem = null;
            }
        }

        private void Lv_Industry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Btn_AddIndustry.IsEnabled = false;
                Btn_UpdateIndustry.IsEnabled = true;
                Btn_DeleteIndustry.IsEnabled = true;
                if (Lv_Industry.SelectedItem is industry curSelInd)
                {
                    Txtbx_Industy_Name.Text = curSelInd.name;
                }
                else
                {
                    ResetFields();
                    Btn_AddIndustry.IsEnabled = true; //TODO ADD NEW AFTER UPDATING ANOTHER
                    //UNSELECT SOMETHING HOW? MAYBE A CLEAR BUTTON AND CAN HELP WITH ^^^
                    Btn_DeleteIndustry.IsEnabled = false;
                    Btn_UpdateIndustry.IsEnabled = false;
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
