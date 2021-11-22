// using System;
// using System.Net.Http;
// using System.Threading.Tasks;
// using Discord;
// using Discord.Commands;
// using Newtonsoft.Json.Linq;
//
// namespace Good_Bot
// {
//     public class amazon
//     {
//         public class News : ModuleBase<SocketCommandContext>
//         {
//             [Command("product")]
//             public async Task Sear([Remainder] String term)
//             {
//
//                 var client = new HttpClient();
//                 var request = new HttpRequestMessage
//                 {
//                     Method = HttpMethod.Get,
//                     RequestUri = new Uri($"https://amazon-product-reviews-keywords.p.rapidapi.com/product/search?keyword={term}&country=IN&category=aps"),
//                     Headers =
//                     {
//                         { "x-rapidapi-key", "388e24cd8cmsh8b7f8a4aae55578p14eb40jsnbd65a0d5bfa4" },
//                         { "x-rapidapi-host", "amazon-product-reviews-keywords.p.rapidapi.com" },
//                     },
//                 };
//                 using var response = await client.SendAsync(request);
//                 response.EnsureSuccessStatusCode();
//                 var body = await response.Content.ReadAsStringAsync();
//                 Console.WriteLine(body);
//                 dynamic data = JObject.Parse(body);
//                 Console.WriteLine(data.products[1]);
//                 int z = 4;
//                 for (int i = 0; i < z; i++)
//                 {
//                     if (data.products[i].title.ToString().Contains("Sponsored"))
//                     {
//                         z++;
//                         continue;
//                     }
//                     EmbedBuilder embedBuilder = new EmbedBuilder().WithColor(Color.Blue)
//                         .WithImageUrl(data.products[i].thumbnail.ToString())
//                         .WithTitle(data.products[i].title.ToString())
//                         .WithUrl(data.products[i].url.ToString())
//                         .WithDescription("₹"+data.products[i].price.current_price.ToString());
//                     Embed embed = embedBuilder.Build();
//                     await ReplyAsync(embed:embed);
//                 }
//          
//             }
//         }
//     }
// }