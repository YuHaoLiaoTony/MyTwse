using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyTwse.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyTwse.Filters
{
    public class ActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // 若發生例外則不在這邊處理
            if (context.Exception != null)
                return;

            dynamic result = context.Result;
            var resp = new ApiResponseModel<dynamic>
            {
                Code = MyTwseExceptionEnum.OK,
                Data = result.Value,
                Message = string.Empty
            };
            context.HttpContext.Response.Headers.Add("Content-Type", "application/json");
            context.HttpContext.Response.WriteAsync(resp.ToJson());
        }
    }
}
