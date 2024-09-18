using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

public class BpkbController : Controller
{
	private readonly HttpClient _httpClient;

	public BpkbController(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	[HttpGet]
	public async Task<IActionResult> Dashboard()
	{
		var token = HttpContext.Session.GetString("JwtToken");

		if (string.IsNullOrEmpty(token))
		{
			return RedirectToRoute(new { controller = "Account", action = "Login" });
		}

		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

		var response = await _httpClient.GetAsync("http://localhost:5282/api/Bpkb/all");

		if (response.IsSuccessStatusCode)
		{
			var bpkbList = await response.Content.ReadFromJsonAsync<List<BpkbViewModel>>();

			return View(bpkbList);
		}

		return View(new List<BpkbViewModel>());
	}

	[HttpGet]
	public IActionResult CreateBpkb()
	{
		var token = HttpContext.Session.GetString("JwtToken");

		if (string.IsNullOrEmpty(token))
		{
			return RedirectToRoute(new { controller = "Account", action = "Login" });
		}

		return View();
	}

	[HttpPost]
	public async Task<IActionResult> CreateBpkb(BpkbViewModel model)
	{
		if (!ModelState.IsValid)
		{
			return View(model);
		}

		var token = HttpContext.Session.GetString("JwtToken");

		if (string.IsNullOrEmpty(token))
		{
			return Unauthorized();
		}

		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

		var response = await _httpClient.PostAsJsonAsync("http://localhost:5282/api/Bpkb/create", model);

		if (response.IsSuccessStatusCode)
		{
			return RedirectToAction("Success");
		}

		ModelState.AddModelError(string.Empty, "Failed to create BPKB.");

		return View(model);
	}

	[HttpGet]
	public async Task<IActionResult> UpdateBpkb(string agreementNumber)
	{
		var token = HttpContext.Session.GetString("JwtToken");

		if (string.IsNullOrEmpty(token))
		{
			return RedirectToRoute(new { controller = "Account", action = "Login" });
		}

		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

		var response = await _httpClient.GetAsync($"http://localhost:5282/api/Bpkb/{agreementNumber}");

		if (response.IsSuccessStatusCode)
		{
			var bpkb = await response.Content.ReadFromJsonAsync<BpkbViewModel>();

			return View(bpkb);
		}

		return NotFound($"Data with Agreement Number {agreementNumber} not found.");
	}

	[HttpPost]
	public async Task<IActionResult> UpdateBpkb(BpkbViewModel model)
	{
		if (!ModelState.IsValid)
		{
			return View(model);
		}

		var token = HttpContext.Session.GetString("JwtToken");

		if (string.IsNullOrEmpty(token))
		{
			return Unauthorized();
		}

		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

		var response = await _httpClient.PutAsJsonAsync($"http://localhost:5282/api/Bpkb/{model.AgreementNumber}", model);

		if (response.IsSuccessStatusCode)
		{
			return RedirectToAction("Dashboard");
		}

		ModelState.AddModelError(string.Empty, "Failed to update BPKB.");

		return View(model);
	}

	[HttpGet]
	public IActionResult Success()
	{
		return View();
	}
}