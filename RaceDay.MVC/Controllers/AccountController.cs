using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Models;
using System.Text;
using System.Text.Json;

namespace RaceDay.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync("api/auth/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<LoginResponse>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (result != null && !string.IsNullOrEmpty(result.Token))
                    {
                        HttpContext.Session.SetString("JWToken", result.Token);
                        HttpContext.Session.SetString("UserName", result.User.FirstName);
                        HttpContext.Session.SetString("UserRole", result.User.Role);

                        if (result.User.Role == "Organiser")
                        {
                            return RedirectToAction("Index", "Organiser");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Participant");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "API returned success, but token was empty.");
                    }
                }
                else
                {
                    // On affiche l'erreur exacte renvoyée par l'API
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError("", $"API Error {response.StatusCode}: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                // Si l'API est éteinte ou l'URL est mauvaise, on verra cette erreur
                ModelState.AddModelError("", $"Connection Error: {ex.Message}");
            }

            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }

    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public UserDto User { get; set; } = new UserDto();
    }

    public class UserDto
    {
        public int UserID { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}