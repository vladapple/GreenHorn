using DocumentFormat.OpenXml.Office2013.Word;
using Microsoft.Win32;
using Spire.Pdf.Graphics;
using Spire.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Drawing;
using DocumentFormat.OpenXml.Vml;
using DocumentFormat.OpenXml.Spreadsheet;


namespace GreenHorn
{
    /// <summary>
    /// Interaction logic for Reports.xaml
    /// </summary>
    public partial class Reports : Window
    {
        List<candidate> candidatesList = new List<candidate>();
        List<position> positionsList = new List<position>();

        public Reports()
        {
            InitializeComponent();
            try
            {
                Globals.DbContext = new GreenHorneDBEntities();
                LvCandidates.ItemsSource = Globals.DbContext.candidates.ToList();
                Lv_Positions.ItemsSource = Globals.DbContext.positions.ToList();
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Fatal database error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }
        }

        private void LvCandidates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void Lv_Positions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void Btn_SaveCandidatesToFile_Click(object sender, RoutedEventArgs e)
        {
            List<string> lines = new List<string>();
            foreach (candidate can in LvCandidates.SelectedItems)
            {
                lines.Add($"Name: {can.name} *Address: {can.address} *Email: {can.email} *Phone: {can.phone} *CV ID: {can.cvfileId} *Progress: {can.status}\n --------------------------------------------------------------- *");
            }
            DateTime now = DateTime.Now;

            string text = $"Date: {now} * *REPORT FROM GREEN HORN.* *";
            foreach (string line in lines)
            {
                text += line;
            }

            PrintDialog printDialog = new PrintDialog();
            if ((bool)printDialog.ShowDialog().GetValueOrDefault())
            {
                FlowDocument flowDocument = new FlowDocument();
                foreach (string line in text.Split('*'))
                {
                    Paragraph myParagraph = new Paragraph();
                    myParagraph.Margin = new Thickness(0);
                    myParagraph.Inlines.Add(line);
                    flowDocument.Blocks.Add(myParagraph);
                }
                DocumentPaginator paginator = ((IDocumentPaginatorSource)flowDocument).DocumentPaginator;
                printDialog.PrintDocument(paginator, "REPORT FROM GREEN HORN.");
            }
        }
        /*
                private void Btn_SaveCandidatesToFile_Click(object sender, RoutedEventArgs e)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Text file (*.txt)|*.txt|All files (*.*)|*.*";
                    if (saveFileDialog.ShowDialog() != true) return; // cancelled
                    List<string> lines = new List<string>();
                    foreach (candidate can in LvCandidates.SelectedItems)
                    {
                        lines.Add($"{can.name};{can.address};{can.email};{can.phone};{can.cvfileId};{can.statusDate}");
                    }
                    try
                    {
                        File.WriteAllLines(saveFileDialog.FileName, lines); // ex IO/Sys
                        MessageBox.Show(this, "Export complete!", "Export Status", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex) when (ex is IOException || ex is SystemException)
                    {
                        MessageBox.Show(this, "Export failed\n" + ex.Message, "Export Status", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
        */
        private void Btn_SavePositionsToFile_Click(object sender, RoutedEventArgs e)
        {
            List<string> lines = new List<string>();
            foreach (position pos in Lv_Positions.SelectedItems)
            {
                lines.Add($"Position: {pos.name}  *Workbook Type: {pos.type}  *Industry ID: {pos.industryId}  *Company ID:{pos.companyId}  *Requirenments: {pos.requirements}\n --------------------------------------------------------------- *");
            }
            DateTime now = DateTime.Now;


            string text = $"Date: {now} * *REPORT FROM GREEN HORN.* *";
            foreach (string line in lines)
            {
                text += line;
            }

            PrintDialog printDialog = new PrintDialog();
            if ((bool)printDialog.ShowDialog().GetValueOrDefault())
            {
                FlowDocument flowDocument = new FlowDocument();
                foreach (string line in text.Split('*'))
                {
                    Paragraph myParagraph = new Paragraph();
                    myParagraph.Margin = new Thickness(0);
                    myParagraph.Inlines.Add(line);
                    flowDocument.Blocks.Add(myParagraph);
                }
                DocumentPaginator paginator = ((IDocumentPaginatorSource)flowDocument).DocumentPaginator;
                printDialog.PrintDocument(paginator, "REPORT FROM GREEN HORN.");
            }
        }
        /*
                private void Btn_SavePositionsToFile_Click(object sender, RoutedEventArgs e)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Pdf file (*.pdf)|*.pdf|All files (*.*)|*.*";
                    if (saveFileDialog.ShowDialog() != true) return; // cancelled
                    List<string> lines = new List<string>();
                    foreach (position pos in Lv_Positions.SelectedItems)
                    {
                        lines.Add($"{pos.name};{pos.type};{pos.industryId};{pos.companyId};{pos.requirements}");
                    }
                    try
                    {
                        File.WriteAllLines(saveFileDialog.FileName, lines); // ex IO/Sys
                        MessageBox.Show(this, "Export complete!", "Export Status", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex) when (ex is IOException || ex is SystemException)
                    {
                        MessageBox.Show(this, "Export failed\n" + ex.Message, "Export Status", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
        */

