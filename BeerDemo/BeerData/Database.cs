using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BeerDemo.Models;
using System.Text.Json;
using System.Diagnostics;

namespace BeerDemo.BeerData
{
    public class DatabaseService : IDatabaseService
    {
        private readonly IConfiguration _configuration;
        private readonly string _databasePath;        
        public DatabaseService(IConfiguration configuration)
        {
            this._configuration = configuration;
            //this._databasePath = Path.Combine(env.ContentRootPath, this._configuration.GetValue<string>("BeerDb"));
            this._databasePath = this._configuration.GetValue<string>("BeerDb");
            this.Init();
        }

        private void Init()
        {
            try
            {

                
                string json = System.IO.File.ReadAllText(this._databasePath);
                if (string.IsNullOrEmpty(json))
                    this.UserRatings = new List<UserRating>();
                else
                    this.UserRatings = JsonSerializer.Deserialize<IList<UserRating>>(json);
                
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        public IList<UserRating> UserRatings { get; private set; }

        public void SaveChanges()
        {
            try
            {
                if (this.UserRatings.Count() > 0)
                {
                    var json = JsonSerializer.Serialize<IList<UserRating>>(this.UserRatings);
                    File.WriteAllText(this._databasePath, json);
                }
            }
            catch (Exception ex) 
            {
                Trace.WriteLine(ex.Message);
            }
        }
    }

    public interface IDatabaseService
    {
        public IList<UserRating> UserRatings { get; }
        public void SaveChanges();
    }
}
