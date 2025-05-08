using BanNoiThat.API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BanNoiThat.API.Filter
{
    public class CustomExceptionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is FileNotFoundException)
            {
                context.Result = new ObjectResult("File not found but handle in fileter")
                {
                    StatusCode = 503
                };
            }
            else if (context.Exception is Exception)
            {
                var apiReponse = new ApiResponse {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                };
                apiReponse.ErrorMessages.Add(context.Exception.Message);

                context.Result = new ObjectResult(apiReponse)
                {
                    StatusCode = 500,
                    Value = apiReponse
                };
            }
            context.ExceptionHandled = true;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}
