using System.Collections.Generic;

namespace ltseExercise
{
    class Program
    {
        static void Main(string[] args)
        {
            var filename = args[0];

            var io = new IO();

            var orders = io.ReadOrders(filename);

            var symbols = new List<string>
            {
                "BARK",
                "CARD",
                "HOOF",
                "LOUD",
                "GLOO",
                "YLLW",
                "BRIC",
                "KRIL",
                "LGHT",
                "VELL",
            };

            var filter = new OrderFilter(symbols, orders);
            filter.FilterOrders();

            io.GenerateListFiles(filter.ValidOrders, "accepted.txt");
            io.GenerateListFiles(filter.InvalidOrders, "rejected.txt");

            io.GenerateJsonFiles(filter.ValidOrders, "accepted.json");
            io.GenerateJsonFiles(filter.InvalidOrders, "rejected.json");


        }

    }
}
