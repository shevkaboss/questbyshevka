using QuestByShevka.Services.Models;

namespace QuestByShevka.WebApi.Models.Responses
{
    public class ProceedAnswerResponse
    {
        public string IsAnswerCorrect { get; set; }
        public bool NeedToProceedToNextQuestion { get; set; }
    }
}
