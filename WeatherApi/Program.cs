using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

using System.Collections.Generic;
using WeatherApi.Models;
using System.Net.Http.Headers;
using System.Threading;

namespace WeatherApi
{
    class Program
    {
        static void Main(string[] args)
        {
            //GetAverageTempForLastHour().Wait();
            //GetTotalRainfallInLund().Wait();
            PrintStations().Wait();

        }

        static async Task GetAverageTempForLastHour()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://opendata-download-metobs.smhi.se/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("api/version/1.0/parameter/1/station-set/all/period/latest-hour/data.json");

                if (response.IsSuccessStatusCode)
                {

                    var jsonString = await response.Content.ReadAsStringAsync();
                    var _Data = JsonConvert.DeserializeObject<RootObject>(jsonString);

                    float totalTemp = 0;
                    int numberOfValues = 0;

                    foreach (Station station in _Data.Stations)
                    {


                        if (station.Values != null)
                        {
                            foreach (Value values in station.Values)
                            {
                                totalTemp = totalTemp + values.value;
                                numberOfValues++;
                            }
                        }
                    }
                    float averageTemp = totalTemp / numberOfValues;
                    Console.WriteLine("The average temperature in Sweden for the last hour was " + averageTemp + " degrees");
                    Console.ReadKey();
                }
            }
        }

        static async Task GetTotalRainfallInLund()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://opendata-download-metobs.smhi.se/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("api/version/1.0/parameter/5/station/53430/period/latest-months/data.json");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var _Data = JsonConvert.DeserializeObject<Station>(jsonString);
                    float totalRainAmount = 0;
                    List<string> dates = new List<string>();
                    int numberOfIndices = 0;
                    foreach (Value values in _Data.Values)
                    {
                        totalRainAmount += values.value;
                        dates.Add(values.Ref);
                        numberOfIndices++;

                    }
                    Console.WriteLine("Between " + dates[0] + " and " + dates[(numberOfIndices - 1)] + " the total rainfall in Lund was " + totalRainAmount + " millimeters");
                    Console.ReadKey();
                }
            }
        }

        static async Task PrintStations()
        {
           

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://opendata-download-metobs.smhi.se/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("api/version/1.0/parameter/1/station-set/all/period/latest-hour/data.json");

                if (response.IsSuccessStatusCode)
                {

                    var jsonString = await response.Content.ReadAsStringAsync();
                    var _Data = JsonConvert.DeserializeObject<RootObject>(jsonString);
                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                    CancellationToken token = cancellationTokenSource.Token;
                    Task task = Task.Run(() =>
                    {
                        while (!token.IsCancellationRequested)
                        {
                            foreach (Station station in _Data.Stations)
                            {

                                Console.Write(station.Name + ": ");
                                if (station.Values != null)
                                {
                                    foreach (Value values in station.Values)
                                    {
                                        Console.WriteLine(values.value);
                                        Thread.Sleep(100);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("");
                                    Thread.Sleep(100);
                                }
                            }
                          
                        }
                        token.ThrowIfCancellationRequested();
                    }, token)
                    .ContinueWith(t =>
                    {
                        t.Exception?.Handle(e => true);
                        Console.WriteLine("You have canceled the task");
                    }, TaskContinuationOptions.OnlyOnCanceled);

                    Console.WriteLine("Press enter to stop the task");
                    Console.ReadLine();
                    cancellationTokenSource.Cancel();
                    task.Wait();

                    
                } else
                {
                    throw new Exception("ERROR");
                }
            }
        }
    }
}
