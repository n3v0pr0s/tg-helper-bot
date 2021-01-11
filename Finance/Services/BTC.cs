using HtmlAgilityPack;
using System.Linq;

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
