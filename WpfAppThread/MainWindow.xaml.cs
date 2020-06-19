using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace WpfAppThread
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Factorial(object numIn)
        {
            int num = Convert.ToInt32(numIn);

            long factorial = 1;
            string rezultText = String.Empty;

            for (int i = 1; i <= num; i++)
            {
                factorial *= i;
                if (i == num)
                {
                    rezultText += $"{i}";
                    PrinRezult(TbRezultFac, rezultText);
                }
                else
                {
                    rezultText += $"{i} * ";
                    PrinRezult(TbRezultFac, rezultText);
                }

                Thread.Sleep(200);
            }

            rezultText += $" = {factorial}";
            PrinRezult(TbRezultFac, rezultText);
        }

        private void SummNumm(object numIn)
        {
            int num = Convert.ToInt32(numIn);

            long summ = 0;
            string rezultText = String.Empty;

            for (int i = 1; i <= num; i++)
            {
                summ += i;
                if (i == num)
                {
                    rezultText += $"{i}";
                    PrinRezult(TbRezultSumm ,rezultText);
                }
                else
                {
                    rezultText += $"{i} + ";
                    PrinRezult(TbRezultSumm, rezultText);
                }

                Thread.Sleep(400);
            }

            rezultText += $" = {summ}";
            PrinRezult(TbRezultSumm, rezultText);
        }

        private void PrinRezult(TextBox tb, string rezultText)
        {
            Application.Current.Dispatcher?.BeginInvoke(new Action(() =>
            {
                tb.Text = rezultText;
            }));
        }

        private void BtnRun_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                int num = int.Parse(TbNum.Text);

                Thread thrFac = new Thread(Factorial);
                thrFac.Start(num);

                Thread thrSumm = new Thread(SummNumm);
                thrSumm.Start(num);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
