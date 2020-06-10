using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NoMoreBeer.Strategies
{
    public abstract class Strategy
    {
        protected Strategy()
        {
            Trades = new List<Trade>();
        }

        protected List<Trade> Trades { get; }

        public decimal BuySum { get; private set; }
        public decimal SellSum { get; private set; }
        public decimal Rate { get; private set; }

        public override string ToString()
        {
            StringWriter writer = new StringWriter();
            foreach (var trade in Trades)
            {
                writer.WriteLine(trade);
            }

            writer.WriteLine($"{SellSum:N0} - {BuySum:N0} = {SellSum - BuySum:N0} ({Rate:P2})");

            return writer.ToString();
        }

        internal void Trade(List<Price> prices)
        {
            // 하루에 한 주씩 매수한다.
            Buy(prices);


            // 마지막 날의 가격으로 매수한 주식을 모두 매도한다.
            Price lastPrice = prices[prices.Count - 1];
            foreach (var trade in Trades)
            {
                trade.SellValue = lastPrice.Value;
                trade.SellOn = lastPrice.Date;
                trade.Rate = trade.BuyValue.GetRate(trade.SellValue);
            }

            // 총매수액 등을 구한다.
            BuySum = Trades.Sum(x => x.BuyValue);
            SellSum = Trades.Sum(x => x.SellValue);
            Rate = BuySum.GetRate(SellSum);
        }

        /// <summary>
        /// 조건에 맞는 주식을 매수한다.
        /// </summary>
        /// <param name="prices"></param>
        protected abstract void Buy(List<Price> prices);

        protected void BuyOne(Price price)
        {
            Trade trade = new Trade();
            trade.BuyOn = price.Date;
            trade.BuyValue = price.Value;

            Trades.Add(trade);
        }
    }
}