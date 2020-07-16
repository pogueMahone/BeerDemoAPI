using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BeerDemo.Models;
using BeerDemo.Attributes;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Diagnostics;
using BeerDemo.BeerData;
using System.Net.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BeerDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeerController : ControllerBase
    {
        
        private readonly IDatabaseService _databaseService;
        private readonly HttpClient _client;
        public BeerController(IDatabaseService databaseService)
        {           
            this._databaseService = databaseService;
            this._client = new HttpClient();
        }
       
        [HttpGet]
        public async Task<IActionResult> GetByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest("Missing parameter.  Please include a name or a partial name.");
            IList<Beer> result = null;
            var streamTask = this._client.GetStreamAsync(string.Format("https://api.punkapi.com/v2/beers?beer_name={0}", name.Trim()));
            try
            {
                result = await JsonSerializer.DeserializeAsync<IList<Beer>>( await streamTask);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                return BadRequest("An error occured connecting to api.punkapi.com");
            }
            if (result == null || result.Count() == 0)
                return Ok(new {results = result, count = 0, msg="No matches found" });

            var query = from beerApi in result
                        join userRatingsDb in this._databaseService.UserRatings on beerApi.Id equals userRatingsDb.BeerId into grp                        
                        select new { id = beerApi.Id, name = beerApi.Name, description = beerApi.Description, userRatings = grp.Select( o => new { comments = o.Comment, rating = o.Rating, userName = o.UserName })};
            return Ok(new { results = query.ToList(), count = query.Count(), msg = "Beer ratings!" });
        }

     
        [ValidateBeerId]
        [ValidateUserRating]
        [HttpPut("{id}")]
        public IActionResult AddRating(int id, [FromBody] UserRating value)
        {
            value.BeerId = id;
            this._databaseService.UserRatings.Add(value);
            this._databaseService.SaveChanges();
            return Ok("Your rating has been added!");
        }        
    }
}
