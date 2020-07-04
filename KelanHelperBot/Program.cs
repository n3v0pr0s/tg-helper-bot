using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
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
            //var proxy = new WebProxy("167.71.183.113:8888", true);            
            //botClient = new TelegramBotClient("822847399:AAHtD0vLdcTZtRas84-LWvvChIUNNPTK07w", proxy);

            botClient = new TelegramBotClient("1325530422:AAF6YmqP0F_UYRInxUJ9Q94j_AVJ3PBiZQ8");
            var me = botClient.GetMeAsync().Result;            

            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
            Thread.Sleep(int.MaxValue);
        }

        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            try
            {
                if (e.Message.Text == null || e.Message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                {
                    return;
                }

                Console.WriteLine($"Received a text message in chat {e.Message.Chat.Id}.");
                
                //TODO: add more commands
                switch (e.Message.Text)
                {
                    case "/rub":
                        await SendMessage(e.Message.Chat, e.Message.MessageId, GetRubRate());
                        break;                    
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

        static async Task SendMessage(Chat chatId, int messageId, IEnumerable<string> messages)
        {
            foreach (var message in messages)
            {
                await botClient.SendTextMessageAsync(chatId: chatId, text: message, replyToMessageId: messageId);
                Thread.Sleep(1500);
            }
        }

        //Business logic
        

        static string GetRubRate()
        {
            var web = new HtmlWeb();
            var doc = web.Load("http://www.profinance.ru/currency_usd.asp");
            var nodes = doc.DocumentNode.SelectNodes("//td[@class='cell'][@align='center'][@colspan='2']/font[@color='Red']/b");

            return nodes.First().InnerText;
        }
    }
}
