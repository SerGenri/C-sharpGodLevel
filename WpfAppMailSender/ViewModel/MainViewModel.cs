using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Windows;
using System.Windows.Data;
using EmailSendServiceDll;
using GalaSoft.MvvmLight;
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

            CalendarScheduler = DateTime.Now;

            EmailsTimeDictionary = new ObservableCollection<EmailDataTimeTextClass>();
        }

        #region Sender Edit


        private readonly IDataAccessService _dataService;


        #region Filter/Sort

        private CollectionViewSource _listEmailsView;
        public ICollectionView ListEmailsView => _listEmailsView?.View;


        private void ListEmailsViewOnFilter(object sender, FilterEventArgs e)
        {
            if (!(e.Item is Email email) || string.IsNullOrWhiteSpace(_emailFilterText))
            return;

            if (!email.Name.ToUpper().Contains(_emailFilterText.ToUpper()) 
                && !email.EmailAddress.ToUpper().Contains(_emailFilterText.ToUpper()))
                e.Accepted = false;
        }

        #endregion


        private ObservableCollection<Email> _listEmails = new ObservableCollection<Email>();
        /// <summary>
        /// Список получателей
        /// </summary>
        public ObservableCollection<Email> ListEmails
        {
            get => _listEmails;
            set
            {
                Set(ref _listEmails, value);

                _listEmailsView = new CollectionViewSource {Source = value};
                _listEmailsView.Filter += ListEmailsViewOnFilter;
                _listEmailsView.SortDescriptions.Add(new SortDescription{PropertyName = "Name", Direction = ListSortDirection.Descending});

                RaisePropertyChanged(nameof(ListEmailsView));
            }
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

                ListEmailsView.Refresh();
            }
        }


        private void GetEmails()
        {
            ListEmails = _dataService.GetEmails();
            CurrentEmail = null;
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

        private string _tbSubject;
        private string _tpTimePicker;
        private DateTime? _calendarScheduler;
        private string _rtbBodyMail;


        public string TbSubject
        {
            get => _tbSubject;
            set
            {
                _tbSubject = value;

                Set(ref _tbSubject, value);

                if (EmailsTimeDictionaryEntrySet && EmailsTimeDictionaryEntry != null) 
                    EmailsTimeDictionaryEntry.Subject = value;

                RaisePropertyChanged();
            }
        }
        public string TpTimePicker
        {
            get => _tpTimePicker;
            set
            {
                _tpTimePicker = value;

                Set(ref _tpTimePicker, value);

                if (EmailsTimeDictionaryEntrySet && EmailsTimeDictionaryEntry != null)
                    EmailsTimeDictionaryEntry.TimeSend = value;

                RaisePropertyChanged();
            }
        }
        public DateTime? CalendarScheduler
        {
            get => _calendarScheduler;
            set
            {
                _calendarScheduler = value;

                Set(ref _calendarScheduler, value);

                if (EmailsTimeDictionaryEntrySet && EmailsTimeDictionaryEntry != null && value != null)
                    EmailsTimeDictionaryEntry.DataSend = (DateTime) value;

                RaisePropertyChanged();
            }
        }
        public string RtbBodyMail
        {
            get => _rtbBodyMail;
            set
            {
                _rtbBodyMail = value;

                Set(ref _rtbBodyMail, value);

                if (EmailsTimeDictionaryEntrySet && EmailsTimeDictionaryEntry != null)
                    EmailsTimeDictionaryEntry.Body = value;

                RaisePropertyChanged();
            }
        }


        public Dictionary<string, int> SmtpServerDictionary { get; } = new Dictionary<string, int>
        {
            {"smtp.yandex.ru" , 25 },
            {"smtp.mail.ru" , 25 },
            {"smtp.google.ru" , 25 }
        };
        public KeyValuePair<string, int> SmtpServerDictionaryEntry { get; set; }
        public Dictionary<string, string> SendersDictionary { get; } = new Dictionary<string, string>
        {
            {"SendSpamBot@yandex.ru" , EncrypterDll.Encrypter.Encrypt("P@$$w0rd") },
            {"SendSpamBot@yandex.ru1" , EncrypterDll.Encrypter.Encrypt("P@$$w0rd") },
            {"SendSpamBot@yandex.ru2" , EncrypterDll.Encrypter.Encrypt("P@$$w0rd") },
            {"SendSpamBot@yandex.ru3" , EncrypterDll.Encrypter.Encrypt("P@$$w0rd") },
            {"SendSpamBot@yandex.ru4" , EncrypterDll.Encrypter.Encrypt("P@$$w0rd") }
        };
        public KeyValuePair<string, string> SendersDictionaryEntry { get; set; }



        #region Планировщик отправки


        private ObservableCollection<EmailDataTimeTextClass> _emailsTimeDictionary;
        public ObservableCollection<EmailDataTimeTextClass> EmailsTimeDictionary
        {
            get => _emailsTimeDictionary;
            set
            {
                _emailsTimeDictionary = value;

                Set(ref _emailsTimeDictionary, value);

                RaisePropertyChanged();
            }
        }

        public bool EmailsTimeDictionaryEntrySet { get; set; }
        private EmailDataTimeTextClass _emailsTimeDictionaryEntry;
        public EmailDataTimeTextClass EmailsTimeDictionaryEntry
        {
            get => _emailsTimeDictionaryEntry;
            set
            {
                _emailsTimeDictionaryEntry = value;

                Set(ref _emailsTimeDictionaryEntry, value);

                EmailsTimeDictionaryEntrySet = false;

                CalendarScheduler = value?.DataSend;
                TpTimePicker = value?.TimeSend;
                TbSubject = value?.Subject;
                RtbBodyMail = value?.Body;

                EmailsTimeDictionaryEntrySet = true;

                RaisePropertyChanged();
            }
        }


        /// <summary>
        /// Метод, который превращает строку из текстбокса tbTimePicker в TimeSpan
        /// </summary>
        /// <param name="strSendTime"></param>
        /// <returns></returns>
        protected virtual TimeSpan GetSendTime​(string strSendTime)
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

        private MyRelayCommand _emailsTimeDictionaryAdd;
        public MyRelayCommand EmailsTimeDictionaryAddCommand
        {
            get
            {
                return _emailsTimeDictionaryAdd ??
                       (_emailsTimeDictionaryAdd = new MyRelayCommand(obj =>
                       {
                           //разбираем дату время
                           TimeSpan tsSendTime = GetSendTime​(TpTimePicker);
                           if (tsSendTime == new TimeSpan())
                           {
                               MessageErrorWindow("Некорректный формат даты");
                               return;
                           }
                           DateTime dtSenDateTime = CalendarScheduler.GetValueOrDefault().Date.Add(tsSendTime);
                           if (dtSenDateTime < DateTime.Now)
                           {
                               MessageErrorWindow("Дата и время отправки писем не могут быть раньше, чем настоящее время");
                               return;
                           }

                           if (!Check(RtbBodyMail, "null", "null", TbSubject)) return;

                           if (EmailsTimeDictionary.Any(x=>x.DataSend.Equals(dtSenDateTime)))
                           {
                               MessageErrorWindow("Такие дата и время уже есть");
                               return;
                           }

                           //формируем текст письма
                           EmailDataTimeTextClass objClass = new EmailDataTimeTextClass
                           {
                               DataSend = dtSenDateTime.Date, 
                               TimeSend = new DateTime(1,1,1, dtSenDateTime.TimeOfDay.Hours, 
                                   dtSenDateTime.TimeOfDay.Minutes,0).ToShortTimeString(), 
                               Subject = TbSubject, 
                               Body = RtbBodyMail
                           };

                           EmailsTimeDictionary.Add(objClass);

                           EmailsTimeDictionaryEntry = null;
                           TbSubject = null;
                           RtbBodyMail = null;
                           CalendarScheduler = DateTime.Now;
                           TpTimePicker = DateTime.Now.ToShortTimeString();
                       }));
            }
        }


        private MyRelayCommand _emailsTimeDictionaryRemove;
        public MyRelayCommand EmailsTimeDictionaryRemoveCommand
        {
            get
            {
                return _emailsTimeDictionaryRemove ??
                       (_emailsTimeDictionaryRemove = new MyRelayCommand(obj =>
                       {
                           if (obj is EmailDataTimeTextClass objClass)
                           {
                               EmailsTimeDictionary.Remove(objClass);

                               EmailsTimeDictionaryEntry = null;
                               TbSubject = null;
                               RtbBodyMail = null;
                           }

                       }));
            }
        }


        #endregion



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

                    EmailsTimeDictionary = new ObservableCollection<EmailDataTimeTextClass>();
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
                                   EmailSendServiceClass emailSender = new EmailSendServiceClass(strLogin, strPass, strMailAddressFrom, 
                                       strSubject, strBody, true, strServerName, serverPort);

                                   if (emailSender.SendMails(listEmails))
                                   {
                                       if (EmailsTimeDictionaryEntry != null)
                                           EmailsTimeDictionary.Remove(EmailsTimeDictionaryEntry);

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
                           string strServerName = SmtpServerDictionaryEntry.Key;
                           int serverPort = SmtpServerDictionaryEntry.Value;
                           SchedulerClass sc = new SchedulerClass();

                           if (!Check("null", strLogin, strPass, "null")) return;

                           //подготовка списка
                           List<string> listEmails = new List<string>();
                           foreach (Email itemEmail in ListEmails)
                           {
                               listEmails.Add(itemEmail.EmailAddress);
                           }

                           if (listEmails.Count > 0)
                           {
                               sc.EventTimerTick += EventTimerTick;
                               sc.TimerSendEmails​Start(EmailsTimeDictionary, listEmails, strLogin, strPass, strMailAddressFrom, strServerName, serverPort);

                               string strDate = String.Empty;
                               foreach (EmailDataTimeTextClass objClass in EmailsTimeDictionary)
                               {
                                   strDate += $"{objClass.DataSend.ToShortDateString()} {objClass.TimeSend}" + Environment.NewLine;
                               }

                               MessageInfoWindow($"Запущена оправка по расписанию{Environment.NewLine}почта будет отправлена в{Environment.NewLine}{strDate}");
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