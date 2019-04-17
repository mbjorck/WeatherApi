using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApi.Models
{
    public class RootObject
    {
        [JsonProperty("station")]
        public Station[] Stations { get; set; }
        //public Station Stations { get; set; }
        ////[JsonProperty("updated")]
        //public Int64 updated { get; set; }

    }

    public class Station
    {
        //private string key;
        [JsonProperty("key")]
        public string Key { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("owner")]
        public string Owner { get; set; }
        [JsonProperty("from")]
        public Int64 From { get; set; }
        [JsonProperty("to")]
        public Int64 To { get; set; }
        [JsonProperty("latitude")]
        public float Latitude { get; set; }
        [JsonProperty("longitude")]
        public float Longitude { get; set; }
        [JsonProperty("value")]
        public Value[] Values { get; set; }
        //public List<Value> Values { get; set; }
        //public string Key { get => key; set => key = value; }
    }
    public class Value
    {
        [JsonProperty("date")]
        public Int64 Date { get; set; }
        [JsonProperty("value")]
        public float value { get; set; }
        [JsonProperty("ref")]
        public string Ref { get; set; }
        [JsonProperty("quality")]
        public string Quality { get; set; }

    }
           
}
