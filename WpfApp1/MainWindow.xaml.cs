using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<bool> primeTable = [];
        public MainWindow()
        {
            InitializeComponent();
            buildPrimeTable();
        }

        private void buildPrimeTable()
        {
            for (int i = 1; i <= 1e7; i++)
            {
                primeTable.Add(true);
            }
            primeTable[1] = false;

            for (int i = 2; i < 1e7; i++)
            {
                if (!primeTable[i])
                {
                    continue;
                }
                for (int j = 2; i * j < 1e7; j++)
                {
                    primeTable[i * j] = false;
                }
            }
        }

        private void CalculatePrimes(object sender, RoutedEventArgs e)
        {
            int number = 0;
            if (!int.TryParse(MyTextBox.Text, out number))
            {
                MyTextBlock.Text = "請輸入數字";
                return;
            }
            if (number < 2)
            {
                MyTextBlock.Text = "請輸入大於等於2的數字";
                return;
            }

            MyTextBlock.Text = "小於等於 " + number + " 的質數有:\n";
            for (int i = 2; i <= number; i++)
            {
                if (primeTable[i])
                {
                    MyTextBlock.Text += i + " ";
                }
            }
        }
    }
}