using Microsoft.Win32;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace DoberVPN
{
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public static EventHandler? OnInterfaceLanguageChanged;
        public static EventHandler? OnTimeIntervalChanged;

        public SettingsPage()   
        {
            InitializeComponent();
            SetLocales();
            autoLaunchCheckBox.IsChecked = VpnManager.Instance.IsAutoStart;
        }

        private void SetLocales()
        {
            intervalLabel.Content = Locales.Instance.GetLocale("text.ip_change_interval");
            languageLabel.Content = Locales.Instance.GetLocale("text.interface_language");
            autoLaunchTextBlock.Text = Locales.Instance.GetLocale("text.startup_at_system");
        }

        private void timeIntervalComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (timeIntervalComboBox.SelectedIndex)
            {
                case 0:
                    VpnManager.Instance.TimeIntervalInMinute = -1;
                    break;
                case 1:
                    VpnManager.Instance.TimeIntervalInMinute = 1;
                    break;
                case 2:
                    VpnManager.Instance.TimeIntervalInMinute = 10;
                    break;
                case 3:
                    VpnManager.Instance.TimeIntervalInMinute = 30;
                    break;
                case 4:
                    VpnManager.Instance.TimeIntervalInMinute = 60;
                    break;
                case 5:
                    VpnManager.Instance.TimeIntervalInMinute = 120;
                    break;
            }
            OnTimeIntervalChanged?.Invoke(this, EventArgs.Empty);
        }

        private void languagesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (languagesComboBox.SelectedItem != null 
                && Locales.Instance.IsRussianLang != (languagesComboBox.SelectedIndex == 0))
            {
                Locales.Instance.IsRussianLang = languagesComboBox.SelectedIndex == 0;
                SetLocales();
                OnInterfaceLanguageChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void checkBox_Click(object sender, RoutedEventArgs e)
        {
            //string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string exePath = Process.GetCurrentProcess().MainModule.FileName;

            if (!VpnManager.Instance.IsAutoStart)
            {
                // Установка приложения в автозагрузку
                SetAutoStart(VpnManager.Instance.AppName, exePath);
            }
            else
            {
                // Удаление приложения из автозагрузки
                RemoveAutoStart(VpnManager.Instance.AppName);
            }
        }

        private void SetAutoStart(string appName, string exePath)
        {
            RegistryKey? regKey = Registry.CurrentUser
                .OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (regKey != null)
            {
                regKey.SetValue(appName, exePath);
                regKey.Close();
            }
        }

        private void RemoveAutoStart(string appName)
        {
            try
            {
                using (RegistryKey? regKey = Registry.CurrentUser
                    .OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
                {
                    if (regKey != null)
                    {
                        regKey.DeleteValue(appName, false);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    Locales.Instance.GetLocale("err.delete_autoload") + ex.Message,
                    string.Empty,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
