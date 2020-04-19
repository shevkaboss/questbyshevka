using System.Collections.Generic;

namespace QuestByShevka.Services.Models
{
    public class Question
    {
        public string Title { get; set; }
        public int Order { get; set; }
        public IEnumerable<QuestionBlock> QuestionBlocks { get; set; }
        public IEnumerable<string> QuestionKeys { get; set; }
    }
}
