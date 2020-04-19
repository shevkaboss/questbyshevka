using System.Collections.Generic;

namespace QuestByShevka.WebApi.Models.Responses
{
    public class QuestionResponse
    {
        public string Title { get; set; }
        public int Order { get; set; }
        public IEnumerable<object> QuestionBlocks { get; set; }
        public bool IsLastQuestion { get; set; }
        public int NumberOfKeys { get; set; }
    }
}