        private void Btn_Positions_Click(object sender, RoutedEventArgs e)
        {
            Btn_SavePositionsToFile.Visibility = Visibility.Visible;
            Btn_SaveCandidatesToFile.Visibility = Visibility.Hidden;
            M_Positions.Visibility = Visibility.Visible;
            M_Candidates.Visibility = Visibility.Hidden;
            Lbl_Title.Content = "Positions:";
            Lv_Positions.Visibility = Visibility.Visible;
            LvCandidates.Visibility = Visibility.Hidden;
        }

        private void Btn_Candidates_Click(object sender, RoutedEventArgs e)
        {
            Btn_SavePositionsToFile.Visibility = Visibility.Hidden;
            Btn_SaveCandidatesToFile.Visibility = Visibility.Visible;
            M_Positions.Visibility = Visibility.Hidden;
            M_Candidates.Visibility = Visibility.Visible;
            Lbl_Title.Content = "Candidates:";
            Lv_Positions.Visibility = Visibility.Hidden;
            LvCandidates.Visibility = Visibility.Visible;
        }

        private void OrderByName_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                candidatesList = Globals.DbContext.candidates.ToList();
                candidatesList = candidatesList.OrderBy(c => c.name).ToList();
                LvCandidates.ItemsSource = candidatesList;
                LvCandidates.Items.Refresh();
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void OrderByStatus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                candidatesList = Globals.DbContext.candidates.ToList();
                candidatesList = candidatesList.OrderBy(c => c.status).ToList();
                LvCandidates.ItemsSource = candidatesList;
                LvCandidates.Items.Refresh();
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void FilterByStatus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                candidatesList = Globals.DbContext.candidates.ToList();
                candidatesList = (from can in candidatesList where can.status == "hired" select can).ToList();
                LvCandidates.ItemsSource = candidatesList;
                LvCandidates.Items.Refresh();
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OrderByTitle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                positionsList = Globals.DbContext.positions.ToList();
                positionsList = positionsList.OrderBy(p => p.name).ToList();
                Lv_Positions.ItemsSource = positionsList;
                Lv_Positions.Items.Refresh();
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void OrderByType_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                positionsList = Globals.DbContext.positions.ToList();
                positionsList = positionsList.OrderBy(p => p.type).ToList();
                Lv_Positions.ItemsSource = positionsList;
                Lv_Positions.Items.Refresh();
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void OrderByIndustry_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                positionsList = Globals.DbContext.positions.ToList();
                positionsList = positionsList.OrderBy(p => p.industryId).ToList();
                Lv_Positions.ItemsSource = positionsList;
                Lv_Positions.Items.Refresh();
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
