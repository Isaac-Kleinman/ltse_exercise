using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ltseExercise
{
    public class Order
    {
        public DateTime Timestamp { get; set; }
        public string Broker  { get; set; }
        public string SequenceId { get; set; }
        public string Type { get; set; }
        public string Symbol { get; set; }
        public int? Quantity { get; set; }
        public double? Price { get; set; }
        public string Side { get; set; }
    }
}
