using DAL;
using DAL.Services;
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
        public static void Main(string[] args)
        {
            botClient = new TelegramBotClient("1325530422:AAF6YmqP0F_UYRInxUJ9Q94j_AVJ3PBiZQ8");
            var me = botClient.GetMeAsync().Result;

            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
            Thread.Sleep(int.MaxValue);
        }

        private static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            try
            {
                if (e.Message.Text == null || e.Message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                    return;

                Console.WriteLine($"User '{e.Message.Chat.Username ?? e.Message.Chat.Id.ToString()}' send message '{e.Message.Text}'.");

                //TODO: add more commands
                switch (e.Message.Text)
                {
                    case "/rub":
                        await SendMessage(e.Message.Chat, Finance.RUR.GetUSDRatio());
                        break;

                    case "/btc":
                        await SendMessage(e.Message.Chat, Finance.BTC.GetUSDRatio());
                        break;


                    case "/cases":
                        using (var ctx = new ApplicationContext())
                        {
                            var caseService = new CaseService(ctx);
                            var msg = await caseService.GetAllCasesAsJSON();

                            await SendMessage(e.Message.Chat, msg);
                        }
                        break;

                }
            }
            catch (Exception ex)
            {
                await SendMessage(e.Message.Chat, ex.Message, e.Message.MessageId);
            }
        }

        private static async Task SendMessage(Chat chatId, string message, int messageId = 0)
        {
            await botClient.SendTextMessageAsync(chatId: chatId, text: message, replyToMessageId: messageId);
        }


        //private static InlineKeyboardButton[][] GetInlineKeyboard(string[] stringArray)
        //{
        //    var keyboardInline = new InlineKeyboardButton[1][];
        //    var keyboardButtons = new InlineKeyboardButton[stringArray.Length];
        //    for (var i = 0; i < stringArray.Length; i++)
        //    {
        //        keyboardButtons[i] = new InlineKeyboardButton
        //        {
        //            Text = stringArray[i],
        //            CallbackData = "Some Callback Data",
        //        };
        //    }
        //    keyboardInline[0] = keyboardButtons;
        //    return keyboardInline;
        //}
    }
}
