﻿using DAL;
using DAL.Entities;
using Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot
{
    class Program
    {
        private const string token = "1325530422:AAF6YmqP0F_UYRInxUJ9Q94j_AVJ3PBiZQ8";
        private static ITelegramBotClient bot;
        public static async Task Main(string[] args)
        {
            bot = new TelegramBotClient(token);

            var me = await bot.GetMeAsync();
            Console.Title = me.Username;

            bot.OnMessage += BotOnMessageReceived;
            bot.OnMessageEdited += BotOnMessageReceived;
            bot.OnCallbackQuery += BotOnCallbackQueryReceived;
            bot.OnInlineQuery += BotOnInlineQueryReceived;
            bot.OnInlineResultChosen += BotOnChosenInlineResultReceived;
            bot.OnReceiveError += BotOnReceiveError;

            bot.StartReceiving(Array.Empty<UpdateType>());
            Console.WriteLine($"Start listening for @{me.Username}");

            Console.ReadLine();
            bot.StopReceiving();

        }

        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;
            if (message == null || message.Type != MessageType.Text)
                return;

            Console.WriteLine($"User '{messageEventArgs.Message.Chat.Username ?? messageEventArgs.Message.Chat.Id.ToString()}' send message '{message.Text}'.");

            switch (message.Text.Split(' ').First())
            {
                case "/rub":
                    await bot.SendTextMessageAsync(chatId: message.Chat.Id, text: FinanceService.GetRUBRatio());
                    break;

                case "/btc":
                    await bot.SendTextMessageAsync(chatId: message.Chat.Id, text: FinanceService.GetBTCRatio());
                    break;

                case "/notes":
                    using (var db = new ApplicationContext())
                    {
                        var servise = new NoteService(db);
                        var notes = await servise.GetNotesByUserId(1);

                        await SendInlineKeyboardByNotes(message, notes);
                    }
                    break;

                default:
                    await Usage(message);
                    break;

                    // Send inline keyboard
                    //case "/inline":
                    //    await SendInlineKeyboard(message);
                    //    break;

                    // send custom keyboard
                    //case "/keyboard":
                    //    await SendReplyKeyboard(message);
                    //    break;


                    // send a photo
                    //case "/photo":
                    //    await SendDocument(message);
                    //    break;

                    // request location or contact
                    //case "/request":
                    //    await RequestContactAndLocation(message);
                    //    break;

            }

            static async Task SendInlineKeyboard(Message message)
            {
                await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

                // Simulate longer running task
                await Task.Delay(1500);

                var inlineKeyboard = new InlineKeyboardMarkup(new[]
                {
                    // first row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("1.1", "11"),
                        InlineKeyboardButton.WithCallbackData("1.2", "12"),
                    },
                    // second row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("2.1", "21"),
                        InlineKeyboardButton.WithCallbackData("2.2", "22"),
                    }
                });
                await bot.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Choose",
                    replyMarkup: inlineKeyboard
                );
            }

            static async Task SendInlineKeyboardByNotes(Message message, IEnumerable<Note> notes)
            {
                var rows = new List<List<InlineKeyboardButton>>();
                for (int i = 0; i < notes.Count(); i++)
                {
                    if (i % 3 == 0)
                        rows.Add(new List<InlineKeyboardButton>());

                    var row = rows.LastOrDefault();
                    if (row != null)
                    {
                        var btn = InlineKeyboardButton.WithCallbackData(notes.ElementAt(i).title, notes.ElementAt(i).content);
                        row.Add(btn);
                    }
                }

                var inlineKeyboard = new InlineKeyboardMarkup(rows);

                await bot.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Choose",
                    replyMarkup: inlineKeyboard
                );
            }

            static async Task SendReplyKeyboard(Message message)
            {
                var replyKeyboardMarkup = new ReplyKeyboardMarkup(
                    new KeyboardButton[][]
                    {
                        new KeyboardButton[] { "1.1", "1.2" },
                        new KeyboardButton[] { "2.1", "2.2" },
                    },
                    resizeKeyboard: true
                );

                await bot.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Choose",
                    replyMarkup: replyKeyboardMarkup

                );
            }

            static async Task SendDocument(Message message)
            {
                await bot.SendChatActionAsync(message.Chat.Id, ChatAction.UploadPhoto);

                const string filePath = @"Files/tux.png";
                using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var fileName = filePath.Split(Path.DirectorySeparatorChar).Last();
                await bot.SendPhotoAsync(
                    chatId: message.Chat.Id,
                    photo: new InputOnlineFile(fileStream, fileName),
                    caption: "Nice Picture"
                );
            }

            static async Task RequestContactAndLocation(Message message)
            {
                var RequestReplyKeyboard = new ReplyKeyboardMarkup(new[]
                {
                    KeyboardButton.WithRequestLocation("Location"),
                    KeyboardButton.WithRequestContact("Contact"),
                });
                await bot.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Who or Where are you?",
                    replyMarkup: RequestReplyKeyboard
                );
            }

            static async Task Usage(Message message)
            {
                const string usage = "Usage:\n" +
                                        "/rub   - get USD/RUB ratio\n" +
                                        "/btc - get BTC/USD\n" +
                                        "/notes - get user notes";

                await bot.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: usage,
                    replyMarkup: new ReplyKeyboardRemove()
                );
            }
        }

        // Process Inline Keyboard callback data
        private static async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            var callbackQuery = callbackQueryEventArgs.CallbackQuery;

            await bot.AnswerCallbackQueryAsync(
                callbackQueryId: callbackQuery.Id
            //text: callbackQuery.Data
            );

            await bot.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: callbackQuery.Data
            );

            //TODO
            //Console.WriteLine($"User '{callbackQuery.Message.Chat.Username ?? callbackQuery.Message.Chat.Id.ToString()}' send message '{callbackQueryEventArgs.CallbackQuery.InlineMessageId}'.");
        }

        private static async void BotOnInlineQueryReceived(object sender, InlineQueryEventArgs inlineQueryEventArgs)
        {
            Console.WriteLine($"Received inline query from: {inlineQueryEventArgs.InlineQuery.From.Id}");

            InlineQueryResultBase[] results = {
                // displayed result
                new InlineQueryResultArticle(
                    id: "3",
                    title: "TgBots",
                    inputMessageContent: new InputTextMessageContent(
                        "hello"
                    )
                )
            };
            await bot.AnswerInlineQueryAsync(
                inlineQueryId: inlineQueryEventArgs.InlineQuery.Id,
                results: results,
                isPersonal: true,
                cacheTime: 0
            );
        }

        private static void BotOnChosenInlineResultReceived(object sender, ChosenInlineResultEventArgs chosenInlineResultEventArgs)
        {
            Console.WriteLine($"Received inline result: {chosenInlineResultEventArgs.ChosenInlineResult.ResultId}");
        }

        private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            Console.WriteLine("Received error: {0} — {1}",
                receiveErrorEventArgs.ApiRequestException.ErrorCode,
                receiveErrorEventArgs.ApiRequestException.Message
            );
        }

    }
}
