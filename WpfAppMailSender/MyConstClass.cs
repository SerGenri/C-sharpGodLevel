namespace WpfAppMailSender
{
    public static class MyConstClass
    {
        /// <summary>
        /// Адрес отправитель
        /// </summary>
        public const string From = "SendSpamBot@yandex.ru";

        /// <summary>
        /// Адрес получатель
        /// </summary>
        public const string To = "SendSpamBot@yandex.ru";

        /// <summary>
        /// Имя севреа для отправки
        /// </summary>
        public const string ServerName = "smtp.yandex.ru";

        /// <summary>
        /// Порт для отправки
        /// </summary>
        public const int ServerPort = 25;

        /// <summary>
        /// Шифрованная отправка
        /// </summary>
        public const bool Ssl = true;
    }
}