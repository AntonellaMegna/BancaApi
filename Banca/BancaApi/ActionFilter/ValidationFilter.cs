using Microsoft.AspNetCore.Mvc.Filters;

namespace BancaApi.ActionFilter
{
    public class ValidationFilter : IActionFilter

    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
           // var param = context.ActionArguments.SingleOrDefault(p => p.Value is IEntity);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
