using System;
using GalaSoft.MvvmLight;

namespace WpfAppMailSender
{
    public class EmailDataTimeTextClass : ViewModelBase
    {
        private DateTime _dataSend;
        private string _subject;
        private string _body;
        private string _timeSend;

        public DateTime DataSend
        {
            get => _dataSend;
            set
            {
                _dataSend = value;

                RaisePropertyChanged();
            }
        }


        public string TimeSend
        {
            get => _timeSend;
            set
            {
                _timeSend = value; 

                RaisePropertyChanged();
            }
        }

        public string Subject
        {
            get => _subject;
            set
            {
                _subject = value;

                RaisePropertyChanged();
            }
        }

        public string Body
        {
            get => _body;
            set
            {
                _body = value;

                RaisePropertyChanged();
            }
        }
    }
}
