using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Timers;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Windows.Threading;
using System.Threading;

namespace Fatura_Front
{
    public partial class MainWindow : Window
    {
        private readonly ConfigHelper _configHelper = new ConfigHelper();
        private readonly ApiService _apiService = new ApiService();
        private DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent(); 
            this.Width = 1000;
            this.Height = 500;
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMinutes(55); // 55 dakika
            _timer.Tick += Timer_Tick; // Timer'ın Tick olayına bir olay işleyici ekleyin
            _timer.Start(); // Timer'ı başlatın
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Her 5 dakikada bir çalışacak kodu buraya yazın
            tokenGuncelle();
        }

        private async void tokenGuncelle()
        {
            string email = ConfigurationManager.AppSettings["Email"];
            string password = ConfigurationManager.AppSettings["Password"];
            string licenceid = ConfigurationManager.AppSettings["LicenseID"];
            try
            {
                var (newToken, newLicenseID) = await _apiService.GetTokenAsync(email, password);
                _configHelper.SaveTokenToConfig(newToken);
                _configHelper.SaveLicenseIDToConfig(newLicenseID);
                Console.WriteLine($"Token yenilendi: {newToken}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Token yenileme hatası: {ex.Message}");
            }

           // MessageBox.Show("token yenilendi");

        }

    }
}