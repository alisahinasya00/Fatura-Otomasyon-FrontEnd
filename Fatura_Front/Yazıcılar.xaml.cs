using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Drawing.Printing;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Fatura_Front.Models;

namespace Fatura_Front
{
    /// <summary>
    /// Yazıcılar.xaml etkileşim mantığı
    /// </summary>
    public partial class Yazıcılar : UserControl
    {
        public static string SelectedPrinter { get; private set; }
        public Yazıcılar()
        {
            InitializeComponent();
            LoadPrinters();
        }

        private void LoadPrinters()
        {
            try
            {
                var printers = PrinterSettings.InstalledPrinters;
                PrinterComboBox.ItemsSource = printers;
                if (printers.Count > 0)
                {
                    PrinterComboBox.SelectedIndex = 0; // Optionally, select the first printer
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions related to printer loading
                System.Diagnostics.Debug.WriteLine($"Printer load error: {ex.Message}");
            }
        }

        private void PrinterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PrinterComboBox.SelectedItem != null)
            {
                SelectedPrinter = PrinterComboBox.SelectedItem.ToString();
            }
        }

        
    }
}
