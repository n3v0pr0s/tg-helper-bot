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
            botClient = new TelegramBotClient("822847399:AAEV2_1cSxDghisCRR-Q-BGIayWtYMq42sY");
            var me = botClient.GetMeAsync().Result;
            Console.WriteLine($"Hello, World! I am user {me.Id} and my name is {me.FirstName}.");

            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
            Thread.Sleep(int.MaxValue);
        }

        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                Console.WriteLine($"Received a text message in chat {e.Message.Chat.Id}.");

                switch (e.Message.Text)
                {
                    case var message when message == "/help":
                        await SendMessage(e.Message.Chat, e.Message.MessageId, "Нужна помощь, скоро поможем!");
                        break;
                    case var message when message == "/hello":
                        await SendMessage(e.Message.Chat, e.Message.MessageId, "Привет!");
                        break;

                    default:
                        await SendMessage(e.Message.Chat, e.Message.MessageId, "Команда не распознана");
                        break;

                }
            }
        }

        public static async Task SendMessage(Chat chatId, int messageId, string message)
        {
            await botClient.SendTextMessageAsync(chatId: chatId, text: message, replyToMessageId: messageId);
        }
    }
}
