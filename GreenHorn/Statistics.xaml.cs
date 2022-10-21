using DocumentFormat.OpenXml.Office2010.PowerPoint;
using DocumentFormat.OpenXml.Spreadsheet;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
using System.Windows.Threading;
using static Dropbox.Api.TeamPolicies.SmartSyncPolicy;

namespace GreenHorn
{
    /// <summary>
    /// Interaction logic for Statistics.xaml
    /// </summary>
    public partial class Statistics : Window
    {
        public Statistics()
        {
            InitializeComponent();
        }
        public SeriesCollection SeriesCollection { get; set; }

        private void PieChart_DataClick(object sender, ChartPoint chartPoint)
        {
            MessageBox.Show("Current value: " + chartPoint.Y + "(" + (chartPoint.Participation * 100).ToString() + " %)");
        }

        private void Btn_Candidates_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataContext = null;
                Lbl_Title.Content = "Candidates statistics:";
                ComboBoxItem cbx = (ComboBoxItem)Cbx_Month.SelectedItem;
                string monthStr = cbx.Content.ToString();
                int.TryParse(monthStr, out int month);

                Globals.DbContext = new GreenHorneDBEntities();

                List<candidate> candidateList = new List<candidate>();
                candidateList = Globals.DbContext.candidates.ToList();

                int applied = 0;
                int interviewed = 0;
                int rejected = 0;
                int hired = 0;

                foreach (candidate can in candidateList)
                {
                    if (can.statusDate.Month == month)
                    {
                        switch (can.status)
                        {
                            case "applied":
                                applied++;
                                break;
                            case "interviewed":
                                interviewed++;
                                break;
                            case "rejected":
                                rejected++;
                                break;
                            case "hired":
                                hired++;
                                break;
                            default:
                                break;
                        }
                    }
                }
                SeriesCollection = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "applied",
                    Values = new ChartValues<ObservableValue> {new ObservableValue(applied) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "interviewed",
                    Values = new ChartValues<ObservableValue> {new ObservableValue(interviewed) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "rejected",
                    Values = new ChartValues<ObservableValue> {new ObservableValue(rejected) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "hired",
                    Values = new ChartValues<ObservableValue> {new ObservableValue(hired) },
                    DataLabels = true
                }
            };

                DataContext = this;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(this, ex.Message, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
            }   
        }

        private void Btn_Positions_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataContext = null;
                Lbl_Title.Content = "Positions in industries:";
                Globals.DbContext = new GreenHorneDBEntities();
                List<position> positionList = new List<position>();
                positionList = Globals.DbContext.positions.ToList();


                int computerScience = 0;
                int agriculture = 0;
                int finance = 0;
                int healthcare = 0;
                int education = 0;
                int entertainment = 0;
                int engineering = 0;
                int retail = 0;
                int middleManagement = 0;
                int socialMediaSpecialist = 0;


                foreach (position pos in positionList)
                {
                    switch (pos.industryId)
                    {
                        case 1:
                            computerScience++;
                            break;
                        case 3:
                            agriculture++;
                            break;
                        case 6:
                            finance++;
                            break;
                        case 7:
                            healthcare++;
                            break;
                        case 10:
                            education++;
                            break;
                        case 13:
                            entertainment++;
                            break;
                        case 14:
                            engineering++;
                            break;
                        case 15:
                            retail++;
                            break;
                        case 16:
                            middleManagement++;
                            break;
                        case 17:
                            socialMediaSpecialist++;
                            break;
                        default:
                            break;
                    }
                }
                SeriesCollection = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "CS",
                    Values = new ChartValues<ObservableValue> {new ObservableValue(computerScience) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Agr",
                    Values = new ChartValues<ObservableValue> {new ObservableValue(agriculture) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Fin",
                    Values = new ChartValues<ObservableValue> {new ObservableValue(finance) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Health",
                    Values = new ChartValues<ObservableValue> {new ObservableValue(healthcare) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Ed",
                    Values = new ChartValues<ObservableValue> {new ObservableValue(education) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Ent",
                    Values = new ChartValues<ObservableValue> {new ObservableValue(entertainment) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Eng",
                    Values = new ChartValues<ObservableValue> {new ObservableValue(engineering) },

                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Ret",
                    Values = new ChartValues<ObservableValue> {new ObservableValue(retail) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Man",
                    Values = new ChartValues<ObservableValue> {new ObservableValue(middleManagement) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "SMS",
                    Values = new ChartValues<ObservableValue> {new ObservableValue(socialMediaSpecialist) },
                    DataLabels = true
                }
            };

                DataContext = this;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(this, ex.Message, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
