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
                        case var message when message.StartsWith("/random"):
                            await SendMessage(e.Message.Chat, e.Message.MessageId, GetRandom(e.Message.Text));
                            break;

                    }
                }
            }
            catch (Exception ex)
            {
                await SendMessage(e.Message.Chat, e.Message.MessageId, ex.Message);
            }
        }

        static string GetRandom(string command)
        {
            var parts = command.Split(new char[] { ' ' });
            if (parts.Length == 2)
            {
                var number = int.TryParse(parts[1], out int num) ? num : throw new Exception($"Параметр '{parts[1]}' должен быть целым числом больше нуля, проверьте данные и повторите ввод");
                if (number < 1)
                {
                    throw new Exception($"Число должно быть больше нуля");
                }
                var rand = new Random();

                return rand.Next(number).ToString();
            }

            throw new Exception("Неверный вызов функции. Используйте формат: /random число");
        }

        static async Task SendMessage(Chat chatId, int messageId, string message)
        {
            await botClient.SendTextMessageAsync(chatId: chatId, text: message, replyToMessageId: messageId);
        }
    }
}
