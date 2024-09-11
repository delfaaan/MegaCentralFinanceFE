using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Threading.Tasks;

public class BpkbController : Controller
{
	private readonly HttpClient _httpClient;

	public BpkbController(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	[HttpGet]
	public IActionResult CreateBpkb()
	{
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
			return RedirectToAction("Success"); // You should create a Success view or similar
		}

		ModelState.AddModelError(string.Empty, "Failed to create BPKB.");
		return View(model);
	}

	[HttpGet]
	public IActionResult Success()
	{
		return View();
	}
}