using System.Net;
using System.Net.Mail;
using System.Windows;

namespace WpfAppMailSender
{
    public sealed class EmailSendServiceClass
    {
        /// <summary>
        /// Отправить письмо
        /// </summary>
        /// <param name="strUser">Логин для авторизации на сервере</param>
        /// <param name="strPass">Пароль</param>
        /// <param name="strSubject">Тема письма</param>
        /// <param name="strBody">Тело письма</param>
        public void Send(string strUser, string strPass, string strSubject, string strBody)
        {
            try
            {
                using (var message = new MailMessage(MyConstClass.From, MyConstClass.To, strSubject, strBody))
                {
                    using (var client = new SmtpClient()
                    {
                        EnableSsl = MyConstClass.Ssl, Host = MyConstClass.ServerName, Port = MyConstClass.ServerPort, 
                        Credentials = new NetworkCredential(strUser, strPass)
                    })
                    {
                        client.Send(message);
                    }
                }

                WindowSendEnd end = new WindowSendEnd {Owner = Application.Current.MainWindow};
                end.ShowDialog();
            }
            catch (SmtpException error)
            {
                throw new SmtpException(error.Message);
            }
        }
        
    }
}