using System.Windows;
using System.Windows.Controls;
using System.IO;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System;

namespace DoberVPN
{
    /// <summary>
    /// Логика взаимодействия для MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        private bool isVpnActive;
        private string configFilesFolderPath = $"C:\\Users\\{Environment.UserName}\\OpenVPN\\config";
        private string configsDataBaseUrl = @"https://ipspeed.info/freevpn_openvpn.php?language=ru";
        string siteHeader = @"https://ipspeed.info/ovpn/";

        private Timer? timer;
        
        public MenuPage()
        {
            InitializeComponent();
            SetLocales();

            isVpnActive = false;
            ipLabel.Content = VpnManager.Instance.GetPublicIp();

            SettingsPage.OnInterfaceLanguageChanged += MenuPage_OnInterfaceLanguageChanged;
            SettingsPage.OnTimeIntervalChanged += MenuPage_OnTimeIntervalChanged;
        }

        private void MenuPage_OnTimeIntervalChanged(object? sender, EventArgs e)
        {
            if (isVpnActive)
            {
                StopTimer();
                StartTimer(VpnManager.Instance.TimeIntervalInMinute * 60000);
                //StartTimer(60000); Для отладки
            }
        }

        private void MenuPage_OnInterfaceLanguageChanged(object? sender, EventArgs e) => SetLocales();

        private void SetLocales()
        {
            nextConnectionButton.Content = Locales.Instance.GetLocale("text.next_connection");
            updateButton.Content = Locales.Instance.GetLocale("text.update_base_ip");
            yourIpLabel.Content = Locales.Instance.GetLocale("text.your_ip");
            launchButton.Content = isVpnActive
                ? Locales.Instance.GetLocale("text.disconnect")
                : Locales.Instance.GetLocale("text.launch");
        }

        private async void updateButton_Click(object sender, RoutedEventArgs e)
        {
            // Скачать содержимое сайта
            Task res = VpnManager.Instance.ReadHtmlFromUrl(configsDataBaseUrl, "ipspeed.info.html");

            string htmlContent;
            using (StreamReader sr = new StreamReader("ipspeed.info.html"))
            {
                htmlContent = sr.ReadToEnd();
            }

            // Вырезать все нужные ссылки с конфиг-файлами
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);
            HtmlNodeCollection fileLinks = doc.DocumentNode.SelectNodes("//a[@href]");

            List<string> configFiles = new List<string>();
            Regex regex = new Regex("([\\w\\p{P}])*(.ovpn)$");
            foreach (var link in fileLinks)
            {
                if (regex.IsMatch(link.InnerText))
                {
                    configFiles.Add(link.InnerText);
                }
            }

            // Удалим ВСЕ ранее скачанные файлы
            DeleteAllContentInDirectory(configFilesFolderPath);

            // Скачать конфиг файлы в указанную папку
            int progress = 0;            
            foreach (var fileName in configFiles)
            {
                progress++;
                // Импортирование конфиг файлы в OpenVPNGUI
                var currentConfigFileFolder = fileName.Substring(0, fileName.Length - 5);
                CreateFolder(configFilesFolderPath + "\\" + currentConfigFileFolder);

                await Task.Run(async () =>
                {
                    //MessageBox.Show(siteHeader + fileName);
                    await VpnManager.Instance.DownloadFileFromUrlAsync(
                        siteHeader + fileName,
                        configFilesFolderPath + "\\" + currentConfigFileFolder + "\\" + fileName);
                });

                updateButton.Content 
                    = $"{Locales.Instance.GetLocale("text.updating")} [{Convert.ToInt32(progress / (double)configFiles.Count * 100)}%]";
            }

            // Удаляем пустые папки
            await Task.Run(() =>
            {
                string[] directories = Directory.GetDirectories(configFilesFolderPath);

                foreach (string directory in directories)
                {
                    string[] subdirectoryFiles = Directory.GetFiles(directory);
                    if (subdirectoryFiles.Length == 0)
                    {
                        Directory.Delete(directory, true);
                    }
                }
            });

