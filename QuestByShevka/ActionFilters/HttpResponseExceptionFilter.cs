using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using QuestByShevka.Shared.Exceptions;
using System;
using System.Net;

namespace QuestByShevka.WebApi.ActionFilters
{
    public class HttpResponseExceptionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                if (context.Exception is CorruptedRequestException)
                {
                    context.Result = new ObjectResult(context.Exception.Message)
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest
                    };
                    context.ExceptionHandled = true;
                    return;
                }

                if (context.Exception is Exception)
                {
                    context.Result = new ObjectResult(context.Exception.Message)
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError
                    };
                    context.ExceptionHandled = true;
                    return;
                }
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {          
        }
    }
}
