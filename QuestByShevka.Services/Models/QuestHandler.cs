using Microsoft.Extensions.Options;
using QuestByShevka.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuestByShevka.Services.Models
{
    public class QuestHandler
    {
        public GameStatus QuestStatus = GameStatus.NotStarted;
        public Quest QuestCore { get; set; }
        public Question CurrentQuestion { get; set; }
        private Dictionary<string, QuestionKeyStatus> MapAnswerStatus { get; set; }
        public QuestHandler(IOptions<Quest> questCoreOptions)
        {
            QuestCore = questCoreOptions.Value;
        }

        public Question GetFirstQuestion()
        {
            return GetQuestionByPosition(1);
        }

        public (Question, bool) GetNextQuestion()
        {
            var nextQuestion = GetQuestionByPosition(CurrentQuestion.Order + 1);
            var isLastQuestion = IsLastQuestion();

            return (nextQuestion, isLastQuestion);
        }
        public bool IsLastQuestion()
        {
            return CurrentQuestion.Order == QuestCore.Questions.Count();
        }

        public QuestionKeyStatus VerifyAnswer(string answer)
        {
            var isQuestionKeyExists = CurrentQuestion.QuestionKeys.Contains(answer.ToLower());
            
            if (isQuestionKeyExists)
            {
                if (MapAnswerStatus[answer.ToLower()] == QuestionKeyStatus.Accepted)
                    return QuestionKeyStatus.AlreadyAccepted;

                SetCorrentAnswer(answer);
                return QuestionKeyStatus.Accepted;
            }

            return QuestionKeyStatus.NotAccepted;
        }
        public void VerifyQuestionNumber(int questionNumber)
        {
            if (CurrentQuestion.Order != questionNumber)
                throw new CorruptedRequestException($"Wrong question number {questionNumber}");
        }

        public bool IsQuestionComplete()
        {
            return !MapAnswerStatus.Any(x => x.Value == QuestionKeyStatus.NotAccepted);
        }
        public void VerifyGameStarted()
        {
            if (QuestStatus != GameStatus.Started)
            {
                throw new CorruptedRequestException("Game is not started.");
            }
        }

        public void Reset()
        {
            QuestStatus = GameStatus.NotStarted;
            CurrentQuestion = null;
            MapAnswerStatus = null;
    }
        #region private memebers
        private void SetCorrentAnswer(string answer)
        {
            MapAnswerStatus[answer.ToLower()] = QuestionKeyStatus.Accepted;
        }

        public Question GetQuestionByPosition(int position)
        {
            var nextQuestion = QuestCore.Questions.FirstOrDefault(q => q.Order == position);

            if (nextQuestion != null)
            {
                SetNextQuestion(nextQuestion);

                return CurrentQuestion;
            }
            else
            {
                throw new Exception($"Cannot find question, requested question position {CurrentQuestion.Order + 1}.");
            }
        }

        private void SetNextQuestion(Question question)
        {
            CurrentQuestion = question;
            MapAnswerStatus = new Dictionary<string, QuestionKeyStatus>();

            foreach (var key in CurrentQuestion.QuestionKeys)
            {
                MapAnswerStatus.Add(key.ToLower(), QuestionKeyStatus.NotAccepted);
            }
        }
        #endregion
    }

    public enum QuestionKeyStatus
    {
        Accepted,
        NotAccepted,
        AlreadyAccepted
    }

    public enum GameStatus
    {
        Started,
        NotStarted,
        Finished
    }
}
