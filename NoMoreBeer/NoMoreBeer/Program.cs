using NoMoreBeer.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NoMoreBeer
{
    class Program
    {
        const string StockName = "삼성전자";
        static readonly DateTime FromDate = DateTime.Today.AddYears(-10);

        static void Main(string[] args)
        {
            List<Price> prices = PriceRepository.Instance.Load(StockName, FromDate);

            //Strategy strategy = new OnePerDayStrategy();
            //Strategy strategy = new SimpleRateStrategy();
            Strategy strategy = new TryNewStrategy();



            strategy.Trade(prices);

            Console.WriteLine(strategy);
        }
    }
}
