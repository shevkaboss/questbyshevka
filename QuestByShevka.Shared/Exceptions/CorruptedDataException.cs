using System;

namespace QuestByShevka.Shared.Exceptions
{
    public class CorruptedRequestException : Exception
    {
        public CorruptedRequestException(string exceptionMessage) : base("Corrupted data. " + exceptionMessage)
        {

        }
    }
}
