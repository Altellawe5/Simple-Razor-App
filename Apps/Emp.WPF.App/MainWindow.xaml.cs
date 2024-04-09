using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Emp.WPF.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Emp.WPF.swaggerClient _swaggerClient;
        private HttpClient _httpClient;

        public MainWindow()
        {
            InitializeComponent();

            _httpClient = new HttpClient();
            _swaggerClient = new swaggerClient("https://localhost:7020", _httpClient);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var employees = _swaggerClient.EmployeesAllAsync();
            EmployeesListBox.ItemsSource = employees.Result.ToList().Select( e => e.Name);
        }
    }
}
