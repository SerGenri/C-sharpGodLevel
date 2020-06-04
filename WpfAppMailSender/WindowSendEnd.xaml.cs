using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfAppMailSender
{
    /// <summary>
    /// Логика взаимодействия для WindowSendEnd.xaml
    /// </summary>
    public partial class WindowSendEnd : Window
    {
        public WindowSendEnd()
        {
            InitializeComponent();
        }

        private void BtnOk_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
