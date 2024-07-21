using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Application = System.Windows.Application;

namespace DoberVPN
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int currentPage;
        private List<Page> pages;
        private NotifyIcon notifyIcon;

        public MainWindow()
        {
            InitializeComponent();
            SetupTrayIcon();
            Hide();

            pages = new List<Page>()
            {
                new MenuPage(),
                new SettingsPage()
            };

            currentPage = 0;
            
            PageFrame.Content = pages[currentPage];

        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            currentPage = currentPage != pages.Count - 1 
                ? currentPage + 1 
                : 0;

            PageFrame.Content = pages[currentPage];
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            VpnManager.Instance.DisconnectAllVpnAsync();
            
            // Находим процесс OpenVPN GUI
            var processes = Process.GetProcessesByName("openvpn-gui")
            // Чтобы закрыть сервисы нужно запустить от имени администратора
                .Concat(Process.GetProcessesByName("ovpnhelper_service"))
                .Concat(Process.GetProcessesByName("openvpnserv2"))
                .Concat(Process.GetProcessesByName("openvpn"))
                .ToList();

            // Завершаем каждый экземпляр процесса
            foreach (Process process in processes)
            {
                process.Kill();
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await VpnManager.Instance.ExecuteCommandAsync(@"C:\Program Files\OpenVPN\bin", "openvpn-gui.exe");
            await VpnManager.Instance.ExecuteCommandAsync(@"C:\Program Files\OpenVPN\bin", "openvpn-gui.exe --command silent_connection 1");
        }

        private void SetupTrayIcon()
        {
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = new System.Drawing.Icon("dobervpn.ico"); // Замените "your-icon.ico" на путь к вашей иконке
            notifyIcon.Visible = true;
            notifyIcon.Text = "DoberVPN";
            notifyIcon.DoubleClick += NotifyIcon_DoubleClick;

            // Создание контекстного меню
            var contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.Add("Show", null, (s, e) => ShowMainWindow());
            contextMenuStrip.Items.Add("Exit", null, (s, e) => ExitApplication());

            notifyIcon.ContextMenuStrip = contextMenuStrip;

            // Скрыть главное окно
            WindowState = WindowState.Minimized;
            ShowInTaskbar = false;
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            ShowMainWindow();
        }

        private void ShowMainWindow()
        {
            ShowInTaskbar = true;
            WindowState = WindowState.Normal;
            Activate();
            Show();
        }

        private void ExitApplication()
        {
            notifyIcon.Visible = false;
            notifyIcon.Dispose();
            Application.Current.Shutdown();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Отменяем закрытие окна
            e.Cancel = true;

            // Сворачиваем окно в трей
            ShowInTaskbar = false;
            WindowState = WindowState.Minimized;
        }
    }
}