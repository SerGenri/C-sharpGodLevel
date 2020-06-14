using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace EmailSendServiceDll
{
    /// <summary>
    /// Класс отправитель почты
    /// Отправляет почту как одному адресату, так по списку рассылки
    /// </summary>
    public sealed class EmailSendServiceClass
    {
        public EmailSendServiceClass(string strLogin, string strPass, string strMailAddressFrom, string strSubject, string strBody, 
            bool ssl, string strServerName, int serverPort)
        {
            StrLogin = strLogin;
            StrPass = strPass;
            StrMailAddressFrom = strMailAddressFrom;
            StrSubject = strSubject;
            StrBody = strBody;
            Ssl = ssl;
            StrServerName = strServerName;
            ServerPort = serverPort;
        }

        /// <summary>
        /// Логин для авторизации на сервере
        /// </summary>
        private string StrLogin { get; set; }
        /// <summary>
        /// Пароль
        /// </summary>
        private string StrPass { get; set; }
        /// <summary>
        /// Адрес отправителя
        /// </summary>
        private string StrMailAddressFrom { get; set; }
        /// <summary>
        /// Тема письма
        /// </summary>
        private string StrSubject { get; set; }
        /// <summary>
        /// Тело письма
        /// </summary>
        private string StrBody { get; set; }
        /// <summary>
        /// Шифрованная отправка
        /// </summary>
        private bool Ssl { get; set; }
        /// <summary>
        /// Имя сервера отправки
        /// </summary>
        private string StrServerName { get; set; }
        /// <summary>
        /// Порт для сервера отправки
        /// </summary>
        private int ServerPort { get; set; }


        /// <summary>
        /// Отправить письмо одному адресату
        /// </summary>
        /// <param name="strMailAddressTo">Адрес получателя</param>
        /// <returns>Результат отправки True/False</returns>
        public bool Send(string strMailAddressTo)
        {
            try
            {
                using (var message = new MailMessage(StrMailAddressFrom, strMailAddressTo, StrSubject, StrBody))
                {
                    using (var client = new SmtpClient()
                    {
                        EnableSsl = Ssl, Host = StrServerName, Port = ServerPort, 
                        Credentials = new NetworkCredential(StrLogin, StrPass)
                    })
                    {
                        client.Send(message);
                    }
                }

                return true;
            }
            catch (SmtpException error)
            {
                throw new SmtpException(error.Message);
            }
        }


        /// <summary>
        /// Отправит письмо нескольким получателям
        /// </summary>
        /// <param name="emails">Список получателей</param>
        /// <returns>Результат отправки True/False</returns>
        public bool SendMails(List<string> emails)
        {
            foreach (string itemEmail in emails)
            {
                Send(itemEmail);
            }

            return true;
        }
    }
}