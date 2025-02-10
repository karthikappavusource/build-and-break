
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeApp.Data.Data;
using EmployeeApp.Data.Models;
using EmployeeApp.Data.Interfaces;

using EmployeeApp.Services.UserServiceFolder;
using EmployeeApp.Services.AddressServiceFolder;
using System.Text.RegularExpressions;
using System.Numerics;
using Microsoft.AspNetCore.Authorization;
using EmployeeApp.PortalWithAuth.Models;
using EmployeeApp.ServiceApi.Controllers;
using EmployeeApp.Data.Interfaces.UserRepo;
using EmployeeApp.ServiceApi.Controllers.UsersService;
using EmployeeApp.ServiceApi.Controllers.AdressesService;
using Newtonsoft.Json;
using EmployeeApp.ServiceApi.Controllers.GroupsService;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;



namespace EmployeeApp.PortalWithAuth.Controllers
{
    public class TokenResponse
    {
        public string Token { get; set; }
    }
    [Authorize]
    
    public class UsersController : Controller
    {
        private readonly EmployeeDB2Context _context;
        private readonly IUsersService _empService;
        private readonly IAddressesService _addressService;
        private readonly IGroupsService _grpService;
        private readonly ILogger<UsersController> _logger;
        private readonly HttpClient _httpClient;
        public UsersController(EmployeeDB2Context context, 
            IUsersService empservice, 
            IAddressesService addressService,
            IGroupsService grpService,
            ILogger<UsersController> logger,
            HttpClient httpClient)
        {
            _context = context;
            _empService = empservice;
            _addressService = addressService;
            _grpService = grpService;
            _logger = logger;
            _httpClient = httpClient;
        }
        //Access denied
        [Route("AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        
        [AllowAnonymous]
        
        public async Task<IActionResult> Landing()
        {
            if (User.Identity.IsAuthenticated)
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                var role = User.FindFirst("role")?.Value;
                if (role == "Admin")
                {
                    return RedirectToAction("LandingAdminView","Admin");
                }
                else if (role == "Associate")
                {
                    
                    return RedirectToAction("Dashboard", "Associate");
                    
                }
                else
                {
                    var userId = _context.Users
                          .Where(u => u.Email == email)
                          .Select(u => u.Id)
                          .FirstOrDefault();
                    if (userId < 1)
                    {
                        return RedirectToAction("Profile","Client");
                    }
                    else
                    {
                        return RedirectToAction("Dashboard","Client");
                        
                    }
                }
            }
            return View();
        }
        
    }
}



