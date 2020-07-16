using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Linq;

namespace BeerDemo.Attributes
{
    public class ValidateUserRatingAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var modelState = context.ModelState;            
            if (!modelState.IsValid)
            {
                IList<string> errors = new List<string>();
                foreach (var key in modelState.Keys)
                {
                    var state = modelState[key];
                    if (state.Errors.Any())
                    {
                        string er = state.Errors.First().ErrorMessage;
                        if (!string.IsNullOrEmpty(er))
                        {
                            errors.Add(er);
                        }
                    }
                }
                context.Result = new BadRequestObjectResult(errors);
            }
        }
    }
}
