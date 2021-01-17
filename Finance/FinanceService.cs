using HtmlAgilityPack;
using System.Linq;

namespace Services
{
    public static class FinanceService
    {
        public static string GetBTCRatio()
        {
            var web = new HtmlWeb();
            var doc = web.Load("https://www.rbc.ru/crypto/currency/btcusd");
            var nodes = doc.DocumentNode.SelectNodes("//div[@class='chart__subtitle js-chart-value']/text()");

            return nodes.First().InnerText.Trim();
        }

        public static string GetRUBRatio()
        {
            var web = new HtmlWeb();
            var doc = web.Load("http://www.profinance.ru/currency_usd.asp");
            var nodes = doc.DocumentNode.SelectNodes("//td[@class='cell'][@align='center'][@colspan='2']/font[@color='Red']/b");

            return nodes.First().InnerText;
        }
    }
}
