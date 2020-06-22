using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfAppParallel
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            ProgressBar.Maximum = ArrSize - 1;
        }

        private const int ArrSize = 100;
        private const int Delay = 1;

        private Random rnd = new Random();
        private int[,] _arr1 = new int[ArrSize, ArrSize];
        private int[,] _arr2 = new int[ArrSize, ArrSize];
        private int[,] _arr3 = new int[ArrSize, ArrSize];

        private void PrinRezult(TextBox tb, string rezultText)
        {
            Application.Current.Dispatcher?.BeginInvoke(new Action(() =>
            {
                tb.Text = rezultText;
            }));
        }

        private void CreateArr(int[,] arr, TextBox tb, IProgress<int> progress)
        {
            string rezult = String.Empty;

            for (int i = 0; i < ArrSize; i++)
            {
                for (int j = 0; j < ArrSize; j++)
                {
                    arr[i, j] = rnd.Next(1,10);

                    string razrad = "";
                    if (arr[i, j] < 10)
                    {
                        razrad = "0";
                    }

                    rezult += $"{razrad}{arr[i, j]} ";

                    progress.Report(i);

                    Thread.Sleep(Delay);
                }

                rezult += "\n";
            }

            PrinRezult(tb, rezult);
        }

        private void MultiArr(int[,] arr1, int[,] arr2, int[,] arr3, TextBox tb, IProgress<int> progress)
        {
            string rezult = String.Empty;

            for (int i = 0; i < ArrSize; i++)
            {
                for (int j = 0; j < ArrSize; j++)
                {
                    arr3[i, j] = arr1[j, i] * arr2[j, i];

                    string razrad = "";
                    if (arr3[i, j] < 10)
                    {
                        razrad = "0";
                    }

                    rezult += $"{razrad}{arr3[i, j]} ";

                    progress.Report(i);

                    Thread.Sleep(Delay);
                }

                rezult += "\n";
            }
                     
            PrinRezult(tb, rezult);
        }
       
        public void EndTask(Task obj)
        {
            Application.Current.Dispatcher?.BeginInvoke(new Action(() =>
            {
                BtnStartMulti.IsEnabled = true;
                BtnStartCreate.IsEnabled = true;
            }));
        }


        private void BtnStartCreate_OnClick(object sender, RoutedEventArgs e)
        {
            BtnStartMulti.IsEnabled = false;
            BtnStartCreate.IsEnabled = false;

            IProgress<int> progress = new Progress<int>(i => ProgressBar.Value = i);

            Task task1 = new Task(() => CreateArr(_arr1, TbArr1, progress));
            task1.ContinueWith(EndTask);
            task1.Start();

            Task task2 = new Task(() => CreateArr(_arr2, TbArr2, progress));
            task2.ContinueWith(EndTask);
            task2.Start();
        }
        private void BtnStartMulti_OnClick(object sender, RoutedEventArgs e)
        {
            BtnStartMulti.IsEnabled = false;
            BtnStartCreate.IsEnabled = false;

            IProgress<int> progress = new Progress<int>(i => ProgressBar.Value = i);

            Task task = new Task(() => MultiArr(_arr1, _arr2, _arr3, TbArr3, progress));
            task.ContinueWith(EndTask);
            task.Start();
        }
    }
}
