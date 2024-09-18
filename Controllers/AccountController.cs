using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

public class AccountController : Controller
{
	private readonly HttpClient _httpClient;

	public AccountController(HttpClient httpClient)
	{
		_httpClient = httpClient;
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
		{
			return View(model);
		}

		var response = await _httpClient.PostAsJsonAsync("http://localhost:5282/api/Auth/login", model);

		if (response.IsSuccessStatusCode)
		{
			var responseData = await response.Content.ReadAsStringAsync();

			try
			{
				var jsonDocument = JsonDocument.Parse(responseData);
				
				if (jsonDocument.RootElement.TryGetProperty("token", out var tokenElement))
				{
					var token = tokenElement.GetString();
					
					HttpContext.Session.SetString("JwtToken", token);

					return RedirectToAction("Dashboard", "Bpkb");
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Token not found in response.");
				}
			}
			catch (JsonException)
			{
				ModelState.AddModelError(string.Empty, "Error parsing JSON response.");
			}
		}
		else
		{
			ModelState.AddModelError(string.Empty, "Invalid login attempt.");
		}

		return View(model);
	}
}