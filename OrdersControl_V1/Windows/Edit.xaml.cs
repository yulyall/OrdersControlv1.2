using OrdersControl_V1.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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

namespace OrdersControl_V1.Windows
{
    public partial class Edit : Window
    {
        public Orders Order { get; set; }

        private List<Manufacturer> manufacturers;
        private List<Status> statuses;
        private List<Mark> marks;

        public Edit(Orders order, List<Manufacturer> manufacturers, List<Status> statuses, List<Mark> marks)
        {
            InitializeComponent();
            Order = order;

            this.manufacturers = manufacturers;
            this.statuses = statuses;
            this.marks = marks;

            // Производство
            manufacturerComboBox.ItemsSource = this.manufacturers;
            manufacturerComboBox.SelectedValuePath = "id";
            manufacturerComboBox.DisplayMemberPath = "name";
            manufacturerComboBox.SelectedValue = order.manufacture_id;

            // ДАты
            startDatePicker.SelectedDate = order.start_date;
            endDatePicker.SelectedDate = order.end_date;

            // Комбобокс статуса
            statusComboBox.ItemsSource = this.statuses;
            statusComboBox.SelectedValuePath = "id";
            statusComboBox.DisplayMemberPath = "name";
            statusComboBox.SelectedValue = order.status_id;

            // Комбобокс марки
            markComboBox.ItemsSource = this.marks;
            markComboBox.SelectedValuePath = "id";
            markComboBox.DisplayMemberPath = "name";
            markComboBox.SelectedValue = order.mark_id;


            diameterTextBox.Text = order.diametr.ToString();
            wallTextBox.Text = order.wall.ToString();
            volumeTextBox.Text = order.Position?.FirstOrDefault()?.volume.ToString() ?? "";
            unitTextBox.Text = order.Position?.FirstOrDefault()?.unit ?? "";
        }


        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(diameterTextBox.Text, out int diameter) || !int.TryParse(wallTextBox.Text, out int wall) || !int.TryParse(volumeTextBox.Text, out int volume))
            {
                MessageBox.Show("Некорректные данные");
                return;
            }
            Order.diametr = diameter;
            Order.wall = wall;
            Order.manufacture_id = (int)manufacturerComboBox.SelectedValue;
            Order.start_date = startDatePicker.SelectedDate.Value;
            Order.end_date = endDatePicker.SelectedDate.Value;
            Order.status_id = (int)statusComboBox.SelectedValue;
            Order.mark_id = (int)markComboBox.SelectedValue;
            var position = Order.Position?.FirstOrDefault();
            if (position != null)
            {
                position.volume = volume;
                position.unit = unitTextBox.Text;
            }
            DialogResult = true;
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
