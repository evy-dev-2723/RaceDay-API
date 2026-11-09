using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Models;
using System.Text;
using System.Text.Json;
using System.Net.Http; // <-- INDISPENSABLE POUR IHttpClientFactory

namespace RaceDay.MVC.Controllers
{
    public class OrganiserController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OrganiserController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Vérifie si l'utilisateur est connecté en tant qu'Organisateur
        private bool IsOrganiser()
        {
            return HttpContext.Session.GetString("UserRole") == "Organiser";
        }

        public async Task<IActionResult> Index()
        {
            if (!IsOrganiser()) return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            // On ajoute le token JWT dans le header
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            var response = await client.GetAsync("api/events");
            var events = new List<EventViewModel>();

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                events = JsonSerializer.Deserialize<List<EventViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<EventViewModel>();
            }

            return View(events);
        }

        [HttpGet]
        public IActionResult CreateEvent()
        {
            if (!IsOrganiser()) return RedirectToAction("Login", "Account");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent(CreateEventViewModel model)
        {
            if (!IsOrganiser()) return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid) return View(model);

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/events", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Failed to create event. Check API.");
            return View(model);
        }
    }
}