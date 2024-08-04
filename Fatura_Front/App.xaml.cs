using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Threading;


namespace Fatura_Front
{
    public partial class App : Application
    {
        private readonly ConfigHelper _configHelper = new ConfigHelper();
        private readonly ApiService _apiService = new ApiService();
        private DispatcherTimer _tokenTimer;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _configHelper.ResetLicenseID();
            girisKontrol();
        }

        private async void girisKontrol()
        {
            string email = ConfigurationManager.AppSettings["Email"];
            string password = ConfigurationManager.AppSettings["Password"];
            if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                Console.Write("1");
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                return;
                
            }

            string licenseID = ConfigurationManager.AppSettings["LicenseID"];
            if(string.IsNullOrEmpty(licenseID))
            {
                Console.Write("2");
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                return;
            }
            if(await IsLicenseValidAsync(licenseID)==false)
            {
                Console.Write("3");
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                return;
            }

            string token = ConfigurationManager.AppSettings["Token"];
            if (string.IsNullOrEmpty(token) || !TokenManager.IsTokenValid(token))
            {
                try
                {
                    var (newToken, newLicenseID) = await _apiService.GetTokenAsync(email, password);
                    _configHelper.SaveTokenToConfig(newToken);
                    _configHelper.SaveLicenseIDToConfig(newLicenseID);
                    Console.WriteLine($"Token güncellendi: {newToken}");
                    token = newToken;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Token yenileme hatası: {ex.Message}");
                }
            }

          //ü  StartTokenTimer(email, password);
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

        }

        private async Task<bool> IsLicenseValidAsync(string licenseID)
        {
            try
            {
                var apiService = new ApiService();
                var userInfoList = await apiService.GetCustomerInfoAsync(licenseID);
                return userInfoList != null && userInfoList.Count > 0;
            }
            catch
            {
                return false;
            }
        }
    }

}
