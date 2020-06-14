using System;
using System.Collections.Generic;
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
    public class SchedulerClass
    {
        private List<string> ListEmails​ { get; set; }
        private DateTime DtSend​ { get; set; }
        private EmailSendServiceClass EmailSender { get; set; }

        private readonly DispatcherTimer _timer = new DispatcherTimer();

        public event RoutedEventHandler EventTimerTick;

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


        /// <summary>
        /// Запускаем таймер
        /// </summary>
        /// <param name="dtSend​">Дата время отправки</param>
        /// <param name="listEmails​">Список получателей</param>
        /// <param name="emailSender">Экземпляр класса, отвечающего за отправку писем, присваиваем</param>
        public void TimerSendEmails​Start(DateTime dtSend​, List<string> listEmails​, EmailSendServiceClass emailSender)
        {
            EmailSender = emailSender;
            ListEmails​ = listEmails​; 
            DtSend​ = dtSend​;

            _timer.Tick += TimerOnTick;
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Start();
        }

        private void TimerOnTick(object sender, EventArgs e)
        {
            if (DtSend​.ToShortTimeString() == DateTime.Now.ToShortTimeString())
            {
                _timer.Stop();

                try
                {
                    EventTimerTick?.Invoke(EmailSender.SendMails(ListEmails​), new RoutedEventArgs());
                }
                catch (SmtpException error)
                {
                    EventTimerTick?.Invoke(error, new RoutedEventArgs());
                }
            }
        }
 }
}