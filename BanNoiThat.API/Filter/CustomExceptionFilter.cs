using BanNoiThat.API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

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
            else if (context.Exception is DbUpdateException dbUpdateEx)
            {
                var innerMessage = dbUpdateEx.InnerException?.Message ?? dbUpdateEx.Message;
                string relatedEntity = "unknown entity";

                // Phân tích thông báo lỗi để lấy thông tin thực thể
                if (innerMessage.Contains("REFERENCE constraint"))
                {
                    // Trích xuất thông tin bảng liên quan từ thông báo
                    var match = System.Text.RegularExpressions.Regex.Match(innerMessage, @"table ""dbo\.(\w+)""");
                    if (match.Success)
                    {
                        relatedEntity = match.Groups[1].Value; // Lấy tên bảng
                    }
                }

                var apiResponse = new ApiResponse
                {
                    StatusCode = System.Net.HttpStatusCode.Conflict, // 409 Conflict
                    ErrorMessages = { $"Không thể xóa vì '{relatedEntity}' đang được trỏ tới." }
                };

                context.Result = new ObjectResult(apiResponse)
                {
                    StatusCode = 409
                };
                context.ExceptionHandled = true;
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
