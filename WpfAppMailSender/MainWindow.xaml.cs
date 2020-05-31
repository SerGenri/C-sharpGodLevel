using System;
using System.Windows;
using System.Windows.Documents;

namespace WpfAppMailSender
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow() => InitializeComponent();

        private void Button_Click(object sebder, RoutedEventArgs e)
        {
            try
            {
                string bodyMail = new TextRange(BodyMail.Document.ContentStart, BodyMail.Document.ContentEnd).Text;

                var send = new EmailSendServiceClass();
                send.Send(LoginBox.Text, PasswordBox.Password, SubjectMail.Text, bodyMail);
            }
            catch (Exception error)
            {
                WindowMessageSendError end = new WindowMessageSendError(error.Message)
                {
                    Owner = Application.Current.MainWindow
                };
                end.ShowDialog();
            }
        }
    }
}
