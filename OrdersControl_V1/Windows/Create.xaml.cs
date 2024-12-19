using OrdersControl_V1.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace OrdersControl_V1
{
    public partial class Create : Window
    {
        private OrderControlEntities dbContext;
        private List<Manufacturer> manufacturers;
        private List<Status> statuses;
        private List<Mark> marks;

        public Create()
        {
            InitializeComponent();
            dbContext = new OrderControlEntities();
            manufacturers = dbContext.Manufacturer.ToList();
            statuses = dbContext.Status.ToList();
            marks = dbContext.Mark.ToList();
            manufacturerComboBox.ItemsSource = manufacturers;
            manufacturerComboBox.SelectedValuePath = "id";
            manufacturerComboBox.DisplayMemberPath = "name";
            statusComboBox.ItemsSource = statuses;
            statusComboBox.SelectedValuePath = "id";
            statusComboBox.DisplayMemberPath = "name";
            markComboBox.ItemsSource = marks;
            markComboBox.SelectedValuePath = "id";
            markComboBox.DisplayMemberPath = "name";
        }

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(diameterTextBox.Text, out int diameter) || !int.TryParse(wallTextBox.Text, out int wall) || !int.TryParse(volumeTextBox.Text, out int volume))
            {
                MessageBox.Show("Некорректные данные. Введите целые числа.");
                return;
            }
            Orders newOrder = new Orders
            {
                diametr = diameter,
                wall = wall,
                manufacture_id = (int)manufacturerComboBox.SelectedValue,
                start_date = startDatePicker.SelectedDate.Value,
                end_date = endDatePicker.SelectedDate.Value,
                status_id = (int)statusComboBox.SelectedValue,
                mark_id = (int)markComboBox.SelectedValue
            };
            Position newPosition = new Position
            {
                volume = volume,
                unit = unitTextBox.Text
            };
            newOrder.Position = new List<Position> { newPosition };
            try
            {
                dbContext.Orders.Add(newOrder);
                dbContext.SaveChanges();
                MessageBox.Show("Запись успешно создана");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void backToMenu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
