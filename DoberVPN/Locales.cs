namespace DoberVPN
{
    public class Locales
    {
        private static Locales? instance;
        public static Locales Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Locales();
                }
                return instance;

            }
        }

        public bool IsRussianLang = true;
        private Dictionary<string, string> ruLocales;
        private Dictionary<string, string> enLocales;
        private List<string> keys = new List<string>()
        {
            "text.next_connection",     // 0
            "text.update_base_ip",      // 1
            "text.your_ip",             // 2
            "text.launch",              // 3
            "text.ip_change_interval",  // 4
            "text.interface_language",  // 5
            "text.startup_at_system",   // 6
            "text.updating",            // 7
            "text.connection",          // 8
            "text.disconnect",          // 9
            "text.disconnection",       // 10
            "info.configs_loaded",      // 11
            "info.cancel_task",         // 12
            "warn.vpn",                 // 13
            "warn.configs",             // 14
            "err.web",                  // 15
            "err.http",                 // 16
            "err.ip",                   // 17
            "err.download",             // 18
            "err.delete_autoload",      // 19
            "err.is_autoload",          // 20
        };

        private Locales() 
        {
            ruLocales = new Dictionary<string, string>()
            {
                {keys[0], "Следующее подключение"},
                {keys[1], "Обновить базу IP"},
                {keys[2], "Ваш IP:"},
                {keys[3], "Запуск"},
                {keys[4], "Интервал смены IP"},
                {keys[5], "Язык интерфейса"},
                {keys[6], "Автозагрузка при запуске системы"},
                {keys[7], "Обновление"},
                {keys[8], "Подключение..."},
                {keys[9], "Отключиться"},
                {keys[10], "Отключение..."},
                {keys[11], "Конфиг-файлы скачены"},
                {keys[12], "Задача была отменена."},
                {keys[13], "VPN не включён. Чтобы включить VPN нажмите на \"Запуск\"\n"},
                {keys[14], "Произошла ошибка при попытке получить список конфигов:\n"},
                {keys[15], "Ошибка при загрузке страницы: "},
                {keys[16], "Ошибка HTTP-запроса: "},
                {keys[17], "Произошла ошибка при получении IP-адреса: "},
                {keys[18], "При загрузке файла произошла ошибка: "},
                {keys[19], "Ошибка при удалении из автозагрузки: "},
                {keys[20], "Ошибка при проверке автозагрузки: "},
            };

            enLocales = new Dictionary<string, string>()
            {
                {keys[0], "Next connection"},
                {keys[1], "Update base IP"},
                {keys[2], "Your IP:"},
                {keys[3], "Launch"},
                {keys[4], "IP change interval"},
                {keys[5], "Interface language"},
                {keys[6], "Autoload at system startup"},
                {keys[7], "Updating"},
                {keys[8], "Connection..."},
                {keys[9], "Disconnect"},
                {keys[10], "Disconnection..."},
                {keys[11], "Config files has been loaded"},
                {keys[12], "The task was canceled"},
                {keys[13], "VPN is not enabled. To enable VPN, click on \"Launch\"\n"},
                {keys[14], "An error occurred while trying to get a list of configs:\n"},
                {keys[15], "Error loading page: "},
                {keys[16], "HTTP request error: "},
                {keys[17], "An error occurred while obtaining the IP address: "},
                {keys[18], "An error occurred while downloading the file: "},
                {keys[19], "Error when deleting from autoload: "},
                {keys[20], "Error checking autoload: "},
            };
        }

        public string GetLocale(string key) => IsRussianLang ? ruLocales[key] : enLocales[key];
    }
}
