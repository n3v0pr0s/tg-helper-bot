using DAL;
using DAL.Services;
using System;
using System.IO;
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
                        await SendMessage(e.Message.Chat, e.Message.MessageId, Finance.RUR.GetUSDRatio());
                        break;

                    case "/btc":
                        await SendMessage(e.Message.Chat, e.Message.MessageId, Finance.BTC.GetUSDRatio());
                        break;

                    case "/case":
                        using (var ctx = new ApplicationContext())
                        {
                            var caseService = new CaseService(ctx);
                            var msg = await caseService.GetAllCasesAsJSON();
                            await SendMessage(e.Message.Chat, e.Message.MessageId, msg);
                        }
                        break;


                        //case "/pic":
                        //    var map = new ProceduralGeneration.Map();
                        //    var bitmap = map.Draw();
                        //    var path = @"map.bmp";
                        //    bitmap.Save(path);
                        //    using (var stream = System.IO.File.OpenRead(path))
                        //        await SendImage(e.Message.Chat, e.Message.MessageId, stream);
                        //    break;                            

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

        static async Task SendImage(Chat chatId, int messageId, dynamic image)
        {
            await botClient.SendPhotoAsync(chatId: chatId, photo: image, replyToMessageId: messageId);
        }
    }
}
