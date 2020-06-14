using System.Windows;

namespace WpfAppMailSender.Views
{
    /// <summary>
    /// Логика взаимодействия для WindowSendEnd.xaml
    /// </summary>
    public partial class WindowSendEnd : Window
    {
        public WindowSendEnd(string message)
        {
            InitializeComponent();

            TxtMessage.Text = message;
        }

        private void BtnOk_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
