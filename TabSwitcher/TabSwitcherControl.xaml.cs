using System.Windows;

namespace TabSwitcher
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class TabSwitcherControl
    {
        public TabSwitcherControl()
        {
            InitializeComponent();
        }

        public event RoutedEventHandler Back;
        public event RoutedEventHandler Forward;

        private void ButtonBackvard_OnClick(object sender, RoutedEventArgs e) => Back?.Invoke(this, new RoutedEventArgs());
        private void ButtonForward_OnClick(object sender, RoutedEventArgs e) => Forward?.Invoke(this, new RoutedEventArgs());

    }
}
