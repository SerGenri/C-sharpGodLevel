using System.Collections;
using System.Windows;


namespace ToolBarSender
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class ToolBarSenderControl
    {
        private string _lblText;
        private object _cbItemSourece;

        public ToolBarSenderControl()
        {
            InitializeComponent();
        }

        public event RoutedEventHandler BtnAdd;
        public event RoutedEventHandler BtnEdit;
        public event RoutedEventHandler BtnDelete;

        public object CbItemSourece
        {
            get => _cbItemSourece;
            set
            {
                _cbItemSourece = value;

                CbSenderSelect.ItemsSource = (IEnumerable) value;
                CbSenderSelect.DisplayMemberPath = "Key";
                CbSenderSelect.SelectedValuePath = "Value";
                CbSenderSelect.SelectedIndex = 0;
            }
        }

        public string LblText
        {
            get => _lblText;
            set
            {
                _lblText = value;

                Label.Content = value;
            }
        }

        private void BtnAddSender_OnClick(object sender, RoutedEventArgs e) => BtnAdd?.Invoke(this, new RoutedEventArgs());

        private void BtnEditSender_OnClick(object sender, RoutedEventArgs e) => BtnEdit?.Invoke(this, new RoutedEventArgs());

        private void BtnDeleteSender_OnClick(object sender, RoutedEventArgs e) => BtnDelete?.Invoke(this, new RoutedEventArgs());
    }
}
