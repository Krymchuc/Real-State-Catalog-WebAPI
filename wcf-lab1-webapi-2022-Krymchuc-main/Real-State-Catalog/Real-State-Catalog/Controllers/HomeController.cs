using Real_State_Catalog.Data;
using Real_State_Catalog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
namespace Real_State_Catalog.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppContextDB _context;

        public HomeController(AppContextDB context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var appContextDB = _context.Offers
                .Include(o => o.Accommodation)
                .Include(o => o.Accommodation.Address)
                .Include(o => o.Accommodation.User)
                .Include(o => o.Accommodation.Pictures);

            return View(await appContextDB.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> Search(string city, string arrivalDate, string departureDate, string nbPerson)
        {
            HttpClient client = new();

            string path = this.Request.Scheme + "://" + this.Request.Host.Value + "/api/search/" + city + "/" + arrivalDate + "/" + departureDate + "/" + nbPerson;
            Debug.WriteLine("Search API path: " + path);

            IEnumerable<Offer>? offers = null;

            HttpResponseMessage response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            { 
                offers = JsonConvert.DeserializeObject<IEnumerable<Offer>>(await response.Content.ReadAsStringAsync());

                ViewBag.Search = true;
            }

            return View("Index", offers);
        }

        public async Task<IActionResult> Filter(string city, string Type, string nbPerson, string PricePerNight)
        {
            HttpClient client = new();

            string path = this.Request.Scheme + "://" + this.Request.Host.Value + "/api/filter/" + city + "/" + Type + "/" + nbPerson + "/" + PricePerNight;
            Debug.WriteLine("Search API path: " + path);

            IEnumerable<Offer>? offers = null;

            HttpResponseMessage response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                offers = JsonConvert.DeserializeObject<IEnumerable<Offer>>(await response.Content.ReadAsStringAsync());

                ViewBag.Filter = true;
            }

            return View("Index", offers);
        }
    }
}