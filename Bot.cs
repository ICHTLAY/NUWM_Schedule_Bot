using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace NUWM_Schedule_Bot
{
    class Bot
    {
        private static TelegramBotClient client;
        private static string token = "xxx";

        public static void Start()
        {
            client = new TelegramBotClient(token);
            client.OnMessage += Client_OnMessageReceived;
            client.OnMessageEdited += Client_OnMessageReceived;
            client.OnCallbackQuery += Client_OnCallbackQuery;
            client.StartReceiving();
            Console.ReadLine();
            client.StopReceiving();
        }
        private static async void Client_OnCallbackQuery(object sender, CallbackQueryEventArgs queryEventArgs)
        {
            var query = queryEventArgs.CallbackQuery;
            switch (query.Data)
            {
                default: break;
            }

        }

        private static async void Client_OnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;

            ReplyKeyboardMarkup markup_menu = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton("КІ-51м"),
                new KeyboardButton("АУТПм-61"),
                new KeyboardButton("АКІТм-61")
            });
            markup_menu.OneTimeKeyboard = true;
            markup_menu.ResizeKeyboard = true;

            //InlineKeyboardMarkup inline_textformat = new InlineKeyboardMarkup(new[]
            //{
            //    new []
            //    {
            //        InlineKeyboardButton.WithCallbackData("Жирный"),
            //        InlineKeyboardButton.WithCallbackData("Курсив"),
            //        InlineKeyboardButton.WithCallbackData("Подчёркнутый")
            //    },
            //    new []
            //    {
            //        InlineKeyboardButton.WithCallbackData("Зачёркнутый"),
            //        InlineKeyboardButton.WithCallbackData("Машинный шрифт"),
            //    }
            //});

            switch (message.Text)
            {
                default:
                    string schedule = await Parser.GetSchedule(message.Text); 
                    await client.SendTextMessageAsync(message.Chat.Id, schedule, ParseMode.Html, replyMarkup: markup_menu);
                    break;
            }
        }
        
    }
}
