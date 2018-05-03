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
using WPFShop.ViewModels;
using WPFShop.ServiceReferenceShop;
using System.Collections.ObjectModel;
using System.Collections;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Windows.Controls.Primitives;

//using System.IO;
//using System.Data;
//using System.Reflection;
//using iTextSharp.text.pdf;
//using iTextSharp.text;
//using System.Windows.Controls.Primitives;

namespace WPFShop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            //this.SizeToContent = SizeToContent.Width;
            //this.SizeToContent = SizeToContent.Height;
            this.DataContext = new MainWindowViewModel(this);
        }

        //private void ButtonPrintPressed(object sender, RoutedEventArgs e)
        //{
        //    ExportToPdf(DataGridCustomer);
        //}

        //private void ExportToPdf(DataGrid grid)
        //{
        //    MessageBox.Show("Jebote");
        //    PdfPTable table = new PdfPTable(grid.Columns.Count);
        //    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);
        //    PdfWriter writer = PdfWriter.GetInstance(doc, new System.IO.FileStream("Test.pdf", System.IO.FileMode.Create));
        //    doc.Open();
        //    for (int j = 0; j < grid.Columns.Count; j++)
        //    {
        //        table.AddCell(new Phrase(grid.Columns[j].Header.ToString()));
        //    }
        //    table.HeaderRows = 1;
        //    IEnumerable itemsSource = grid.ItemsSource as IEnumerable;
        //    if (itemsSource != null)
        //    {
        //        foreach (var item in itemsSource)
        //        {
        //            DataGridRow row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
        //            if (row != null)
        //            {
        //                DataGridCellsPresenter presenter = FindVisualChild<DataGridCellsPresenter>(row);
        //                for (int i = 0; i < grid.Columns.Count; ++i)
        //                {
        //                    DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(i);
        //                    TextBlock txt = cell.Content as TextBlock;
        //                    if (txt != null)
        //                    {
        //                        table.AddCell(new Phrase(txt.Text));
        //                    }
        //                }
        //            }
        //        }

        //        doc.Add(table);
        //        doc.Close();
        //    }
        //}

        //private T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        //{
        //    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
        //    {
        //        DependencyObject child = VisualTreeHelper.GetChild(obj, i);
        //        if (child != null && child is T)
        //            return (T)child;
        //        else
        //        {
        //            T childOfChild = FindVisualChild<T>(child);
        //            if (childOfChild != null)
        //                return childOfChild;
        //        }
        //    }
        //    return null;
        //}

        //public MainWindow(vwOffice officeForEdit)
        //{
        //    InitializeComponent();
        //    this.DataContext = new MainWindowViewModel(this, officeForEdit, null);
        //}

        //private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        //{
        //    var input = txtSearch.Text;
        //    this.DataContext = new MainWindowViewModel(this, input);
            
        //}

        //private void ButtonPrintPressed(object sender, RoutedEventArgs e)
        //{
           
        //    //Creating iTextSharp Table from the DataTable data
        //    PdfPTable pdfTable = new PdfPTable(dataGridView1.ColumnCount);
        //    pdfTable.DefaultCell.Padding = 3;
        //    pdfTable.WidthPercentage = 30;
        //    pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
        //    pdfTable.DefaultCell.BorderWidth = 1;

        //    //Adding Header row
        //    foreach (DataGridViewColumn column in dataGridView1.Columns)
        //    {
        //        PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
        //        cell.BackgroundColor = new iTextSharp.text.Color(240, 240, 240);
        //        pdfTable.AddCell(cell);
        //    }

        //    //Adding DataRow
        //    foreach (DataGridViewRow row in dataGridView1.Rows)
        //    {
        //        foreach (DataGridViewCell cell in row.Cells)
        //        {
        //            pdfTable.AddCell(cell.Value.ToString());
        //        }
        //    }

        //    //Exporting to PDF
        //    string folderPath = "C:\\PDFs\\";
        //    if (!Directory.Exists(folderPath))
        //    {
        //        Directory.CreateDirectory(folderPath);
        //    }
        //    using (FileStream stream = new FileStream(folderPath + "DataGridViewExport.pdf", FileMode.Create))
        //    {
        //        Document pdfDoc = new Document(PageSize.A2, 10f, 10f, 10f, 0f);
        //        PdfWriter.GetInstance(pdfDoc, stream);
        //        pdfDoc.Open();
        //        pdfDoc.Add(pdfTable);
        //        pdfDoc.Close();
        //        stream.Close();
        //    }
        //}

   
    }
}
