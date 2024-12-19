using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersControl_V1
{
    public class OrderView
    {
        public int orderId { get; set; }
        public string ManufacturerName { get; set; }
        public string MarkName { get; set; }
        public int Diameter { get; set; }
        public int Wall { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public int status_id { get; set; }
        public string StatusName { get; set; }
        public int Volume { get; set; }
        public string Unit { get; set; }
    }
}
