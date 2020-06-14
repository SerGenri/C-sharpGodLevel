using System.Windows;

namespace WpfAppMailSender.Views
{
    /// <summary>
    /// Логика взаимодействия для WindowMessageSendError.xaml
    /// </summary>
    public partial class WindowMessageSendError : Window
    {
        public WindowMessageSendError(string errorMessage)
        {
            InitializeComponent();

            TxtSendError.Text = errorMessage;
        }

        private void BtnOk_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