            updateButton.Content = Locales.Instance.GetLocale("text.update_base_ip");
            MessageBox.Show(
                Locales.Instance.GetLocale("info.configs_loaded"), 
                string.Empty, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private async void launchButton_Click(object sender, RoutedEventArgs e)
            => await SwitchConnection();

        private string GetRandomConfigFile()
        {
            try
            {
                string[] directories = Directory.GetDirectories(configFilesFolderPath);
                return directories[new Random().Next(0, directories.Length)];
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    Locales.Instance.GetLocale("warn.configs") + ex.Message,
                    string.Empty,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
            return string.Empty;
        }

        private async Task UpdatePublicIpAsync()
        {
            string publicIp = await VpnManager.Instance.GetPublicIpAsync();
            ipLabel.Content = publicIp;
        }

        private async void nextConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isVpnActive)
            {
                MessageBox.Show(
                    Locales.Instance.GetLocale("warn.vpn"),
                    string.Empty,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            await NextConnection();
        }

        private async Task NextConnection()
        {
            await VpnManager.Instance.DisconnectAllVpnAsync();

            // Переключиться на UI-поток
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                ipLabel.Content = Locales.Instance.GetLocale("text.connection");
                launchButton.Content = Locales.Instance.GetLocale("text.disconnect");
            });

            var config = GetRandomConfigFile().Split('\\').Last();
            await VpnManager.Instance.ConnectVpnAsync(config);

            await Task.Delay(15000);
            await UpdatePublicIpAsync();
        }

        private async Task SwitchConnection()
        {
            if (!isVpnActive)
            {
                isVpnActive = true;
                var prevIp = ipLabel.Content.ToString();
                bool isCorrectConnection = false;
                while (!isCorrectConnection)
                {
                    ipLabel.Content = Locales.Instance.GetLocale("text.connection");
                    launchButton.Content = Locales.Instance.GetLocale("text.disconnect");
                    var config = GetRandomConfigFile().Split('\\').Last();
                    await VpnManager.Instance.ConnectVpnAsync(config);

                    // Ждём не более 15 секунд
                    await Task.Delay(15000);

                    // Если не подключились, 
                    if (prevIp == await VpnManager.Instance.GetPublicIpAsync())
                    {
                        // то удаляем этот конфиг
                        // Здесь может быть потенциальная логика удаления нерабочего конфига
                        // ...разрываем соединение и выполняем следующее подключение

                        //await VpnManager.Instance.DisconnectConfigAsync(config);
                        await VpnManager.Instance.DisconnectAllVpnAsync();
                    }
                    else
                    {
                        isCorrectConnection = true;
                        // Если подключились, то ожидаем глобальный таймер СМЕНЫ конфига,
                        // время для которого устанавливает пользователь
                        if (VpnManager.Instance.TimeIntervalInMinute != -1)
                        {
                            StartTimer(VpnManager.Instance.TimeIntervalInMinute * 60000);
                            //StartTimer(30000); Для отладки
                        }
                    }
                }
            }
            else
            {
                isVpnActive = false;
                ipLabel.Content = Locales.Instance.GetLocale("text.disconnection");
                launchButton.Content = Locales.Instance.GetLocale("text.launch");
                await VpnManager.Instance.DisconnectAllVpnAsync();

                StopTimer();
            }

            await UpdatePublicIpAsync();
        }

        // Метод для запуска таймера
        private void StartTimer(int interval)
        {
            // Остановить предыдущий таймер, если он существует
            timer?.Change(Timeout.Infinite, Timeout.Infinite);

            // Создать новый таймер
            timer = new Timer(TimerCallback, null, 0, interval);
        }

        // Метод, который будет вызываться таймером
        private async void TimerCallback(object? state)
        {
            // Переключиться на UI-поток
            await Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                await NextConnection();
            });
        }

        // Метод для остановки таймера
        private void StopTimer()
        {
            // Остановить и освободить ресурсы таймера
            timer?.Dispose();
            timer = null;
        }

        // В методах ниже есть нелокализованные тексты!

        private void CreateFolder(string path)
        {
            try
            {
                // Проверяем существует ли папка уже
                if (!Directory.Exists(path))
                {
                    // Создаем папку
                    Directory.CreateDirectory(path);
                    //MessageBox.Show("Папка создана успешно!");
                }
                else
                {
                    MessageBox.Show(
                        "Директория " + path + "уже существует.",
                        string.Empty,
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(
                    "Произошла ошибка при создании директории:\n" + e.Message,
                    string.Empty,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
        private void DeleteAllContentInDirectory(string path)
        {
            try
            {
                // Проверяем, существует ли директория
                if (Directory.Exists(path))
                {
                    // Удаляем все файлы в директории
                    string[] files = Directory.GetFiles(path);
                    foreach (string file in files)
                    {
                        File.Delete(file);
                    }

                    // Удаляем все поддиректории в директории
                    string[] directories = Directory.GetDirectories(path);
                    foreach (string dir in directories)
                    {
                        Directory.Delete(dir, true);
                    }

                    //MessageBox.Show("Содержимое директории успешно удалено.");
                }
                else
                {
                    MessageBox.Show(
                        "Директория " + path + " не существует.",
                        string.Empty,
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(
                    "Произошла ошибка при очистки базы ip:\n" + e.Message,
                    string.Empty,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
