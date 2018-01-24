using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ltseExercise
{
    public class IO
    {
        public List<Order> ReadOrders(string filename)
        {
            var orders = new List<Order>();

            var file = new StreamReader(filename);

            var fieldNames = file.ReadLine().Split(',');

            var fieldIndex = new Dictionary<string, int>();

            for (int i = 0; i < fieldNames.Length; i++)

            {
                fieldIndex[fieldNames[i]] = i;
            }

            var line = file.ReadLine();

            while (line != null)
            {
                var fields = line.Split(',');
                var order = new Order();

                order.Timestamp = DateTime.Parse(fields[fieldIndex["Time stamp"]]);

                var broker = fields[fieldIndex["broker"]];
                order.Broker = broker != String.Empty ? broker : null;

                var sequenceId = fields[fieldIndex["sequence id"]];
                order.SequenceId = sequenceId != string.Empty ? sequenceId : null;

                var type = fields[fieldIndex["type"]];
                order.Type = type != string.Empty ? type : null;

                var symbol = fields[fieldIndex["Symbol"]];
                order.Symbol = symbol != string.Empty ? symbol : null;

                var side = fields[fieldIndex["Side"]];
                order.Side = side != string.Empty ? side : null;

                int quantity;

                if (Int32.TryParse(fields[fieldIndex["Quantity"]], out quantity))
                {
                    order.Quantity = quantity;
                }
                else
                {
                    order.Quantity = null;
                }

                double price;

                if (double.TryParse(fields[fieldIndex["Price"]], out price))
                {
                    order.Price = price;
                }
                else
                {
                    order.Price = null;
                }

                orders.Add(order);

                line = file.ReadLine();
            }

            return orders;
        }

        public void GenerateListFiles(List<Order> orders, string filename)
        {
            var lines = orders
                .OrderBy(o => o.Timestamp)
                .Select(o => o.Broker + "," + o.SequenceId);

            File.WriteAllLines(filename, lines);
        }

        public void GenerateJsonFiles(List<Order> orders, string filename)
        {
            var json = JsonConvert.SerializeObject(orders);

            File.WriteAllText(filename, json);
        }
    }
}
