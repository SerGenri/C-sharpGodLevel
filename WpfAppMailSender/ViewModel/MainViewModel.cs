using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using EmailSendServiceDll;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using WpfAppMailSender.Service;
using WpfAppMailSender.Views;

namespace WpfAppMailSender.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataAccessService dataService)
        {
            _dataService = dataService;

            CldSchedulDateTimes = DateTime.Now;
        }

        #region Sender Edit


        private readonly IDataAccessService _dataService;


        private CollectionView ListEmailsView { get; set; }


        private ObservableCollection<Email> _listEmails = new ObservableCollection<Email>();
        /// <summary>
        /// Список получателей
        /// </summary>
        public ObservableCollection<Email> ListEmails
        {
            get => _listEmails;
            set => Set(ref _listEmails, value);
        }


        private bool _textBoxLock;
        public bool TextBoxLock
        {
            get => _textBoxLock;
            set => Set(ref _textBoxLock, value);
        }

        private bool _btnLock;
        public bool BtnLock
        {
            get => _btnLock;
            set => Set(ref _btnLock, value);
        }


        private Email _currentEmail = new Email();
        public Email CurrentEmail
        {
            get => _currentEmail;
            set
            {
                Set(ref _currentEmail, value);

                TextBoxLock = value != null;

                if (value != null) 
                    value.PropertyChanged += ValueOnPropertyChanged;
            }
        }
        private void ValueOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(CurrentEmail.Name) || string.IsNullOrEmpty(CurrentEmail.EmailAddress))
            {
                BtnLock = false;
            }
            else
            {
                BtnLock = true;
            }
        }


        private string _emailFilterText;
        public string EmailFilterText
        {
            get => _emailFilterText;
            set
            {
                _emailFilterText = value;

                Set(ref _emailFilterText, value);

                FilterEmails();
            }
        }

        private void FilterEmails()
        {
            ListEmailsView = (CollectionView)CollectionViewSource.GetDefaultView(ListEmails);

            ListEmailsView.Filter = (obj) =>
            {
                Email p = obj as Email;

                bool filter1 = false, filter2 = true;
                if (EmailFilterText?.Length > 0 && p != null)
                {
                    string filterText = EmailFilterText.ToUpper();

                    filter1 = p.Name.ToUpper().Contains(filterText);
                    filter2 = p.EmailAddress.ToUpper().Contains(filterText);
                }

                return filter1 || filter2;
            };
        }


        private void GetEmails()
        {
            ListEmails = _dataService.GetEmails();
            CurrentEmail = null;

            FilterEmails();
        }
        private MyRelayCommand _readAllMailsCommand;
        public MyRelayCommand ReadAllMailsCommand
        {
            get
            {
                return _readAllMailsCommand ??
                       (_readAllMailsCommand = new MyRelayCommand(obj =>
                       {
                           GetEmails();
                       }));
            }
        }


        private MyRelayCommand _addEmailCommand;
        public MyRelayCommand AddEmailCommand
        {
            get
            {
                return _addEmailCommand ??
                       (_addEmailCommand = new MyRelayCommand(obj =>
                       {
                           Email email = new Email {Id = ListEmails.Max(x => x.Id) + 1};

                           _dataService.CreateEmail(email);
                           ListEmails.Add(email);

                           CurrentEmail = email;
                       }));
            }
        }


        private MyRelayCommand _saveChangesEmailCommand;
        public MyRelayCommand SaveChangesEmailCommand
        {
            get
            {
                return _saveChangesEmailCommand ??
                       (_saveChangesEmailCommand = new MyRelayCommand(obj =>
                       {
                           _dataService.SaveChanges();

                           GetEmails();

                       },obj => BtnLock));
            }
        }

        #endregion

        #region Email Send
                
        public string TbSubject { get; set; }
        public string TpTimePicker { get; set; }
        public DateTime? CldSchedulDateTimes { get; set; }
        public string RtbBodyMail { get; set; }


        public Dictionary<string, int> SmtpServerDictionary { get; } = new Dictionary<string, int>
        {
            {"smtp.yandex.ru" , 25 },
            {"smtp.mail.ru" , 25 },
            {"smtp.google.ru" , 25 }
        };
        public KeyValuePair<string, int> SmtpServerDictionaryEntry { get; set; }


        public static Dictionary<string, string> SendersDictionary { get; } = new Dictionary<string, string>
        {
            {"SendSpamBot@yandex.ru" , EncrypterDll.Encrypter.Encrypt("P@$$w0rd") },
            {"SendSpamBot@yandex.ru1" , EncrypterDll.Encrypter.Encrypt("P@$$w0rd") },
            {"SendSpamBot@yandex.ru2" , EncrypterDll.Encrypter.Encrypt("P@$$w0rd") },
            {"SendSpamBot@yandex.ru3" , EncrypterDll.Encrypter.Encrypt("P@$$w0rd") },
            {"SendSpamBot@yandex.ru4" , EncrypterDll.Encrypter.Encrypt("P@$$w0rd") }
        };
        public KeyValuePair<string, string> SendersDictionaryEntry { get; set; }



        /// <summary>
        /// Вывести окно с ошибкой
        /// </summary>
        /// <param name="strError"></param>
        private static void MessageErrorWindow(string strError)
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
        private static void MessageInfoWindow(string strInfo)
        {
            WindowSendEnd window = new WindowSendEnd(strInfo)
            {
                Owner = Application.Current.MainWindow
            };
            window.ShowDialog();
        }


        /// <summary>
        /// Проверка параметров отправки
        /// </summary>
        /// <param name="strBody"></param>
        /// <param name="strLogin"></param>
        /// <param name="strPass"></param>
        /// <param name="strSubject"></param>
        private static bool Check(string strBody, string strLogin, string strPass, string strSubject)
        {
            bool check = true;

            if (string.IsNullOrEmpty(strBody))
            {
                MessageErrorWindow("Заполните текст письма");
                check = false;

                //MainTabControl.SelectedItem = TabEditMail;
            }

            if (string.IsNullOrEmpty(strSubject))
            {
                MessageErrorWindow("Тема не заполнена");
                check = false;

                //MainTabControl.SelectedItem = TabEditMail;
            }

            if (string.IsNullOrEmpty(strLogin))
            {
                MessageErrorWindow("Выберите отправителя");
                check = false;

                //MainTabControl.SelectedItem = TabSender;
            }

            if (string.IsNullOrEmpty(strPass))
            {
                MessageErrorWindow("Укажите пароль отправителя");
                check = false;

                //MainTabControl.SelectedItem = TabSender;
            }

            return check;
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



        private MyRelayCommand _btnSendAtOnceOnClick;
        public MyRelayCommand BtnSendAtOnceOnClickCommand
        {
            get
            {
                return _btnSendAtOnceOnClick ??
                       (_btnSendAtOnceOnClick = new MyRelayCommand(obj =>
                       {
                           string strLogin = SendersDictionaryEntry.Key;
                           string strPass = EncrypterDll.Encrypter.Deencrypt(SendersDictionaryEntry.Value);
                           string strMailAddressFrom = SendersDictionaryEntry.Key;
                           string strSubject = TbSubject;
                           string strBody = RtbBodyMail;
                           string strServerName = SmtpServerDictionaryEntry.Key;
                           int serverPort = SmtpServerDictionaryEntry.Value;

                           if (!Check(strBody, strLogin, strPass, strSubject)) return;

                           //подготовка списка
                           List<string> listEmails = new List<string>();
                           foreach (Email itemEmail in ListEmails)
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
                       }));
            }
        }


        private MyRelayCommand _btnSendClockOnClick;
        public MyRelayCommand BtnSendClockOnClickCommand
        {
            get
            {
                return _btnSendClockOnClick ??
                       (_btnSendClockOnClick = new MyRelayCommand(obj =>
                       {
                           string strLogin = SendersDictionaryEntry.Key;
                           string strPass = EncrypterDll.Encrypter.Deencrypt(SendersDictionaryEntry.Value);
                           string strMailAddressFrom = SendersDictionaryEntry.Key;
                           string strSubject = TbSubject;
                           string strBody = RtbBodyMail;
                           string strServerName = SmtpServerDictionaryEntry.Key;
                           int serverPort = SmtpServerDictionaryEntry.Value;
                           SchedulerClass sc = new SchedulerClass();

                           TimeSpan tsSendTime = sc.GetSendTime​(TpTimePicker);
                           if (tsSendTime == new TimeSpan())
                           {
                               MessageErrorWindow("Некорректный формат даты");
                               return;
                           }

                           DateTime dtSenDateTime = CldSchedulDateTimes.GetValueOrDefault().Date.Add(tsSendTime);
                           if (dtSenDateTime < DateTime.Now)
                           {
                               MessageErrorWindow("Дата и время отправки писем не могут быть раньше, чем настоящее время");
                               return;
                           }

                           if (!Check(strBody, strLogin, strPass, strSubject)) return;

                           //подготовка списка
                           List<string> listEmails = new List<string>();
                           foreach (Email itemEmail in ListEmails)
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

                       }));
            }
        }



        #endregion

        private MyRelayCommand _exitAppCommand;
        public MyRelayCommand ExitAppCommand
        {
            get
            {
                return _exitAppCommand ??
                       (_exitAppCommand = new MyRelayCommand(obj =>
                       {

                           Application.Current.Shutdown();

                       }));
            }
        }
    }
}