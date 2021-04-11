using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyTwse.Extensions;

namespace MyTwse
{
    public class ApiResponseModel<T> : ApiResponseModel
    {
        public T Data { get; set; }
    }
    public class ApiResponseModel
    {
        public MyTwseExceptionEnum Code { get; set; }

        public string Message { get; set; }
    }

    public class ExceptionFilter : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

            ApiResponseModel result = new ApiResponseModel
            {
                Code = MyTwseExceptionEnum.InternalServerError,
                Message = "系統錯誤"
            };

            MyTwseException myTwseException = context.Exception as MyTwseException;
            if (myTwseException != null)
            {
                statusCode = myTwseException.ErrorCode.GetHttpStatusCode();
                result.Code = myTwseException.ErrorCode;

                if (myTwseException.ExceptionDetail is ModelStateDictionary)
                {
                    var errors = ((ModelStateDictionary)myTwseException.ExceptionDetail)
                        .Values.SelectMany(e => e.Errors)
                        .Select(e => e.ErrorMessage);

                    result.Message = string.Join("|", errors);
                }
                else
                {
                    result.Message = myTwseException.ErrorCode.GetDescription();
                    if (string.IsNullOrWhiteSpace(myTwseException.Message) == false)
                    {
                        result.Message = $"{result.Message}：{myTwseException.ExceptionDetail}";
                    }
                }
            }
            context.HttpContext.Response.Headers.Add("Content-Type", "application/json");
            context.HttpContext.Response.StatusCode = (int)statusCode;
            context.HttpContext.Response.WriteAsync(result.ToJson());
            return Task.CompletedTask;
        }
    }
}
