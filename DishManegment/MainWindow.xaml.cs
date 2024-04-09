using DishManegment.Services;
using Menu.REST.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace DishManegment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DishService _dishService;


        public MainWindow()
        {
            InitializeComponent();
            _dishService = new DishService();
            
        }
        private async void LoadDishes()
        {
            var dishes = await _dishService.GetAllDishes();
            DishListView.ItemsSource = dishes;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            DishWindow window = new DishWindow();
            window.Owner = this;
            if (window.ShowDialog() == true)
            {
                LoadDishes();
            }
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            var selectedDish = (DishDTO)DishListView.SelectedItem;

            if (selectedDish != null)
            {
                await _dishService.DeleteDish(selectedDish.Id);

                LoadDishes();
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            var selectedDish = (DishDTO)DishListView.SelectedItem;

            DishWindow window = new DishWindow(selectedDish);
            window.Owner = this;
            if (window.ShowDialog() == true)
            {
                LoadDishes();
            }
        }

        private void GetAllDishesBTN_Click(object sender, RoutedEventArgs e)
        {
            GetAllDishesBTN.Visibility = Visibility.Hidden;
            DishListView.Visibility = Visibility.Visible;
            LoadDishes();
        }

        private void DishListView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;
            if (headerClicked != null)
            {
                string header = headerClicked.Column.Header as string;
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(DishListView.ItemsSource);

                // Clear previous sort descriptions
                view.SortDescriptions.Clear();

                // Add new sort description
                view.SortDescriptions.Add(new SortDescription(header, ListSortDirection.Ascending));

                // Apply sort
                view.Refresh();
            }
        }
    }
}
