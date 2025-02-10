using EmployeeApp.PortalWithAuth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace EmployeeApp.PortalWithAuth.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly HttpClient _httpClient;
        public AccountController(ILogger<AccountController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;

        }
        
        public IActionResult Login([FromQuery] string returnUrl)
        {
            if (!User.Identity.IsAuthenticated)
            {
                var model = new LoginViewModel
                {
                    ReturnUrl = returnUrl
                };
                _logger.LogInformation(returnUrl);
                return View(model);
            }
            return RedirectToAction("Landing", "Users");   
        }
        [HttpPost]
        public async Task<IActionResult> LoginClicked([Bind("ReturnUrl,Email,Password")] LoginViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                var requestBody = new
                {
                    Email = model.Email,
                    Password = model.Password
                };

                var json = System.Text.Json.JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("https://localhost:7245/api/Token/token", content);
                var responseBody = await response.Content.ReadFromJsonAsync<TokenResponse>();
                if (responseBody.Token != null)
                {
                    HttpContext.Session.SetString("JWTtoken", responseBody.Token);

                    if (model.ReturnUrl != null)
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Landing", "Users");

                    }

                }
                // Handle login failure
                ModelState.AddModelError("", "Invalid login attempt.");

                return View("LoginInvalid");
            }
            else
            {
                return RedirectToAction("Landing", "Users");
            }
            
        }
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            var cookieName = ".AspNetCore.Session"; 
            if (Request.Cookies[cookieName] != null)
            {
                Response.Cookies.Delete(cookieName);
            }
            return Redirect("/");
        }
        
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterSave
            ([Bind("Email,Password,ConfirmPassword")] RegisterViewModel model)
        {


            if (ModelState.IsValid)
            {
                var requestBody = new
                {
                    Email = model.Email,
                    Password = model.Password
                };

                var json = System.Text.Json.JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var regEndpt = await _httpClient.PostAsync("https://localhost:7245/api/Token/register", content);
                var response = await regEndpt.Content.ReadAsStringAsync();

                if (response == "true")
                {
                    return RedirectToAction(nameof(Login));
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    ModelState.AddModelError(string.Empty, error.ErrorMessage);
                    _logger.LogError(error.ErrorMessage);
                }
                return BadRequest(errors);
            }
            return View(model);
        }

        public JwtSecurityToken DecodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken;
        }
        public class TokenResponse
        {
            public string Token { get; set; }
        }
    }
}
