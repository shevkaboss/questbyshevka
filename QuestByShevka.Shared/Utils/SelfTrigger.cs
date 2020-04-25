using System.Net.Http;
using System.Threading.Tasks;

namespace QuestByShevka.Shared.Utils
{
    public class SelfTrigger
    {
        private readonly HttpClient _httpClient;

        public SelfTrigger(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void StartHerokuSelfTrigger()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        var test = await _httpClient.GetAsync("https://lashici-quest.herokuapp.com/");
                        var test2 = await _httpClient.GetAsync("https://lashiciquest.herokuapp.com/api/quest/getgamestatus");
                    }
                    finally
                    {
                        await Task.Delay(60000);
                    }
                }
            });
        }
    }
}
