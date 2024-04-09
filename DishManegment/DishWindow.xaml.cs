using DishManegment.Services;
using Menu.REST.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DishManegment
{
    /// <summary>
    /// Interaction logic for DishWindow.xaml
    /// </summary>
    public partial class DishWindow : Window
    {
        private readonly DishService _dishService;
        private DishDTO _dish;
        public DishWindow(DishDTO dish =null)
        {
            InitializeComponent();
            _dishService = new DishService();
            _dish = dish;
            if (_dish != null)
            {
                // Populate fields with existing dish info
                NameTextBox.Text = _dish.Name;
                DescriptionTextBox.Text = _dish.Description;
                PriceTextBox.Text = _dish.Price.ToString();
                QtyTextBox.Text = _dish.Quantity.ToString();
            }
        }

        private void CancelBikeButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private async void SaveBikeButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) ||
            string.IsNullOrWhiteSpace(DescriptionTextBox.Text) ||
            string.IsNullOrWhiteSpace(PriceTextBox.Text) ||
            string.IsNullOrWhiteSpace(QtyTextBox.Text))
            {
                MessageBox.Show("All fields are required.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (_dish == null)
            {
                _dish = new DishDTO();
            }
            _dish.Name = NameTextBox.Text;
            _dish.Description = DescriptionTextBox.Text;
            _dish.Price = decimal.Parse(PriceTextBox.Text);
            _dish.Quantity = int.Parse(QtyTextBox.Text);

            if (_dish.Id == Guid.Empty)
            {
                await _dishService.AddDish(_dish);
            }
            else
            {
                await _dishService.UpdateDish(_dish.Id, _dish);
            }

            DialogResult = true;
        }

        private void QtyTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //Regex regex = new Regex("[^0-9]+");
            //e.Handled = regex.IsMatch(e.Text);
        }
    }
}
