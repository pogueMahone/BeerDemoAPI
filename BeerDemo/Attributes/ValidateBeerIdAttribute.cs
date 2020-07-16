using BeerDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;

namespace BeerDemo.Attributes
{
    public class ValidateBeerIdAttribute : ActionFilterAttribute
    {
        private readonly HttpClient _client;        
        public ValidateBeerIdAttribute()
        {
            this._client = new HttpClient();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.Count() == 0)
            {
                context.Result = new BadRequestObjectResult("Beer Id Required!!!");
                return;
            }   
            var id = context.ActionArguments["id"];
            string err = null;
            IList<Beer> result = null;
            if (id == null)
                context.Result = new BadRequestObjectResult("Beer Id Required!!!");
            var streamTask = this._client.GetStreamAsync(string.Format("https://api.punkapi.com/v2/beers/{0}", id));
            try 
            {
                result = JsonSerializer.DeserializeAsync<IList<Beer>>(streamTask.Result).Result;
            }
            catch (Exception ex)
            {
                err = ex.Message;
            }
                        
            if (result == null || result.Count() == 0)            
                context.Result = new BadRequestObjectResult(string.IsNullOrEmpty(err) ? "Invalid Beer Id!!!" : string.Format("Invalid Beer Id!!!  error - {0}", err));
            
        }
    }
}
