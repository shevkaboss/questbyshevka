using Telegram.Bot;

namespace QuestByShevka.Shared.Utils
{
    public class TelegramBotUtil
    {
        private static readonly TelegramBotClient telegramBot;
        static TelegramBotUtil()
        {
            telegramBot = new TelegramBotClient("1151750740:AAFVEhSfvinmoXp3-DUcY1y3IZ4Go69rFYs");
        }
        public static async void SendMessage(string msg)
        {
            try
            {
                await telegramBot.SendTextMessageAsync("389495300", msg);
            }
            catch { }
        }
    }
}
