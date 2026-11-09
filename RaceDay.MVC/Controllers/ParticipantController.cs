using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Models;
using System.Text;
using System.Text.Json;

namespace RaceDay.MVC.Controllers
{
    public class ParticipantController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ParticipantController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private bool IsParticipant()
        {
            return HttpContext.Session.GetString("UserRole") == "Participant";
        }

        public async Task<IActionResult> Index()
        {
            if (!IsParticipant()) return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("RaceDayAPI");
            var response = await client.GetAsync("api/events");

            var events = new List<EventViewModel>();
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                events = JsonSerializer.Deserialize<List<EventViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<EventViewModel>();
            }

            return View(events);
        }

        [HttpPost]
        public async Task<IActionResult> Enrol(int eventId)
        {
            if (!IsParticipant()) return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("RaceDayAPI");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("JWToken"));

            // Note: Dans un vrai scénario, il faudrait choisir une catégorie. Ici on simplifie.
            var enrolModel = new { EventId = eventId, CategoryId = 1 };
            var json = JsonSerializer.Serialize(enrolModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"api/events/{eventId}/enrol", content);

            // Recharge la page
            return RedirectToAction("Index");
        }
    }
}