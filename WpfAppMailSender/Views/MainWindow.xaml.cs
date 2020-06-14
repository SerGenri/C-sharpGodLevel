using System.Windows;

namespace WpfAppMailSender.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow() => InitializeComponent();


        private void ButtonBackvard_OnClick(object sender, RoutedEventArgs e)
        {
            if (MainTabControl.SelectedIndex > 0)
            {
                MainTabControl.SelectedIndex--;
            }
        }

        private void ButtonForward_OnClick(object sender, RoutedEventArgs e)
        {
            if (MainTabControl.SelectedIndex < MainTabControl.Items.Count)
            {
                MainTabControl.SelectedIndex++;
            }
        }
    }
}
