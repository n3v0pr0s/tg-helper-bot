using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Finance
{
    public class BTC
    {
        public static string GetUSDRatio()
        {
            var web = new HtmlWeb();
            var doc = web.Load("https://www.rbc.ru/crypto/currency/btcusd");
            var nodes = doc.DocumentNode.SelectNodes("//div[@class='chart__subtitle js-chart-value']/text()");

            return nodes.First().InnerText.Trim();
        }
    }
}
