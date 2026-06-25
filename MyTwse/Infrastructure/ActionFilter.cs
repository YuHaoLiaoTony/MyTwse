using Microsoft.AspNetCore.Mvc.Filters;

namespace MyTwse.Infrastructure
{
    public class ActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context) { }
        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
