using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Windows;
using System.Windows.Documents;
using EmailSendServiceDll;

namespace WpfAppMailSender
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow() => InitializeComponent();

        /// <summary>
        /// Вывести окно с ошибкой
        /// </summary>
        /// <param name="strError"></param>
        private void MessageErrorWindow(string strError)
        {
            WindowMessageSendError window = new WindowMessageSendError(strError)
            {
                Owner = Application.Current.MainWindow
            };
            window.ShowDialog();
        }
        /// <summary>
        /// Вывести окно с информацией
        /// </summary>
        /// <param name="strInfo"></param>
        private void MessageInfoWindow(string strInfo)
        {
            WindowSendEnd window = new WindowSendEnd(strInfo)
            {
                Owner = Application.Current.MainWindow
            };
            window.ShowDialog();
        }


        private void TabSwitcherControl_OnBack(object sender, RoutedEventArgs e)
        {
            if (MainTabControl.SelectedIndex != 0)
            {
                MainTabControl.SelectedIndex--;
            }
        }
        private void TabSwitcherControl_OnForward(object sender, RoutedEventArgs e)
        {
            if (MainTabControl.SelectedIndex != MainTabControl.Items.Count - 1)
            {
                MainTabControl.SelectedIndex++;
            }
        }


        /// <summary>
        /// Проверка параметров отправки
        /// </summary>
        /// <param name="strBody"></param>
        /// <param name="strLogin"></param>
        /// <param name="strPass"></param>
        /// <param name="strSubject"></param>
        private bool Check(string strBody, string strLogin, string strPass, string strSubject)
        {
            bool check = true;

            if (strBody?.Length <= 2)
            {
                MessageErrorWindow("Заполните текст письма");
                check = false;

                MainTabControl.SelectedItem = TabEditMail;
            }

            if (strSubject?.Length == 0)
            {
                MessageErrorWindow("Тема не заполнена");
                check = false;

                MainTabControl.SelectedItem = TabEditMail;
            }

            if (string.IsNullOrEmpty(strLogin))
            {
                MessageErrorWindow("Выберите отправителя");
                check = false;

                MainTabControl.SelectedItem = TabSender;
            }

            if (string.IsNullOrEmpty(strPass))
            {
                MessageErrorWindow("Укажите пароль отправителя");
                check = false;

                MainTabControl.SelectedItem = TabSender;
            }

            return check;
        }

        private void BtnSendAtOnce_OnClick(object sender, RoutedEventArgs e)
        {
            KeyValuePair<string, string> itemSender = (KeyValuePair<string, string>) CbSenderSelect.SelectionBoxItem;
            KeyValuePair<string, int> itemServer = (KeyValuePair<string, int>) CbServerSelect.SelectionBoxItem;

            string strLogin = itemSender.Key;
            string strPass = EncrypterDll.Encrypter.Deencrypt(itemSender.Value);
            string strMailAddressFrom = itemSender.Key;
            string strSubject = TbSubject.Text;
            string strBody = new TextRange(RtbBodyMail.Document.ContentStart, RtbBodyMail.Document.ContentEnd).Text;
            string strServerName = itemServer.Key;
            int serverPort = itemServer.Value;

            if (!Check(strBody, strLogin, strPass, strSubject)) return;

            //подготовка списка
            List<string> listEmails = new List<string>();
            IQueryable<Email> db = DataBaseClass.Emails;
            foreach (Email itemEmail in db)
            {
                listEmails.Add(itemEmail.EmailAddress);
            }

            if (listEmails.Count > 0)
            {
                //отправка на список
                try
                {
                    EmailSendServiceClass emailSender = new EmailSendServiceClass(strLogin, strPass, strMailAddressFrom, strSubject, strBody,
                        true, strServerName, serverPort);

                    if (emailSender.SendMails(listEmails))
                    {
                        MessageInfoWindow("Почта отправлена");
                    }
                }
                catch (Exception error)
                {
                    MessageErrorWindow(error.Message);
                }
            }
            else
            {
                MessageErrorWindow("Список адресатов пуст");
            }
        }

        private void BtnSendClock_OnClick(object sender, RoutedEventArgs e)
        {
            KeyValuePair<string, string> itemSender = (KeyValuePair<string, string>)CbSenderSelect.SelectionBoxItem;
            KeyValuePair<string, int> itemServer = (KeyValuePair<string, int>)CbServerSelect.SelectionBoxItem;

            string strLogin = itemSender.Key;
            string strPass = EncrypterDll.Encrypter.Deencrypt(itemSender.Value);
            string strMailAddressFrom = itemSender.Key;
            string strSubject = TbSubject.Text;
            string strBody = new TextRange(RtbBodyMail.Document.ContentStart, RtbBodyMail.Document.ContentEnd).Text;
            string strServerName = itemServer.Key;
            int serverPort = itemServer.Value;
            SchedulerClass sc = new SchedulerClass();

            TimeSpan tsSendTime = sc.GetSendTime​(TpTimePicker.Text);
            if (tsSendTime == new TimeSpan())
            {
                MessageErrorWindow("Некорректный формат даты");
                return;
            }

            DateTime dtSenDateTime = (CldSchedulDateTimes.SelectedDate ?? DateTime.Today).Add(tsSendTime);
            if (dtSenDateTime < DateTime.Now)
            {
                MessageErrorWindow("Дата и время отправки писем не могут быть раньше, чем настоящее время");
                return;
            }

            if (!Check(strBody, strLogin, strPass, strSubject)) return;

            //подготовка списка
            List<string> listEmails = new List<string>();
            IQueryable<Email> db = DataBaseClass.Emails;
            foreach (Email itemEmail in db)
            {
                listEmails.Add(itemEmail.EmailAddress);
            }

            if (listEmails.Count > 0)
            {
                EmailSendServiceClass emailSender = new EmailSendServiceClass(strLogin, strPass, strMailAddressFrom, strSubject, strBody,
                    true, strServerName, serverPort);
                sc.EventTimerTick += EventTimerTick;
                sc.TimerSendEmails​Start(dtSenDateTime, listEmails, emailSender);

                MessageInfoWindow($"Запущена оправка по расписанию{Environment.NewLine}почта будет отправлена в{Environment.NewLine}{dtSenDateTime}");
            }
            else
            {
                MessageErrorWindow("Список адресатов пуст");
            }
        }


        /// <summary>
        /// Подписка на сработку таймера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventTimerTick(object sender, RoutedEventArgs e)
        {
            if (sender is SmtpException objException)
            {
                MessageErrorWindow(objException.Message);
            }

            if (sender is bool objSender)
            {
                if (objSender)
                {
                    MessageInfoWindow("Почта отправлена по расписанию");
                }
                else
                {
                    MessageErrorWindow("Ошибка при отправке почты");
                }
            }
        }


        private void BtnClock_OnClick(object sender, RoutedEventArgs e)
        {
            MainTabControl.SelectedItem = TabPlaner;
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
