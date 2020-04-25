using System.Collections.Generic;

namespace QuestByShevka.Services.Models
{
    public class Quest
    {
        public IEnumerable<Question> Questions { get; set; }
        public string Finish { get; set; }
    }
}
