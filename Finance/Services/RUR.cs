using HtmlAgilityPack;
using System.Linq;

namespace Finance
{
    public class RUR
    {
        public static string GetUSDRatio()
        {
            var web = new HtmlWeb();
            var doc = web.Load("http://www.profinance.ru/currency_usd.asp");
            var nodes = doc.DocumentNode.SelectNodes("//td[@class='cell'][@align='center'][@colspan='2']/font[@color='Red']/b");

            return nodes.First().InnerText;
        }
    }
}
