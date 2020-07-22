using System;
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
                        await SendMessage(e.Message.Chat, e.Message.MessageId, Finance.RUR.GetRurRatio());
                        break;
                    case "/map":
                        var map = new ProceduralGeneration.Map();
                        await SendImage(e.Message.Chat, e.Message.MessageId, map.Draw());
                        break;
                    case "/test":
                        await SendMessage(e.Message.Chat, e.Message.MessageId, "test test test");
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

        static async Task SendImage(Chat chatId, int messageId, dynamic image)
        {
            await botClient.SendPhotoAsync(chatId: chatId, photo: image, replyToMessageId: messageId);
        }
    }
}
