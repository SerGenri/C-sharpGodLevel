using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using System.Windows;
using System.Windows.Threading;
using EmailSendServiceDll;

namespace WpfAppMailSender
{

    /// <summary>
    /// Класс-планировщик, который создает расписание, следит за его выполнением и напоминает о событиях
    /// Также помогает автоматизировать рассылку писем в соответствии с расписанием
    /// </summary>
    public class SchedulerClass : IDisposable
    {
        private List<string> ListEmails { get; set; }
        public ObservableCollection<EmailDataTimeTextClass> EmailsTimeDictionary { get; set; }

        private string StrLogin​ { get; set; }
        private string StrPass { get; set; }
        private string StrMailAddressFrom { get; set; }
        private string StrServerName { get; set; }
        private int ServerPort { get; set; }


        private readonly DispatcherTimer _timer = new DispatcherTimer();
        
        public event RoutedEventHandler EventTimerTick;

        /// <summary>
        /// Запускаем таймер
        /// </summary>
        /// <param name="emailsTimeDictionary">Список Дата время отправки + текст письма</param>
        /// <param name="listEmails"></param>
        /// <param name="strLogin​"></param>
        /// <param name="strPass"></param>
        /// <param name="strMailAddressFrom"></param>
        /// <param name="strServerName"></param>
        /// <param name="serverPort"></param>
        public void TimerSendEmails​Start(ObservableCollection<EmailDataTimeTextClass> emailsTimeDictionary, List<string> listEmails, string strLogin​, 
            string strPass, string strMailAddressFrom, string strServerName, int serverPort)
        {
            ListEmails = listEmails;
            EmailsTimeDictionary = emailsTimeDictionary;
            StrLogin​ = strLogin​;
            StrPass = strPass;
            StrMailAddressFrom = strMailAddressFrom;
            StrServerName = strServerName;
            ServerPort = serverPort;

            _timer.Tick += TimerOnTick;
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Start();
        }


        /// <summary>
        /// Метод, который превращает строку из текстбокса tbTimePicker в TimeSpan
        /// </summary>
        /// <param name="strSendTime"></param>
        /// <returns></returns>
        public TimeSpan GetSendTime​(string strSendTime)
        {
            TimeSpan tsSendTime = new TimeSpan();

            try
            {
                tsSendTime = TimeSpan.Parse(strSendTime);
            }
            catch (Exception)
            {
                // Exception
            }

            return tsSendTime;
        }


        private void TimerOnTick(object sender, EventArgs e)
        {
            if (EmailsTimeDictionary.Count == 0)
            {
                _timer.Stop();

                EventTimerTick?.Invoke(true, new RoutedEventArgs());
                return;
            }

            DateTime dataTimeNow = DateTime.Now;
            string dataNow = dataTimeNow.Date.ToShortDateString();
            string timeNow = new DateTime(1, 1, 1, dataTimeNow.TimeOfDay.Hours, 
                    dataTimeNow.TimeOfDay.Minutes, 0).ToShortTimeString();

            if (dataTimeNow.Second == 0)
            {
                foreach (EmailDataTimeTextClass emailDataTimeTextClass in EmailsTimeDictionary.Reverse())
                {
                    if (emailDataTimeTextClass.DataSend.ToShortDateString().Equals(dataNow) 
                        && emailDataTimeTextClass.TimeSend.Equals(timeNow))
                    {
                        try
                        {
                            EmailSendServiceClass emailSender = new EmailSendServiceClass(StrLogin​, StrPass, StrMailAddressFrom,
                                emailDataTimeTextClass.Subject, emailDataTimeTextClass.Body, true, StrServerName, ServerPort);

                            emailSender.SendMails(ListEmails);

                            EmailsTimeDictionary.Remove(emailDataTimeTextClass);
                        }
                        catch (SmtpException error)
                        {
                            EventTimerTick?.Invoke(error, new RoutedEventArgs());
                        }
                    }
                }
            }
        }


        public void Dispose()
        {
            ListEmails = null;
            EmailsTimeDictionary = null;
        }
    }
}