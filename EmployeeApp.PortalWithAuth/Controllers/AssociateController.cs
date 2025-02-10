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
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;

namespace EmployeeApp.PortalWithAuth.Controllers
{

    public class AssociateController : Controller
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
        public AssociateController(
            
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
        public async Task<IActionResult> FileUpload()
        {
            return View();
        }
        /*[HttpPost]*/
        /*public async Task<IActionResult> FileUpload(FileUploadModel model)
        {
            if (model.file != null && model.file.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await model.file.CopyToAsync(memoryStream);

                    var fileUpload = new FileUpload
                    {
                        FileName = model.file.FileName,
                        FileContent = memoryStream.ToArray(),
                        ContentType = model.file.ContentType
                    };

                    _context.FileUploads.Add(fileUpload);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Dashboard");
                }
            }

            return View();

        }*/
        public async Task<IActionResult> Dashboard()
        {
            return View();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> DetailsAssociate(int id)
        {
            ActionResult<User> actionResult = await _empService.GetUser(id);
            User user = await ConvertActionResultToUserAsync(actionResult);


            ViewBag.empEditId = user.Id;
            ViewBag.addressEditId = user.address.Id;
            var model = new UserViewModel();

            model.Name = user.Name;
            model.Email = user.Email;
            model.Phone = user.Phone;
            model.DateOfJoining = user.DateOfJoining;
            model.AddressLine1 = user.address.AddressLine1;
            model.AddressLine2 = user.address.AddressLine2;
            model.State = user.address.State;
            model.Country = user.address.Country;
            if (user.GroupId != null)
            {
                model.GroupName = user.Group.Name;
            }
            else
            {
                model.GroupName = "Not assigned";
            }
            List<Leave> leavesApplied =(await _leaveService.GetLeavesByUserId(id)).ToList() ;
                
            model.leavesApplied = leavesApplied;
            List<Certification> certifications = (await _certService.GetCertificationsByUserId(id)).ToList() ;
                
            var models = new List<CertificateViewModel>();
            foreach (var certification in certifications)
            {
                models.Add(new CertificateViewModel
                {
                    CertifiedOn = certification.CertifiedOn,
                    Expiration = certification.Expiration,
                    QualifiedFor = certification.QualifiedFor,
                    FileId = certification.Id // Unique identifier for the certification
                });
            }
            model.certifications = models;
            return View(model);
        }
        
        public async Task<IActionResult> CreateLeave()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateLeave(LeaveViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUserEmail = User.FindFirst(ClaimTypes.Email).Value;
                var userId = await _userService.GetByEmail(currentUserEmail);
                    
                var leave = new Leave
                {
                    From = model.From,
                    To = model.To,
                    PurposeOfLeave = model.PurposeOfLeave,
                    Note = model.Note,
                    UserId = userId.Id,
                    statusId=1
                };
                await _leaveService.UpdateLeaveAsync(leave);
                
                return RedirectToAction("MyLeaves");
            }

            return BadRequest();
        }
        
        public async Task<IActionResult> AddCertificate()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> SaveCertificate(CertificateViewModel model)
        {
            using var memoryStream = new MemoryStream();
            var currentUserEmail = User.FindFirst(ClaimTypes.Email).Value;
            var userId = await _userService.GetByEmail(currentUserEmail);

            if (model.file != null && model.file.Length > 0)
            {
                await model.file.CopyToAsync(memoryStream);
            }

            var certification = new Certification
            {
                CertifiedOn = model.CertifiedOn,
                QualifiedFor = model.QualifiedFor,
                Expiration = model.Expiration,
                UserId = userId.Id,
                file = memoryStream.ToArray(),  // Convert MemoryStream to byte array
                statusId = 1 // Assuming some status ID
            };
            await _certService.UpdateCertificationAsync(certification);
            return RedirectToAction("MyCertifications");
        }

        public async Task<IActionResult> MyLeaves()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var userId = await _userService.GetByEmail(email);
            List<Leave> leavesApplied = (await _leaveService.GetLeavesByUserId(userId.Id)).ToList();
            return View(leavesApplied);
        }
        public async Task<IActionResult> MyCertifications()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var userId = await _userService.GetByEmail(email);

            var certifications =await _certService.GetCertificationsByUserId(userId.Id);

            var models = new List<CertificateViewModel>();
            foreach (var certification in certifications)
            {
                models.Add(new CertificateViewModel
                {
                    CertifiedOn = certification.CertifiedOn,
                    Expiration = certification.Expiration,
                    QualifiedFor = certification.QualifiedFor,
                    FileId = certification.Id,
                    status=certification.status.Name
                });
            }

            return View(models);
        }
        
        public async Task<IActionResult> DownloadFile(int id)
        {
            var certification = await _certService.GetCertificationByIdAsync(id);
            if (certification == null)
            {
                return NotFound();
            }

            var fileName = "downloaded_file"; // Default file name
            var contentType = "application/octet-stream"; // Default content type

            return File(certification.file, contentType, fileName);
        }

        public async Task<IActionResult> Edit(int id)
        {

            TempData["empEditId"] = id;
            

            if (id == null)
            {

                return NotFound();
            }

            ActionResult<User> actionResult = await _empService.GetUser(id);

            User user = await ConvertActionResultToUserAsync(actionResult);

            if (user == null)
            {
                return NotFound();
            }
            EAGViewModel eag = new EAGViewModel();

            eag.Name = user.Name;
            eag.AddressLine1 = user.address.AddressLine1;
            eag.AddressLine2 = user.address.AddressLine2;
            eag.State = user.address.State;
            eag.Country = user.address.Country;
            eag.Email = user.Email;
            eag.Phone = user.Phone;
            return View(eag);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EAGViewModel model)
        {
            int empEditId = (int)TempData["empEditId"];
            ActionResult<User> actionResult = await _empService.GetUser(empEditId);

            // Convert ActionResult<User> to User
            User emp = await ConvertActionResultToUserAsync(actionResult);
            var address = emp.address;

            address.AddressLine1 = model.AddressLine1;
            address.AddressLine2 = model.AddressLine2;
            address.State = model.State;
            address.Country = model.Country;
            address.LastModifiedPersonId = empEditId;
            await _addressService.PutAddress(address);
           
            emp.Name = model.Name.ToUpper();
            emp.Email = model.Email;
            emp.Phone = model.Phone;
            emp.LastModifiedPersonId = empEditId;
            await _empService.PutUser(emp);
            return RedirectToAction("Dashboard");
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



