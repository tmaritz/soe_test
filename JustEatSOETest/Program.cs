using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace JustEatSOETest
{
    public class FundsManaged
    {
        static HttpClient client = new HttpClient();

        //run main method
        static void Main()
        {
            RunAsync().Wait();
        }

        //display results in easily readable console output
        static void ShowRestaurants(RestaurantsFound.RootObject restaurants)
        {
            foreach (var r in restaurants.Restaurants)
            {
                Console.WriteLine($"Name: {r.Name}\n\tAverage Rating: {r.RatingAverage}");
                foreach (var c in r.CuisineTypes)
                {
                    Console.WriteLine($"\tType: {c.Name}");
                }
            }
        }

        //process GET request and parse the JSON
        static async Task<RestaurantsFound.RootObject> GetRestaurantsAsync(string path)
        {
            RestaurantsFound.RootObject _restaurants = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<RestaurantsFound.RootObject>(jsonString);
                ShowRestaurants(data);
            }
            return _restaurants;
        }

        //try running the GET method along with ShowRestaurants using JE's API and headers
        static async Task RunAsync()
        {
            Console.WriteLine("Please enter an outcode:...");
            var outcode = Console.ReadLine();

            client.BaseAddress = new Uri("https://public.je-apis.com/restaurants?q=" + outcode);
            client.DefaultRequestHeaders.Add("Accept-Tenant", "uk");
            client.DefaultRequestHeaders.Add("Accept-Language", "en-GB");
            client.DefaultRequestHeaders.Add("Authorization", "Basic VGVjaFRlc3RBUEk6dXNlcjI=");
            client.DefaultRequestHeaders.Add("Host", "public.je-apis.com");

            try
            {
                RestaurantsFound.RootObject _restaurants = await GetRestaurantsAsync(client.BaseAddress.ToString());
                ShowRestaurants(_restaurants);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();
        }
    }
}