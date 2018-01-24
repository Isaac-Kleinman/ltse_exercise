using System;
using System.Collections.Generic;
using System.Linq;

namespace ltseExercise
{
    public class OrderFilter
    {
        private readonly List<string> symbols;
        private readonly List<Order> orders;

        private List<Order> validOrders;
        private List<Order> invalidOrders;

        public List<Order> ValidOrders
        {
            get { return validOrders; }
        }

        public List<Order> InvalidOrders
        {
            get { return invalidOrders; }
        }

        public OrderFilter(List<string> symbols, List<Order> orders)
        {
            if (symbols == null)
            {
                throw new ArgumentNullException(nameof(symbols));
            }

            if (orders == null)
            {
                throw new ArgumentNullException(nameof(orders));
            }

            this.symbols = symbols;
            this.orders = orders;

            validOrders = new List<Order>();
            invalidOrders = new List<Order>();
        }

        public void FilterOrders()
        {
            RemoveOverThrottleLimit();
            RemoveMissingValues();
            RemoveForeignSymbols();
            RemoveDuplicates();

            validOrders = orders;
        }

        void RemoveOverThrottleLimit()
        {
            var groups = orders
                .GroupBy(o => o.Broker);

            foreach (var group in groups)
            {
                var groupOrder = group
                    .OrderBy(o => o.Timestamp)
                    .ToList();

                for (int i = 0; i < groupOrder.Count() - 3; i++)
                {
                   while ( i + 3 < groupOrder.Count() &&
                        (groupOrder[i+3].Timestamp - groupOrder[i].Timestamp).Minutes == 0)
                    {
                        var invalidOrder = groupOrder[i + 3];

                        invalidOrders.Add(invalidOrder);
                        orders.Remove(invalidOrder);
                        groupOrder.Remove(invalidOrder);
                    }
                }
            }
        }

        private void RemoveMissingValues()
        {
            var ordersMissingValues = orders
                .Where(o => o.Broker == null ||
                    o.Symbol == null ||
                    o.Type == null ||
                    o.Quantity == null ||
                    o.SequenceId == null ||
                    o.Side == null ||
                    o.Price == null)
                    .ToList();

            foreach (var order in ordersMissingValues)
            {
                orders.Remove(order);
            }

            invalidOrders.AddRange(ordersMissingValues);
        }

        private void RemoveForeignSymbols()
        {
            var ordersWithForeignSymbols = orders
                .Where(o => !symbols.Contains(o.Symbol))
                .ToList();

            foreach (var order in ordersWithForeignSymbols)
            {
                orders.Remove(order);
            }

            invalidOrders.AddRange(ordersWithForeignSymbols);
        }

        private void RemoveDuplicates()
        {
            var groups = orders
                 .GroupBy(o => new
                 {
                     broker = o.Broker,
                     id = o.SequenceId
                 });

            foreach (var group in groups)
            {
                var groupOrder = group
                    .OrderBy(o => o.Timestamp)
                    .Skip(1);

                foreach (var order in groupOrder)
                {
                    invalidOrders.Add(order);
                    orders.Remove(order);
                }
            }
        }
    }
}
