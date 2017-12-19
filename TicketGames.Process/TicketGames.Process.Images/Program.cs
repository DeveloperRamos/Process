using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TicketGames.Process.Images.Models;

namespace TicketGames.Process.Images
{
    public class Program
    {
        static void Main(string[] args)
        {
            Task T = new Task(ApiCall);
            T.Start();
            Console.WriteLine("Json data........");
            Console.ReadLine();
        }
        static async void ApiCall()
        {

            var apiImageShack = "https://api.imageshack.com/";
            var route = "v2/user/marcio.correia/images?offset=0&limit=1000";

            using (var client = new HttpClient())
            {

                HttpResponseMessage response = await client.GetAsync(string.Concat(apiImageShack, route));

                response.EnsureSuccessStatusCode();

                Models.RootObject images = null;

                using (HttpContent content = response.Content)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    Console.WriteLine(responseBody.Substring(0, 50) + "........");

                    images = JsonConvert.DeserializeObject<Models.RootObject>(responseBody);
                }

                List<Product> products = new List<Product>();
                Product product = null;

                foreach (var image in images.result.images)
                {
                    var list = image.original_filename.Split('.');
                    long id;
                    long.TryParse(list[0], out id);

                    if (id > 0)
                    {
                        product = new Product();
                        product.Id = id;

                        product.Images.Add(new Image() { TypeId = 2, url = "https://" + image.direct_link });

                        var b = images.result.images.Where(i => i.original_filename.Contains(id.ToString() + "-B")).Select(x => x.direct_link).FirstOrDefault();
                        var gp1 = images.result.images.Where(i => i.original_filename.Contains(id.ToString() + "-GP-1")).Select(x => x.direct_link).FirstOrDefault();
                        var gp2 = images.result.images.Where(i => i.original_filename.Contains(id.ToString() + "-GP-2")).Select(x => x.direct_link).FirstOrDefault();
                        var gp3 = images.result.images.Where(i => i.original_filename.Contains(id.ToString() + "-GP-3")).Select(x => x.direct_link).FirstOrDefault();

                        if (!string.IsNullOrEmpty(b)) product.Images.Add(new Image() { TypeId = 1, url = "https://" + b });
                        if (!string.IsNullOrEmpty(gp1)) product.Images.Add(new Image() { TypeId = 3, url = "https://" + gp1 });
                        if (!string.IsNullOrEmpty(gp2)) product.Images.Add(new Image() { TypeId = 3, url = "https://" + gp2 });
                        if (!string.IsNullOrEmpty(gp3)) product.Images.Add(new Image() { TypeId = 3, url = "https://" + gp3 });

                        products.Add(product);
                    }
                }






                Console.WriteLine(products.Count.ToString());

                Console.ReadKey();
            }
        }
    }
}
