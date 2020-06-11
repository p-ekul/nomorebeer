using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

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

        public decimal TotalSum { get; private set; }

        public decimal BuyAmount { get; set; }
        


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
            TotalSum = Trades.Sum(x => x.DailyInvest);
            BuySum = Trades.Sum(x => x.BuyValue);
            SellSum = Trades.Sum(x => x.SellValue);
            BuyAmount = Trades.Sum(x => x.BuyAmount);

            //Rate = Trades.Sum(x => 1 - x.BuyAmount);

        }


        public override string ToString()
        {
            
            StringWriter writer = new StringWriter();
            foreach (var trade in Trades)
            {
                writer.WriteLine(trade);
            }
            
            //writer.WriteLine($"\n {SellSum:N0} - {TotalSum:N0} - {BuySum:N0} = {SellSum - BuySum:N0} ({Rate:P2})");
            writer.WriteLine($"\n총 투자금액 : {TotalSum:N0}");
            writer.WriteLine($"총 주식 수 : {BuyAmount:N2}");
            SellSum = 55000 * BuyAmount;
            writer.WriteLine($"판매금액 : {SellSum:N0}");
            writer.WriteLine($"수익률 : {SellSum/TotalSum:P2}");

            return writer.ToString();
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
            trade.BuyAmount = trade.DailyInvest / price.Value;


            Trades.Add(trade);
        }
    }
}