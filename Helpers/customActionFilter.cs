using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace StackApi.Helpers;

public class customActionFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid)
        {
            return;
        }
        var errors = context.ModelState.Keys.SelectMany(x => context.ModelState[x].Errors).Select(x => x.ErrorMessage).ToArray();
        var response = new Response<object>() { Succeeded = false, Errors = errors };
        context.Result = new BadRequestObjectResult(response);
    }
}