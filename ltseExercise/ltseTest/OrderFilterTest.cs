using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ltseExercise;

namespace ltseExerciseTest
{
    public class OrderFilterTest
    {
        Order CreateOrder()
        {
            return new Order
            {
                Timestamp = DateTime.Now,
                Broker = string.Empty,
                SequenceId = string.Empty,
                Type = string.Empty,
                Symbol = string.Empty,
                Quantity = 0,
                Price = 0,
                Side = string.Empty
            };

        }

        [Fact]
        public void ForeignSymbolOrder_Invalidated()
        {
            var symbols = new List<string> { "ABC" };

            var order = CreateOrder();
            order.Symbol = "XYZ";

            var orders = new List<Order> { order };

            var filter = new OrderFilter(symbols, orders);
            filter.FilterOrders();

            Assert.Empty(filter.ValidOrders);
            Assert.NotEmpty(filter.InvalidOrders);
        }

        [Fact]
        public void ExchangeSymbolOrder_Validated()
        {
            var symbols = new List<string> { "ABC" };

            var order = CreateOrder();
            order.Symbol = "ABC";

            var orders = new List<Order> { order };

            var filter = new OrderFilter(symbols, orders);
            filter.FilterOrders();

            Assert.Empty(filter.InvalidOrders);
            Assert.NotEmpty(filter.ValidOrders);
        }

        [Fact]
        public void MissingValue_Invalidated()
        {
            var order = CreateOrder();
            order.Symbol = "ABC";
            order.SequenceId = null;

            var orders = new List<Order> { order };

            var symbols = new List<string> { "ABC" };

            var filter = new OrderFilter(symbols, orders);
            filter.FilterOrders();

            Assert.Empty(filter.ValidOrders);
            Assert.NotEmpty(filter.InvalidOrders);
        }

        [Fact]
        public void SingleBroker_DuplicateId_InvalidateDuplicate()
        {
            var order1 = CreateOrder();
            order1.Symbol = "ABC";
            order1.Timestamp = DateTime.Now;
            order1.Broker = "Broker";
            order1.SequenceId = "1";

            var order2 = CreateOrder();
            order2.Symbol = "ABC";
            order2.Timestamp = DateTime.Now.AddSeconds(1);
            order2.Broker = "Broker";
            order2.SequenceId = "1";

            var orders = new List<Order> { order1, order2 };

            var symbols = new List<string> { "ABC" };

            var filter = new OrderFilter(symbols, orders);
            filter.FilterOrders();

            Assert.Same(filter.ValidOrders.Single(), order1);
            Assert.Same(filter.InvalidOrders.Single(), order2);
        }

        [Fact]
        public void MultipleBroker_DuplicateId_ValidateDuplicate()
        {
            var order1 = CreateOrder();
            order1.Symbol = "ABC";
            order1.Timestamp = DateTime.Now;
            order1.Broker = "Broker";
            order1.SequenceId = "1";

            var order2 = CreateOrder();
            order2.Symbol = "ABC";
            order2.Timestamp = DateTime.Now.AddSeconds(1);
            order2.Broker = "Other Broker";
            order2.SequenceId = "1";

            var orders = new List<Order> { order1, order2 };

            var symbols = new List<string> { "ABC" };

            var filter = new OrderFilter(symbols, orders);
            filter.FilterOrders();

            Assert.Empty(filter.InvalidOrders);
            Assert.Equal(2, filter.ValidOrders.Count);
        }

        [Fact]
        public void _4_Orders_1_Minute_4thInvalidated()
        {
            var baseDate = new DateTime(2018, 1, 1, 1, 1, 1);

            var order1 = CreateOrder();
            order1.Symbol = "ABC";
            order1.Timestamp = baseDate;
            order1.Broker = "Broker";
            order1.SequenceId = "1";

            var order2 = CreateOrder();
            order2.Symbol = "ABC";
            order2.Timestamp = baseDate.AddSeconds(1);
            order2.Broker = "Broker";
            order2.SequenceId = "2";

            var order3 = CreateOrder();
            order3.Symbol = "ABC";
            order3.Timestamp = baseDate.AddSeconds(2);
            order3.Broker = "Broker";
            order3.SequenceId = "3";

            var order4 = CreateOrder();
            order4.Symbol = "ABC";
            order4.Timestamp = baseDate.AddSeconds(3);
            order4.Broker = "Broker";
            order4.SequenceId = "4";

            var orders = new List<Order>
            {
                order1, order2, order3, order4
            };

            var symbols = new List<string> { "ABC" };

            var filter = new OrderFilter(symbols, orders);
            filter.FilterOrders();

            Assert.Same(filter.InvalidOrders.Single(), order4);
            Assert.Equal(3, filter.ValidOrders.Count);
        }

        [Fact]
        public void _5_Orders_1_Minute_4_and_5_Invalidated()
        {
            var baseDate = new DateTime(2018, 1, 1, 1, 1, 1);

            var order1 = CreateOrder();
            order1.Symbol = "ABC";
            order1.Timestamp = baseDate;
            order1.Broker = "Broker";
            order1.SequenceId = "1";

            var order2 = CreateOrder();
            order2.Symbol = "ABC";
            order2.Timestamp = baseDate.AddSeconds(1);
            order2.Broker = "Broker";
            order2.SequenceId = "2";

            var order3 = CreateOrder();
            order3.Symbol = "ABC";
            order3.Timestamp = baseDate.AddSeconds(2);
            order3.Broker = "Broker";
            order3.SequenceId = "3";

            var order4 = CreateOrder();
            order4.Symbol = "ABC";
            order4.Timestamp = baseDate.AddSeconds(3);
            order4.Broker = "Broker";
            order4.SequenceId = "4";

            var order5 = CreateOrder();
            order5.Symbol = "ABC";
            order5.Timestamp = baseDate.AddSeconds(4);
            order5.Broker = "Broker";
            order5.SequenceId = "5";

            var orders = new List<Order>
            {
                order1, order2, order3, order4, order5
            };

            var symbols = new List<string> { "ABC" };

            var filter = new OrderFilter(symbols, orders);
            filter.FilterOrders();

            Assert.Collection(filter.InvalidOrders,
                order => Assert.Same(order4, order),
                order => Assert.Same(order5, order));

            Assert.Equal(3, filter.ValidOrders.Count);
        }
    }
}
