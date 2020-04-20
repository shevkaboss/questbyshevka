﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuestByShevka.Services;

namespace QuestByShevka.WebApi.Controllers
{
    [Route("api/admin/[action]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        public QuestRunnerService QuestRunnerService { get; }

        public AdminController(QuestRunnerService questRunnerService)
        {
            QuestRunnerService = questRunnerService;
        }

        /// <summary>
        /// Resets the game.
        /// </summary>
        [HttpGet]
        public ActionResult StartGame()
        {
            QuestRunnerService.ResetGame();

            return Ok();
        }
    }
}