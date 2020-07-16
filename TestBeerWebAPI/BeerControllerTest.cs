using System;
using Xunit;
using Microsoft.Extensions.Configuration;
using BeerDemo.BeerData;
using BeerDemo.Controllers;
using BeerDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Collections.Generic;
using BeerDemo.Attributes;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TestBeerWebAPI
{
    public class BeerControllerTest
    {
        private readonly IDatabaseService _databaseService;
        private readonly BeerController _beerController;
        public BeerControllerTest()
        {
            var config = new ConfigurationBuilder().AddJsonFile(@"C:\Projects\BeerDemo\TestBeerWebAPI\test_appsettings.json").Build();

            _databaseService = new DatabaseService(config);
            _beerController = new BeerController(_databaseService);
        }


        [Fact]
        public void GetByName_OKTest()
        {
            var name = "Ale";
            var result = this._beerController.GetByName(name);
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetByName_NameMissingTest()
        {
            var name = string.Empty;
            var result = this._beerController.GetByName(name);
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void AddRating_OKTest()
        {
            UserRating rating = new UserRating() { UserName = "john.doe@domain.com", Rating = 4, Comment = "My comment" };
            var result = this._beerController.AddRating(9, rating);
            Assert.IsType<OkObjectResult>(result);           
        }

        

        private Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext GetActionExecutingContext() 
        {
            var modelState = new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary();
            var actionContext = new ActionContext(
                                    Mock.Of<HttpContext>(),
                                    Mock.Of<Microsoft.AspNetCore.Routing.RouteData>(),
                                    Mock.Of<Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor>(),
                                    modelState
                                    );
            return new Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                Mock.Of<Microsoft.AspNetCore.Mvc.Controller>()
            );
        }

        [Fact]
        public void ValidateBeerIdAttribute_MissingBeerId_ReturnBadRequestTest()
        {
            var filter = new ValidateBeerIdAttribute();
            var actionExecutingContext = GetActionExecutingContext();
            filter.OnActionExecuting(actionExecutingContext);
            Assert.IsType<BadRequestObjectResult>(actionExecutingContext.Result);
        }

        [Fact]
        public void ValidateBeerIdAttribute_InvalidBeerId_ReturnBadRequestTest()
        {
            var filter = new ValidateBeerIdAttribute();
            
            var actionExecutingContext = GetActionExecutingContext();
            actionExecutingContext.ActionArguments.Add("id", 5000);
            filter.OnActionExecuting(actionExecutingContext);
            Assert.IsType<BadRequestObjectResult>(actionExecutingContext.Result);           
        }
       
    }
}
