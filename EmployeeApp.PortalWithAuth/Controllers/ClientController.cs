using EmployeeApp.Data.Data;
using EmployeeApp.Data.Models;
using EmployeeApp.PortalWithAuth.Models;
using EmployeeApp.ServiceApi.Controllers.AdressesService;
using EmployeeApp.ServiceApi.Controllers.GroupsService;
using EmployeeApp.ServiceApi.Controllers.UsersService;
using EmployeeApp.Services.CertificationServiceFolder;
using EmployeeApp.Services.LeaveServiceFolder;
using EmployeeApp.Services.ProgramApplicationServiceFolder;
using EmployeeApp.Services.ProgramServiceFolder;
using EmployeeApp.Services.SectionServiceFolder;
using EmployeeApp.Services.StatusServiceFolder;
using EmployeeApp.Services.TopicServiceFolder;
using EmployeeApp.Services.UserRoleServiceFolder;
using EmployeeApp.Services.UserServiceFolder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace EmployeeApp.PortalWithAuth.Controllers
{
    public class ClientController : Controller
    {
        
        private readonly IUsersService _empService;
        private readonly IUserService _userService;
        private readonly IAddressesService _addressService;
        private readonly IGroupsService _grpService;
        private readonly ICertificationService _certService;
        private readonly ILeaveService _leaveService;
        private readonly IStatusService _statusService;
        private readonly IProgramService _programService;
        private readonly ISectionService _sectionService;
        private readonly ITopicService _topicService;
        private readonly IUserRoleService _userRoleService;
        private readonly IProgramApplicationService _programApplicationService;
        private readonly ILogger<UsersController> _logger;
        private readonly HttpClient _httpClient;
        public ClientController(
            IUsersService empservice,
            IUserService userservice,
            IAddressesService addressService,
            IGroupsService grpService,
            ILogger<UsersController> logger,
            HttpClient httpClient,
            ICertificationService certService,
            ILeaveService leaveService,
            IStatusService statusService,
            IProgramService programService,
            ISectionService sectionService,
            ITopicService topicService,
            IUserRoleService userRoleService,
            IProgramApplicationService programApplicationService)
        {
            
            _empService = empservice;
            _userService = userservice;
            _addressService = addressService;
            _grpService = grpService;
            _logger = logger;
            _httpClient = httpClient;
            _certService = certService;
            _leaveService = leaveService;
            _statusService = statusService;
            _programService = programService;
            _topicService = topicService;
            _sectionService = sectionService;
            _userRoleService = userRoleService;
            _programApplicationService = programApplicationService;
        }
        public async Task<IActionResult> Profile()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ProfileSave(EAGViewModel model)
        {
            try
            {
                var address = new Address
                {
                    AddressLine1 = model.AddressLine1,
                    AddressLine2 = model.AddressLine2,
                    State = model.State,
                    Country = model.Country,
                    CreatedPersonId = 1,
                    LastModifiedPersonId = 1
                };
                ActionResult<Address> actionResult = await _addressService.PostAddress(address);

                // Convert ActionResult<User> to User
                address = await ConvertActionResultToAddressAsync(actionResult);


                var user = new User
                {
                    Name = model.Name.ToUpper(),
                    DateOfJoining = model.DateOfJoining,
                    AddressId = address.Id,
                    IsActive = model.IsActive,
                    Email = User.FindFirstValue(ClaimTypes.Email),
                    Phone = model.Phone,
                    Password = model.Password,
                    CreatedPersonId = 1,
                    LastModifiedPersonId = 1

                };
                ActionResult<User> actionResult1 = await _empService.PostUser(user);

                User emp = await ConvertActionResultToUserAsync(actionResult1);
                UserRole role = new UserRole
                {
                    UserId = emp.Id,
                    RoleId = 4
                };
                await _userRoleService.AddUserRoleAsync(role);
                
                return RedirectToAction("Dashboard");
                //return RedirectToAction(nameof(Details), new { id = user.Id });
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return BadRequest();
            }
        }
        public async Task<IActionResult> Dashboard()
        {
            return View();
        }
        public async Task<IActionResult> ApplyStandard()
        {
            var programs=await _programService.GetAllProgramsAsync();
            List< StandardViewModel > programsView= new List< StandardViewModel >();
            foreach (var program in programs)
            {
                programsView.Add(new StandardViewModel()
                {
                    Id=program.Id,
                    Name = program.Name,
                    Description = program.Description,
                });
            }
            return View(programsView);
        }
        public async Task<IActionResult> ProgramDetails(int id)
        {
            var program = await _programService.GetProgramByIdAsync(id);
                
            var sections=program.Sections.ToList();
            
            var model = new ProgramDetailsViewModel
            {
                Id = program.Id,
                Name = program.Name,
                Description = program.Description,
                sections = sections
            };
            return View(model);
        }
        public async Task<IActionResult> SectionDetails(int id)
        {
            var section=await _sectionService.GetSectionByIdAsync(id);
                
            var topics= section.Topics.ToList();
            var model = new SectionDetailsViewModel
            {
                Id = section.Id,
                Name = section.Name,
                Description = section.Description,
                topics = topics
            };
            return View(model);
        }
        public async Task<IActionResult> ApplyPage(int id)
        {
            var userIdString = User.FindFirstValue("Id");
            var userIdInt = Int32.Parse(userIdString);
            var appliedPrograms = await _programApplicationService.GetProgramApplicationsByUserId(userIdInt);
                
            List<int> programsIds = new List<int>();
            foreach (var program in appliedPrograms)
            {
                programsIds.Add(program.program.Id);
            }
            if(!programsIds.Contains(id)) 
            {
                var model = new ApplyPageViewModel
                {
                    ProgramId = id,
                };
                return View(model);
            }
            else
            {
                ViewBag.message = "You have already applied for this program!";
                return View();
            }
            
            
        }
        [HttpPost]
        public async Task<IActionResult> ApplyPage(ApplyPageViewModel model)
        {
            var userIdString = User.FindFirstValue("Id");
            var userIdInt=Int32.Parse(userIdString);
            var application = new ProgramApplication
            {
                ProgramId = model.ProgramId,
                ApplicantId=userIdInt
            };
            await _programApplicationService.AddProgramApplicationAsync(application);
            
            return RedirectToAction("ViewAppliedPrograms",new {id=application.ApplicantId});
        }
        public async Task<IActionResult> ViewAppliedPrograms(int id)
        {
            var appliedPrograms=await _programApplicationService.GetProgramApplicationsByUserId(id);
            List<AppliedProgram> programs = new List<AppliedProgram>();
            foreach (var program in appliedPrograms)
            {
                programs.Add(new AppliedProgram { programName = program.program.Name });
                
            }
            ViewBag.appliedPrograms = programs; 
            return View(programs);
        }



        //helpers
        private async Task<IEnumerable<User>> ConvertActionResultToUsersAsync(ActionResult<IEnumerable<User>> actionResult)
        {
            if (actionResult.Result is OkObjectResult okResult)
            {
                return okResult.Value as IEnumerable<User>;
            }
            return Enumerable.Empty<User>(); // Handle other status codes as needed
        }

        private async Task<IEnumerable<string>> ConvertActionResultToStringEnumerableAsync(ActionResult<IEnumerable<string>> actionResult)
        {
            if (actionResult.Result is OkObjectResult okResult)
            {
                return okResult.Value as IEnumerable<string>;
            }
            return Enumerable.Empty<string>(); // Handle other status codes as needed
        }

        private async Task<User> ConvertActionResultToUserAsync(ActionResult<User> actionResult)
        {
            if (actionResult.Result is OkObjectResult okResult)
            {
                // Serialize the Value to JSON
                string json = JsonConvert.SerializeObject(okResult.Value);

                // Deserialize the JSON to a User object
                User user = JsonConvert.DeserializeObject<User>(json);

                return user;
            }
            else
            {
                throw new InvalidOperationException("The action did not return a valid user.");
            }
        }

        private async Task<Address> ConvertActionResultToAddressAsync(ActionResult<Address> actionResult)
        {
            if (actionResult.Result is OkObjectResult okResult)
            {
                // Serialize the Value to JSON
                string json = JsonConvert.SerializeObject(okResult.Value);

                // Deserialize the JSON to a User object
                Address address = JsonConvert.DeserializeObject<Address>(json);

                return address;
            }
            else
            {
                throw new InvalidOperationException("The action did not return a valid address.");
            }
        }

        private async Task<string> ConvertActionResultToStringAsync(ActionResult<string> actionResult)
        {
            _logger.LogInformation("inside ConvertActionResultToStringAsync");
            if (actionResult.Result is OkObjectResult okResult)
            {
                _logger.LogInformation("inside ConvertActionResultToStringAsync if block");
                _logger.LogInformation("" + okResult.Value as string);
                return okResult.Value as string;
            }
            return null; // Handle other status codes as needed
        }


    }
}
