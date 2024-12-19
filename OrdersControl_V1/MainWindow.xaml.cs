using OrdersControl_V1.Model;
using OrdersControl_V1.Windows;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OrdersControl_V1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<OrderView> OrderData { get; set; }
        private OrderControlEntities dbContext;

        public MainWindow()
        {
            InitializeComponent();
            dbContext = new OrderControlEntities();
            LoadOrderData();
            DataContext = this;
        }

        private void LoadOrderData()
        {
            OrderData = dbContext.Orders
                .Include(o => o.Manufacturer)
             .Include(o => o.Mark)
             .Include(o => o.Status)
               .Include(o => o.Position)
            .Select(order => new OrderView
            {
                orderId = order.id,
                ManufacturerName = order.Manufacturer.name,
                MarkName = order.Mark.name,
                Diameter = order.diametr,
                Wall = order.wall,
                start_date = order.start_date,
                end_date = order.end_date,
                StatusName = order.Status.name,
                status_id = order.status_id,
            }).ToList();
            orderData.ItemsSource = OrderData;
        }

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            Create create = new Create();
            create.Show();
            this.Close();
        }


        private void Search()
        {
            string searchText = SearchBox.Text.Trim();

            var allOrders = dbContext.Orders
                .Include(o => o.Manufacturer)
                .Include(o => o.Mark)
                .Select(order => new OrderView
                {
                    orderId = order.id,
                    ManufacturerName = order.Manufacturer.name,
                    MarkName = order.Mark.name,
                    Diameter = order.diametr,
                    Wall = order.wall,
                    start_date = order.start_date,
                    end_date = order.end_date,
                    StatusName = order.Status.name,
                    status_id = order.status_id
                });

            IQueryable<OrderView> filteredOrders = allOrders;

            if (!string.IsNullOrEmpty(searchText))
            {
                filteredOrders = filteredOrders.Where(order =>
                    order.ManufacturerName.Contains(searchText) ||
                    order.MarkName.Contains(searchText) ||
                    order.Diameter.ToString().Contains(searchText) ||
                    order.Wall.ToString().Contains(searchText)
                );
            }

            OrderData = filteredOrders.ToList();
            orderData.ItemsSource = OrderData;
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            OrderView selectedOrderView = orderData.SelectedItem as OrderView;
            if (selectedOrderView == null)
            {
                MessageBox.Show("Выберите заказ для редактирования.");
                return;
            }

            var selectedOrder = dbContext.Orders.Find(selectedOrderView.orderId);
            if (selectedOrder == null)
            {
                MessageBox.Show("Не удалось найти выбранный заказ в базе данных.");
                return;
            }

            var manufacturers = dbContext.Manufacturer.ToList();
            var statuses = dbContext.Status.ToList();
            var marks = dbContext.Mark.ToList();

            Edit editWindow = new Edit(selectedOrder, manufacturers, statuses, marks);

            bool? result = editWindow.ShowDialog();

            if (result == true)
            {
                try
                {
                    dbContext.Entry(selectedOrder).State = EntityState.Modified;
                    if (selectedOrder.Position?.FirstOrDefault() != null)
                    {
                        dbContext.Entry(selectedOrder.Position?.FirstOrDefault()).State = EntityState.Modified;
                    }
                    dbContext.SaveChanges();
                    MessageBox.Show("Изменения успешно сохранены!");
                    LoadOrderData();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        var currentValues = entry.CurrentValues;
                        var databaseValues = entry.GetDatabaseValues();
                        Debug.WriteLine($"Current values: {currentValues}");
                        Debug.WriteLine($"Database values: {databaseValues}");
                    }
                    throw;
                }
            }
        }


        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            {
                OrderView selectedOrderView = orderData.SelectedItem as OrderView;
                if (selectedOrderView == null)
                {
                    MessageBox.Show("Выберите заказ для удаления.");
                    return;
                }

                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить выбранный заказ?", "Подтверждение ", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        var selectedOrder = dbContext.Orders.Find(selectedOrderView.orderId);
                        if (selectedOrder != null)
                        {
                            var positionsToDelete = dbContext.Position.Where(p => p.order_id == selectedOrder.id).ToList();
                            dbContext.Position.RemoveRange(positionsToDelete);

                            dbContext.Orders.Remove(selectedOrder);
                            dbContext.SaveChanges();
                            MessageBox.Show("Заказ удален");
                            LoadOrderData();
                        }
                        else
                        {
                            MessageBox.Show("Заказ не найден в базе данных.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении заказа: {ex.Message}");
                    }
                }
            }
        }
    }
}

