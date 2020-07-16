﻿
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BeerDemo.Models
{
    public class Beer
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
       
    }
}
