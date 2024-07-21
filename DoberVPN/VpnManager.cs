using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Windows;

namespace DoberVPN
{
    // Класс реализован через паттерн Синглтон
    public class VpnManager
    {
        private static VpnManager? instance;
        public static VpnManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new VpnManager();
                }
                return instance;
            }
        }
        private VpnManager() { }

        public string AppName => "DoberVPN";
        public bool IsAutoStart => IsAutoStartEnabled(AppName);
        public int TimeIntervalInMinute = -1;

        public async Task ReadHtmlFromUrl(string url, string path)
        {
            string content = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Загрузка содержимого страницы
                    HttpResponseMessage response = await client.GetAsync(url);

                    // Проверка успешности запроса
                    if (response.IsSuccessStatusCode)
                    {
                        // Чтение содержимого страницы как строки
                        string htmlContent = await response.Content.ReadAsStringAsync();
                        content += htmlContent;
                    }
                    else
                    {
                        MessageBox.Show(
                            Locales.Instance.GetLocale("err.web") + response.StatusCode,
                            string.Empty,
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
                catch (HttpRequestException e)
                {
                    MessageBox.Show(
                        Locales.Instance.GetLocale("err.http") + e.Message,
                        string.Empty,
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            using (StreamWriter streamWriter = new StreamWriter(path))
            {
                streamWriter.WriteLine(content);
            }
        }

        public async Task DownloadFileFromUrlAsync(string url, string path)
        {
            HttpClient httpClient = new HttpClient();
            try
            {
                using HttpResponseMessage response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                await using Stream contentStream = await response.Content.ReadAsStreamAsync(),
                                   fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);

                await contentStream.CopyToAsync(fileStream);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(
                //    Locales.Instance.GetLocale("err.download") + ex.Message,
                //    string.Empty,
                //    MessageBoxButton.OK,
                //    MessageBoxImage.Error);
            }
        }


        public string GetPublicIp()
        {
            try
            {
                string externalIP;
                using (WebClient client = new WebClient())
                {
                    externalIP = client.DownloadString("https://ifconfig.me/ip");
                }
                return externalIP;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    Locales.Instance.GetLocale("err.ip") + ex.Message,
                    string.Empty,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            return string.Empty;
        }

        public async Task<string> GetPublicIpAsync()
        {
            (string, string) result = await ExecuteCommandAsync("", "curl ifconfig.me");
            string output = result.Item1;
            return output.Trim(); // Удаляем лишние пробелы и символы перевода строки
        }

        public async Task ConnectVpnAsync(string configFileName) 
            => await ExecuteCommandAsync(@"C:\Program Files\OpenVPN\bin", "openvpn-gui.exe --command connect " + configFileName);
        

        public async Task DisconnectAllVpnAsync()
            => await ExecuteCommandAsync(@"C:\Program Files\OpenVPN\bin", "openvpn-gui.exe --command disconnect_all");
        

        public async Task DisconnectConfigAsync(string configFileName)
            => await ExecuteCommandAsync(@"C:\Program Files\OpenVPN\bin", "openvpn-gui.exe --command disconnect " + configFileName);
        

        public async Task<(string, string)> ExecuteCommandAsync(string directoryPath, string command)
        {
            return await Task<(string, string)>.Run(() =>
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    WorkingDirectory = directoryPath,
                    FileName = "cmd.exe",
                    Arguments = $"/c {command}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = new Process())
                {
                    process.StartInfo = startInfo;
                    process.Start();

                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    return (output, error);
                }
            });
        }

        private bool IsAutoStartEnabled(string appName)
        {
            try
            {
                using (RegistryKey? regKey = Registry.CurrentUser
                    .OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", false))
                {
                    if (regKey != null)
                    {
                        return regKey.GetValue(appName) != null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    Locales.Instance.GetLocale("err.is_autoload") + ex.Message,
                    string.Empty,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            return false;
        }
    }
}
