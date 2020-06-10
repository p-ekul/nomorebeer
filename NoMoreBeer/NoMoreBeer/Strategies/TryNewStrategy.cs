using NoMoreBeer.Strategies;
using System.Collections.Generic;

namespace NoMoreBeer
{
    public class TryNewStrategy : Strategy
    {
        protected override void Buy(List<Price> prices)
        {
            for (int i = 0; i < 100; i ++)
            {
                BuyOne(prices[i]);
            }
        }
    }
}