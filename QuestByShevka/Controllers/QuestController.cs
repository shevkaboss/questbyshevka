using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuestByShevka.Services;
using QuestByShevka.Services.Models;
using QuestByShevka.Shared.Utils;
using QuestByShevka.WebApi.Models.Requests;
using QuestByShevka.WebApi.Models.Responses;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace QuestByShevka.WebApi.Controllers
{
    [ApiController]
    [Route("api/quest/[action]")]
    public class QuestController : ControllerBase
    {
        #region members
        public QuestRunnerService QuestRunnerService { get; }

        public QuestController(QuestRunnerService questRunnerService)
        {
            QuestRunnerService = questRunnerService;
        }
        #endregion


        /// <summary>
        /// Starts the game.
        /// </summary>
        [HttpGet]
        public ActionResult StartGame()
        {
            var question = QuestRunnerService.StartGame();

            var questionResponse = CreateQuestionResponse(question, false);

            return Ok(questionResponse);
        }

        /// <summary>
        /// Finishes the game.
        /// </summary>
        [HttpGet]
        public ActionResult FinishGame()
        {
            QuestRunnerService.FinishGame();
            return Ok();
        }

        /// <summary>
        /// Gets game status.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetGameStatus()
        {
            var questStatus = QuestRunnerService.GetQuestStatus();

            return Ok(questStatus.ToString());
        }

        /// <summary>
        /// Gets current question.
        /// </summary>
        [HttpGet]
        public ActionResult GetCurrentGame()
        {
            var (question, isLastQuestion) = QuestRunnerService.GetCurrentQuestion();

            var response = CreateQuestionResponse(question, isLastQuestion);

            return Ok(response);
        }

        /// <summary>
        /// Processes inputed answer.
        /// </summary>
        [HttpPost]
        public ActionResult ProceedAnswer([Required][FromBody] ProceedAnswerRequest proceedAnswerRequest)
        {
            if (string.IsNullOrEmpty(proceedAnswerRequest.Answer))
            {
                BadRequest("Empty answer field."); 
            }

            TelegramBotUtil.SendMessage($"Inputed asnwer: {proceedAnswerRequest.Answer}");

            var answerStatus = QuestRunnerService.IsCorrentAnswer(proceedAnswerRequest.Answer, proceedAnswerRequest.QuestionNumber);
            var isQuestionComplete = QuestRunnerService.NeedToProceedNextQuestion();

            var proceedAnswerResponse = CreateProceedAnswerResponce(answerStatus, isQuestionComplete);

            return Ok(proceedAnswerResponse);
        }

        /// <summary>
        /// Proceeds to next question.
        /// </summary>
        [HttpGet]
        public ActionResult GetNextQuestion()
        {
            var (question, isLastQuestion) = QuestRunnerService.GetNextQuestion();

            var questionResponse = CreateQuestionResponse(question, isLastQuestion);

            TelegramBotUtil.SendMessage($"Proceeded to next question.");

            return Ok(questionResponse);
        }

        #region private members
        private QuestionResponse CreateQuestionResponse(Question question, bool isLastQuestion)
        {
            return new QuestionResponse
            {
                Title = question.Title,
                Order = question.Order,
                QuestionBlocks = question.QuestionBlocks.OrderBy(x => x.Order).Select
                (
                    q => new
                    {
                        q.Order,
                        q.Text,
                        Image = q.ImageName
                    }),
                IsLastQuestion = isLastQuestion,
                NumberOfKeys = question.QuestionKeys.Count()
            };
        }
        private ProceedAnswerResponse CreateProceedAnswerResponce(QuestionKeyStatus questionKeyStatus, bool isQuestionComplete)
        {
            return new ProceedAnswerResponse
            {
                IsAnswerCorrect = questionKeyStatus.ToString(),
                NeedToProceedToNextQuestion = isQuestionComplete
            };
        }
        #endregion
    }
}
