using Microsoft.Extensions.Options;
using QuestByShevka.Services.Models;
using QuestByShevka.Shared.Exceptions;
using System;

namespace QuestByShevka.Services
{
    public class QuestRunnerService
    {
        private QuestHandler QuestHandler { get; set; }

        public QuestRunnerService(QuestHandler questHandler)
        {
            QuestHandler = questHandler;
        }

        public Question StartGame()
        {
            if (QuestHandler.CurrentQuestion != null)
            {
                throw new CorruptedRequestException("Cannot start game. Game is running already.");
            }

            QuestHandler.QuestStatus = GameStatus.Started;
            return QuestHandler.GetFirstQuestion();
        }
        public void FinishGame()
        {
            if (QuestHandler.QuestStatus != GameStatus.Started)
            {
                throw new CorruptedRequestException("Cannot finish game. Game is not started.");
            }
            if (!QuestHandler.IsLastQuestion() || !QuestHandler.IsQuestionComplete())
            {
                throw new CorruptedRequestException("Cannot finish game. Not all questions are complete.");
            }

            QuestHandler.QuestStatus = GameStatus.Finished;
            QuestHandler.CurrentQuestion = null;
        }
        public (Question, bool) GetNextQuestion()
        {
            QuestHandler.VerifyGameStarted();

            if (!NeedToProceedNextQuestion())
            {
                throw new CorruptedRequestException("Cannot proceed to next question. Current is not complete.");
            }
            return QuestHandler.GetNextQuestion();
        }
        public QuestionKeyStatus IsCorrentAnswer(string answer, int questionNumber)
        {
            QuestHandler.VerifyGameStarted();

            QuestHandler.VerifyQuestionNumber(questionNumber);

            var answerStatus =  QuestHandler.VerifyAnswer(answer);

            return answerStatus;
        }
        public bool NeedToProceedNextQuestion()
        {
            return QuestHandler.IsQuestionComplete();
        }
        public (Question, bool) GetCurrentQuestion()
        {
            QuestHandler.VerifyGameStarted();

            return (QuestHandler.CurrentQuestion, QuestHandler.IsLastQuestion());
        }

        public GameStatus GetQuestStatus()
        {
            return QuestHandler.QuestStatus;
        }

        public void ResetGame()
        {
            QuestHandler.Reset();
        }
    }
}
