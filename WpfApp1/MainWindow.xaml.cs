
using Microsoft.Win32;
using System.IO;
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
        private Dictionary<string, int> _menuItems = new Dictionary<string, int>();

        public MainWindow()
        {
            InitializeComponent();
            order.Text = "";
            ReadItems();
        }

        private void ReadItems()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "CSV files (*.csv)|*.csv";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                openFileDialog.Title = "請選擇商品 CSV 檔";
                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog() == true)
                {
                    using (var reader = new StreamReader(openFileDialog.FileName, Encoding.Default))
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            var values = line.Split(',');
                            _menuItems.Add(values[0], Convert.ToInt32(values[1]));
                        }
                    }
                }
                addMenuItemsToPanel();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"檔案讀取錯誤，錯誤訊息：${ex.Message})");
            }
        }



        private void addMenuItemsToPanel()
        {
            menu.Children.Clear();
            foreach (var item in _menuItems)
            {
                var checkbox = new CheckBox
                {
                    Content = $"{item.Key} ${item.Value}",
                    Margin = new Thickness(5),
                    FontSize = 18,
                    FontWeight = FontWeights.Bold,
                    FontFamily = new FontFamily("微軟正黑體"),

                };
                var slideBar = new Slider
                {
                    Minimum = 0,
                    Maximum = 10,
                    Value = 0,
                    Width = 200,
                    Margin = new Thickness(5),
                    IsSnapToTickEnabled = true,
                    TickFrequency = 1,
                    TickPlacement = System.Windows.Controls.Primitives.TickPlacement.BottomRight
                };
                var binding = new Binding("Value")
                {
                    Source = slideBar,
                    Mode = BindingMode.OneWay
                };
                var slideBarLabel = new Label
                {
                    FontSize = 18,
                    FontWeight = FontWeights.Bold,
                    FontFamily = new FontFamily("微軟正黑體"),
                    Margin = new Thickness(5)
                };
                slideBarLabel.SetBinding(ContentProperty, binding);
                var smallStackPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };
                smallStackPanel.Children.Add(checkbox);
                smallStackPanel.Children.Add(slideBar);
                smallStackPanel.Children.Add(slideBarLabel);
                menu.Children.Add(smallStackPanel);
            }
        }

        private void SaveOrder(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Text files (*.txt)|*.txt";
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                saveFileDialog.AddExtension = true;
                saveFileDialog.DefaultExt = "txt";
                saveFileDialog.FileName = "訂單";
                saveFileDialog.Title = "請選擇儲存位置";
                if (saveFileDialog.ShowDialog() == true)
                {
                    using (var writer = new StreamWriter(saveFileDialog.FileName))
                    {
                        writer.Write(order.Text);
                        string date = DateTime.Now.ToString("d");
                        writer.Write($"訂單日期：{date}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"檔案儲存錯誤，錯誤訊息：${ex.Message})");
            }
        }

        private void place(object sender, RoutedEventArgs e)
        {
            order.Text = "";
            if (chs.IsChecked == true)
            {
                order.Text += "內用\n";
            }
            else
            {
                order.Text += "外帶\n";
            }
            int total = 0;
            foreach (StackPanel item in menu.Children)
            {
                foreach (var child in item.Children)
                {
                    if (child is CheckBox)
                    {
                        var checkbox = (CheckBox)child;
                        var slideBar = (Slider)item.Children[1];
                        if (checkbox.IsChecked == true && slideBar.Value != 0)
                        {
                            total += (int)slideBar.Value * _menuItems[checkbox.Content.ToString().Split(' ')[0]];
                            order.Text += $"{checkbox.Content} {((Label)item.Children[2]).Content}杯\n";
                        }
                    }
                }
            }
            order.Text += $"總金額: ${total}\n";
            if (total > 300)
            {
                total = Convert.ToInt32(Convert.ToDouble(total) * 0.9);
                order.Text += $"(滿300元打9折)，實際支付：${total}";
            }
        }
    }
}