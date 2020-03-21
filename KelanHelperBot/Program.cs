using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace KelanHelperBot
{
    class Program
    {
        private static ITelegramBotClient botClient;
        static void Main(string[] args)
        {
            botClient = new TelegramBotClient("822847399:AAHtD0vLdcTZtRas84-LWvvChIUNNPTK07w");
            var me = botClient.GetMeAsync().Result;
            Console.WriteLine($"Hello, World! I am user {me.Id} and my name is {me.FirstName}.");

            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
            Thread.Sleep(int.MaxValue);
        }

        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            try
            {
                if (e.Message.Text != null)
                {
                    Console.WriteLine($"Received a text message in chat {e.Message.Chat.Id}.");

                    switch (e.Message.Text)
                    {
                        case var message when message == "/help":
                            await SendMessage(e.Message.Chat, e.Message.MessageId, "Звоните 911!");
                            break;
                        case var message when message == "/hello":
                            await SendMessage(e.Message.Chat, e.Message.MessageId, "Привет!");
                            break;
                        case var message when message == "/rub":
                            await SendMessage(e.Message.Chat, e.Message.MessageId, GetRubRate());
                            break;
                        case var message when message.StartsWith("/random"):
                            await SendMessage(e.Message.Chat, e.Message.MessageId, GetRandom(e.Message.Text));
                            break;
                        case var message when message.StartsWith("/auto"):
                            await SendMessage(e.Message.Chat, e.Message.MessageId, GetHrefsFromAutoRu(e.Message.Text));
                            break;


                    }
                }
            }
            catch (Exception ex)
            {
                await SendMessage(e.Message.Chat, e.Message.MessageId, ex.Message);
            }
        }

        static async Task SendMessage(Chat chatId, int messageId, string message)
        {
            await botClient.SendTextMessageAsync(chatId: chatId, text: message, replyToMessageId: messageId);
        }

        //static async Task SendMessage(Chat chatId, int messageId, string[] messages)
        //{
        //    var result = string.Join("\n", messages);
        //    await botClient.SendTextMessageAsync(chatId: chatId, text: result, replyToMessageId: messageId);
        //}

        //Business logic

        static string GetRandom(string command)
        {
            var parts = command.Split(new char[] { ' ' });
            var rand = new Random();
            if (parts.Length == 2)
            {
                var number = int.TryParse(parts[1], out int num) ? num : throw new Exception($"Параметр '{parts[1]}' должен быть целым числом больше нуля, проверьте данные и повторите ввод");
                if (number < 1)
                {
                    throw new Exception($"Число должно быть больше нуля");
                }

                return rand.Next(number).ToString();
            }
            if (parts.Length == 3)
            {
                var from = int.TryParse(parts[1], out int num1) ? num1 : throw new Exception($"Параметр '{parts[1]}' должен быть целым числом больше нуля, проверьте данные и повторите ввод");
                var to = int.TryParse(parts[2], out int num2) ? num2 : throw new Exception($"Параметр '{parts[2]}' должен быть целым числом больше нуля, проверьте данные и повторите ввод");
                if (to <= from)
                {
                    throw new Exception("Второе число должно быть больше первого");
                }

                return rand.Next(from, to).ToString();
            }

            throw new Exception("Неверный вызов функции. Используйте формат: /random число или /random от до");
        }

        static string GetRubRate()
        {
            var web = new HtmlWeb();
            var doc = web.Load("http://www.profinance.ru/currency_usd.asp");
            var nodes = doc.DocumentNode.SelectNodes("//td[@class='cell'][@align='center'][@colspan='2']/font[@color='Red']/b");

            return nodes.First().InnerText;
        }


        static string GetHrefsFromAutoRu(string command)
        {
            var parts = command.Split(new char[] { ' ' });

            if (parts.Length < 3)
            {
                throw new Exception("Недостаточно параметров для поиска, повторите ввод");
            }

            var city = "orenburg";
            var takeCount = 3;
            var vendor = parts[1];
            var model = parts[2];

            if (parts.Length == 4)
            {
                city = parts[3];
            }
            if (parts.Length == 5)
            {
                takeCount = int.TryParse(parts[4], out int take) ? take : throw new Exception($"Вместо '{parts[4]}' нужно указать число");
            }

            //business logic

            var result = new List<string>();

            var web = new HtmlWeb();
            var doc = web.Load($"https://auto.ru/{city}/cars/{vendor}/{model}/all/?sort=fresh_relevance_1-desc&geo_radius=200");
            var hrefs = doc.DocumentNode.SelectNodes("//a[@class='Link ListingItemTitle-module__link']/href");

            foreach (var href in hrefs)
            {
                return href.InnerText;
                 //result.Add(href.InnerText);
                //break;
            }

            //return result.Take(takeCount).ToArray();  
            return null;
        }




    }
}
