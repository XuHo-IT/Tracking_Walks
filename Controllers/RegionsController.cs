using Microsoft.AspNetCore.Mvc;
using NZwalks.web.Models;
using NZwalks.web.Models.DTO;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace NZwalks.web.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<RegionDto> response = new List<RegionDto>();
            try
            {
                // Get All Regions from Web API
                var client = httpClientFactory.CreateClient();

                var httpResponseMessage = await client.GetAsync("https://localhost:7196/api/regions");

                httpResponseMessage.EnsureSuccessStatusCode();

                response = await httpResponseMessage.Content.ReadFromJsonAsync<List<RegionDto>>();

            }
            catch (Exception ex)
            {
                // Log the exception
                // For now, we can use ViewBag to pass the error message to the view
                ViewBag.ErrorMessage = ex.Message;
            }
            return View(response);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddRegionView model)
        {
            var client = httpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7196/api/regions"),
                Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")
            };
            var httpResponseMessage = await client.SendAsync(httpRequestMessage);
            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();

            if (response != null)
            {
                return RedirectToAction("Index", "Regions");
            }
            return View();
        }

    }
}
