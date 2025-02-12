using EmployeeApp.Data.Data;
using EmployeeApp.Data.Models;
using EmployeeApp.PortalWithAuth.Models;
using EmployeeApp.ServiceApi.Controllers.AdressesService;
using EmployeeApp.ServiceApi.Controllers.GroupsService;
using EmployeeApp.ServiceApi.Controllers.UsersService;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Security.Claims;
using ClosedXML.Excel;
using EmployeeApp.Services.CertificationServiceFolder;
using EmployeeApp.Services.LeaveServiceFolder;
using EmployeeApp.Services.ProgramServiceFolder;
using EmployeeApp.Services.SectionServiceFolder;
using EmployeeApp.Services.TopicServiceFolder;
using EmployeeApp.Services.UserRoleServiceFolder;
using EmployeeApp.Services.ProgramApplicationServiceFolder;
using EmployeeApp.Services.UserServiceFolder;
using EmployeeApp.Services.StatusServiceFolder;

namespace EmployeeApp.PortalWithAuth.Controllers
{
    [Authorize(Policy = "CustomPolicy")]
    public class AdminController : Controller
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
        public AdminController(
            
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
            _statusService= statusService;
            _programService = programService;
            _topicService = topicService;
            _sectionService = sectionService;
            _userRoleService = userRoleService;
            _programApplicationService = programApplicationService;
        }
        
        
        public async Task<IActionResult> LandingAdminView()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> addNewGroup()
        {
            return View(new Group());
        }
        [HttpPost]
        public async Task<IActionResult> addNewGroup([Bind("Name")] Group g)
        {
            ViewBag.Message = "Group added successfully";
            var newGrp= new Group{
                Name = g.Name,
                CreatedDate = DateTime.UtcNow,
                CreatedPersonId = 1,
                LastModified = DateTime.UtcNow,
                LastModifiedPersonId = 1
            };
            await _grpService.PostGroup(newGrp);
            return View(new Group());
        }
        public async Task<IActionResult> Index()
        {
            // Await the async call to get the result
            ActionResult<IEnumerable<User>> actionResult = await _empService.GetUsers();

            // Convert the ActionResult to IEnumerable<User>
            IEnumerable<User> listOfUsers = await ConvertActionResultToUsersAsync(actionResult);

            // Pass the list of users to the view
            return View(listOfUsers);
        }
        
        public async Task<IActionResult> IndexAjax()
        {
            return View();
        }
        
        public async Task<IActionResult> IndexAjax1()
        {
            return View();
        }
        
        public async Task<IActionResult> DetailsAdminView(int id)
        {
            ActionResult<User> actionResult = await _empService.GetUser(id);

            // Convert ActionResult<User> to User
            User user = await ConvertActionResultToUserAsync(actionResult);
            ViewBag.empEditId = user.Id;
            ViewBag.addressEditId = user.address.Id;
            var model = new UserViewModel();
            var userViewingRole = await _userRoleService.GetUserRoleByIdAsync(user.Id);
            var userViewingRoleName = userViewingRole.role.RoleName;
            ViewBag.ViewingUserRole = userViewingRoleName;
            if (userViewingRoleName != "Client")
            {
                model.Name = user.Name;
                model.Email = user.Email;
                model.Phone = user.Phone;
                model.DateOfJoining = user.DateOfJoining;
                model.IsActive = user.IsActive;
                model.AddressLine1 = user.address.AddressLine1;
                model.AddressLine2 = user.address.AddressLine2;
                model.State = user.address.State;
                model.Country = user.address.Country;
                if (user.Group != null)
                {
                    model.GroupName = user.Group.Name;
                }
                
                
                List<Leave> leavesApplied = (List<Leave>)await _leaveService.GetLeavesByUserId(id);
                model.leavesApplied = leavesApplied;
                List<Certification> certifications = await _certService.GetCertificationsByUserId(id);
                var models = new List<CertificateViewModel>();
                foreach (var certification in certifications)
                {
                    models.Add(new CertificateViewModel
                    {
                        CertifiedOn = certification.CertifiedOn,
                        Expiration = certification.Expiration,
                        QualifiedFor = certification.QualifiedFor,
                        FileId = certification.Id,
                        status = certification.status.Name
                    });
                }
                model.certifications = models;
            }
            else
            {
                model.Name = user.Name;
                model.Email = user.Email;
                model.Phone = user.Phone;
                model.DateOfJoining = user.DateOfJoining;
                model.IsActive = user.IsActive;
                model.AddressLine1 = user.address.AddressLine1;
                model.AddressLine2 = user.address.AddressLine2;
                model.State = user.address.State;
                model.Country = user.address.Country;
                if (user.Group != null)
                {
                    model.GroupName = user.Group.Name;
                }
                var appliedPrograms = await _programApplicationService.GetProgramApplicationsByUserId(id);

                List<string> programs = new List<string>();
                foreach (var program in appliedPrograms)
                {
                    var name = await _programService.GetProgramByIdAsync(program.ProgramId);
                        
                    programs.Add(name.Name);
                }
                model.programsApplied=programs;
            }
            
            return View(model);
        }
        public async Task<IActionResult> DownloadFile(int id)
        {
            var certification = await _certService.GetCertificationByIdAsync(id);
                //_context.Certifications.FindAsync(id);

            if (certification == null)
            {
                return NotFound();
            }

            var fileName = "downloaded_file"; // Default file name
            var contentType = "application/octet-stream"; // Default content type

            return File(certification.file, contentType, fileName);
        }
        public async Task<IActionResult> RegisterAdminView()
        {
            var groupNamesAction = await _grpService.GetGroups();
            var groupNames = await ConvertActionResultToStringEnumerableAsync(groupNamesAction);
                //_context.Groups.Select(g => g.Name).ToList();
            groupNames.Insert(0, "Select");
            List<SelectListItem> groupSelectListItems = groupNames.Select(name => new SelectListItem
            {
                Value = name,
                Text = name
            }).ToList();

            ViewBag.GroupName = groupSelectListItems;
            return View();

        }
        
        public async Task<IActionResult> RegisterSaveAdmin
            ([Bind("Name,Email,Phone,Password,DateOfJoining,GroupName,IsActive,AddressLine1,AddressLine2,State,Country,Password,ConfirmPassword")] EAGViewModel eag)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var address = new Address
                    {
                        AddressLine1 = eag.AddressLine1,
                        AddressLine2 = eag.AddressLine2,
                        State = eag.State,
                        Country = eag.Country,
                        CreatedPersonId = 2,
                        LastModifiedPersonId = 2
                    };
                    ActionResult<Address> actionResult = await _addressService.PostAddress(address);

                    // Convert ActionResult<User> to User
                    address = await ConvertActionResultToAddressAsync(actionResult);


                    var user = new User
                    {
                        Name = eag.Name.ToUpper(),
                        DateOfJoining = eag.DateOfJoining,
                        AddressId = address.Id,
                        IsActive = eag.IsActive,
                        Email = eag.Email,
                        Phone = eag.Phone,
                        Password = eag.Password,
                        CreatedPersonId = 2,
                        LastModifiedPersonId = 2

                    };
                    ActionResult<User> actionResult1 = await _empService.PostUser(user);

                    User emp = await ConvertActionResultToUserAsync(actionResult1);
                    UserRole role = new UserRole
                    {
                        UserId = emp.Id,
                        RoleId = 3
                    };
                    await _userRoleService.AddUserRoleAsync(role);
                    
                    return RedirectToAction(nameof(DetailsAdminView), new { id = user.Id });
                }
                catch (Exception e)
                {
                    _logger.LogInformation(e.Message);
                }

            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    _logger.LogError(error.ErrorMessage);
                }
            }
            return NotFound();

        }

        public async Task<IActionResult> EditAdminView(int id)
        {
            TempData["empEditId"] = id;
            //TempData["addEditId"] = addressid;
            // Fetch the user
            ActionResult<User> actionResult = await _empService.GetUser(id);
            User user = await ConvertActionResultToUserAsync(actionResult);

            if (user == null)
            {
                return NotFound();
            }

            EAGViewModel eag = new EAGViewModel
            {
                Name = user.Name,
                DateOfJoining = user.DateOfJoining,
                GroupName = user.Group?.Name ?? "Not assigned",
                AddressLine1 = user.address.AddressLine1,
                AddressLine2 = user.address.AddressLine2,
                State = user.address.State,
                Country = user.address.Country,
                Email = user.Email,
                Phone = user.Phone,
                IsActive = user.IsActive,
            };

            // Fetch the groups
            ActionResult<IEnumerable<string>> groupActionResult = await _grpService.GetGroups();

            // Convert the ActionResult to IEnumerable<string>
            IEnumerable<string> groupNames = await ConvertActionResultToStringEnumerableAsync(groupActionResult);

            // Insert "Select" at the beginning of the list
            List<string> groupNamesList = groupNames.ToList();
            groupNamesList.Insert(0, "Select");

            // Convert to List<SelectListItem>
            List<SelectListItem> groupSelectListItems = groupNamesList.Select(name => new SelectListItem
            {
                Value = name,
                Text = name
            }).ToList();

            ViewBag.GroupNames = groupSelectListItems;
            var statuses=await _statusService.GetAllStatusesAsync();
            List<string> statusList = new List<string>();
            foreach (var stat in statuses)
            {
                statusList.Add(stat.Name);
            }
            statusList.Insert(0, "Select");
            List<SelectListItem> statusSelectListItems = statusList.Select(name => new SelectListItem
            {
                Value = name,
                Text = name
            }).ToList();
            ViewBag.StatusNames = statusSelectListItems;
            return View(eag);
        }
        [Route("EditClickedByAdmin")]
        public async Task<IActionResult> EditClickedByAdmin
            ([Bind("Name,DateOfJoining,AddressLine1,AddressLine2,State,Country,Email,Phone,GroupName,IsActive")]
                EAGViewModel eag)
        {
            int empEditId = (int)TempData["empEditId"];
            int addEditId = (int)TempData["addEditId"];

            // Await the task to get ActionResult<User>
            ActionResult<User> actionResult = await _empService.GetUser(empEditId);

            // Convert ActionResult<User> to User
            User emp = await ConvertActionResultToUserAsync(actionResult);

            // Check if the address is already being tracked
            var existingAddress = await ConvertActionResultToAddressAsync(await _addressService.GetAddress(emp.address.Id));
                //await _context.Addresses.FindAsync();
            /*if (existingAddress != null)
            {
                _context.Entry(existingAddress).State = EntityState.Detached;
            }*/

            // Update the address properties
            var address = emp.address;
            address.AddressLine1 = eag.AddressLine1;
            address.AddressLine2 = eag.AddressLine2;
            address.State = eag.State;
            address.Country = eag.Country;
            address.LastModifiedPersonId = 2;

            // Attach and update the address
            /*_context.Attach(address);
            _context.Update(address);
            await _context.SaveChangesAsync();*/

            await _addressService.PutAddress(address);

            // Update the user properties
            emp.Name = eag.Name.ToUpper();
            emp.Email = eag.Email;
            emp.Phone = eag.Phone;
            emp.DateOfJoining = eag.DateOfJoining;
            emp.IsActive = eag.IsActive;
            _logger.LogInformation("" + eag.GroupName);
            if (eag.GroupName != "Select")
            {
                emp.GroupId =await ConvertActionResultToIntAsync(await _grpService.GetGroupId(eag.GroupName));
                    
            }
            else
            {
                emp.GroupId = null;
                emp.Group = null;
            }
            emp.LastModifiedPersonId = 2;

            // Update the user using the service
            await _empService.PutUser(emp);

            return RedirectToAction(nameof(DetailsAdminView), new { id = emp.Id });
        }
        
        public async Task<IActionResult> Delete(int id)
        {
            ViewBag.DelId = id;
            _logger.LogInformation("inside del");
            ActionResult<User> actionResult = await _empService.GetUser(id);

            // Convert ActionResult<User> to User
            User employee = await ConvertActionResultToUserAsync(actionResult);
            return View(employee);
        }

        
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("inside del confirmed");
            await _empService.DeleteUser(id);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ListAssociates()
        {
            var associateUsers = await _userService.listAssoc();
            DateTime today = DateTime.Today;
            DateTime twentyDaysFromNow = today.AddDays(20);

            var upcomingLeaves = await _leaveService.UpcomingLeaves();
            var upcomingLeavesList = new List<UpcomingLeaveModel>();
            foreach (var x in upcomingLeaves)
            {
                upcomingLeavesList.Add(new UpcomingLeaveModel
                {
                    leaveId = x.Id,
                    Name = x.user.Name,
                    From = x.From,
                    To = x.To,
                    PurposeOfLeave = x.PurposeOfLeave,
                    status = x.status.Name
                });
            }

            var model = Tuple.Create(associateUsers, upcomingLeavesList);
            return View(model);
            
        }
        public async Task<IActionResult> ListAdmins()
        {
            var adminUsers = await _userService.listAdmins();
            return View(adminUsers);
        }
        public async Task<IActionResult> ListClients()
        {
            var clientUsers = await _userService.listClients();
            return View(clientUsers);
        }
        public async Task<IActionResult> EditLeaveStatusfromUpcoming(int leaveId)
        {
            var email = User.FindFirst(ClaimTypes.Email).Value;
            var user = await ConvertActionResultToUserAsync(await _empService.getUserByMail(email));
            ViewBag.userName = user.Name;
            var leave = _leaveService.GetLeaveByIdAsync(leaveId);
            var statuses=await _statusService.GetAllStatusesAsync();
            List<string> statusList = new List<string>();
            foreach (var stat in statuses)
            {
                statusList.Add(stat.Name);
            }
            statusList.Insert(0, "Select");
            List<SelectListItem> statusSelectListItems = statusList.Select(name => new SelectListItem
            {
                Value = name,
                Text = name
            }).ToList();
            ViewBag.StatusNames = statusSelectListItems;
            return View(leave);
        }
        [HttpPost]
        public async Task<IActionResult> EditLeaveStatusfromUpcoming(Leave model)
        {
            var leave = await _leaveService.GetLeaveByIdAsync(model.Id);
            var statusId = await _statusService.GetByNameAsync(model.status.Name);
                //_context.Statuses.FirstOrDefault(x => x.Name == model.status.Name);
            leave.statusId = statusId.Id;
            await _leaveService.UpdateLeaveAsync(leave);
            
            return RedirectToAction("ListAssociates");
        }
        
        public async Task<IActionResult> EditLeaveStatus(int leaveId)
        {
            var email = User.FindFirst(ClaimTypes.Email).Value;
            var user = await ConvertActionResultToUserAsync(await _empService.getUserByMail(email));
            //_context.Users.FirstOrDefault(u => u.Email == email);
            ViewBag.userName = user.Name;
            var leave =await _leaveService.GetLeaveByIdAsync(leaveId);
            //_context.Leaves.Include(u=>u.status).FirstOrDefault(x=>x.Id==leaveId);
            var statuses = await _statusService.GetAllStatusesAsync();
            List<string> statusList = new List<string>();
            foreach (var stat in statuses)
            {
                statusList.Add(stat.Name);
            }
            statusList.Insert(0, "Select");
            List<SelectListItem> statusSelectListItems = statusList.Select(name => new SelectListItem
            {
                Value = name,
                Text = name
            }).ToList();
            ViewBag.StatusNames = statusSelectListItems;
            return View(leave);
        }
        [HttpPost]
        public async Task<IActionResult> EditLeaveStatus(Leave model)
        {
            var leave = await _leaveService.GetLeaveByIdAsync(model.Id);
            var statusId = await _statusService.GetByNameAsync(model.status.Name);
            leave.statusId = statusId.Id;
            await _leaveService.UpdateLeaveAsync(leave);
            
            return RedirectToAction(nameof(DetailsAdminView), new {id=leave.UserId});
        }
        public async Task<IActionResult> EditCertificationStatus(int certId)
        {
            var email = User.FindFirst(ClaimTypes.Email).Value;
            var user = await ConvertActionResultToUserAsync(await _empService.getUserByMail(email));
            ViewBag.userName = user.Name;
            var certificate = await _certService.GetCertificationByIdAsync(certId);
            var certificateView = new CertificateViewModel
            {
                CertifiedOn = certificate.CertifiedOn,
                Expiration = certificate.Expiration,
                QualifiedFor = certificate.QualifiedFor,
                FileId = certificate.Id,
                status = certificate.status.Name
            };
            var statuses = await _statusService.GetAllStatusesAsync();
            List<string> statusList = new List<string>();
            foreach (var stat in statuses)
            {
                statusList.Add(stat.Name);
            }
            statusList.Insert(0, "Select");
            List<SelectListItem> statusSelectListItems = statusList.Select(name => new SelectListItem
            {
                Value = name,
                Text = name
            }).ToList();
            ViewBag.StatusNames = statusSelectListItems;
            return View(certificateView);
        }
        [HttpPost]
        public async Task<IActionResult> EditCertificationStatus(CertificateViewModel model)
        {
            var certificate =await _certService.GetCertificationByIdAsync(model.FileId);
            var statusId =  await _statusService.GetByNameAsync(model.status);
            certificate.statusId = statusId.Id;
            await _certService.UpdateCertificationAsync(certificate);
            
            return RedirectToAction(nameof(DetailsAdminView), new { id = certificate.UserId });
        }

        public async Task<IActionResult> AddProgram()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddProgram(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.Error = "Please select an Excel file.";
                return View("Index");
            }

            var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            await ProcessExcelFileAsync(stream);

            ViewBag.Message = "File upload success";
            return View();
        }
        public async Task<IActionResult> AllProgramApplications()
        {
            var allapplications = await _programApplicationService.GetAllProgramApplicationsAsync();
            List<AppliedProgram> applications = new List<AppliedProgram>();
            foreach (var item in allapplications)
            {
                var programName = await _programService.GetProgramByIdAsync(item.ProgramId);
                var applicantName = await _userService.GetDetails(item.ApplicantId);
                applications.Add(new AppliedProgram
                {
                    programName=programName.Name,
                    applicantName=applicantName.Name
                });
            }
            return View(applications);
        }
        public async Task<IActionResult> UpcomingLeaves()
        {
            var associateUsers = await _userService.listAssoc();


            var upcomingLeaves = await _leaveService.UpcomingLeaves();
            var upcomingLeavesList = new List<UpcomingLeaveModel>();
            foreach (var x in upcomingLeaves)
            {
                upcomingLeavesList.Add(new UpcomingLeaveModel
                {
                    leaveId = x.Id,
                    Name = x.user.Name,
                    From = x.From,
                    To = x.To,
                    PurposeOfLeave = x.PurposeOfLeave,
                    status = x.status.Name
                });
            }
            var model = Tuple.Create(associateUsers, upcomingLeavesList);
            return View(model);
        }

        //helpers
        public async Task ProcessExcelFileAsync(Stream fileStream)
        {
            using var workbook = new XLWorkbook(fileStream);

            var programs = new List<EmployeeApp.Data.Models.Program>();
            var sections = new List<Section>();
            var topics = new List<Topic>();

            // Process Programs Sheet
            var programSheet = workbook.Worksheet("Programs");
            foreach (var row in programSheet.RowsUsed().Skip(1))
            {
                var programId = row.Cell(1).GetValue<int>();
                var programName = row.Cell(2).GetValue<string>();
                var programDescription = row.Cell(3).GetValue<string>();

                // Check if the program already exists in the database
                var existingProgram = await _programService.GetProgramByIdAsync(programId);
                if (existingProgram == null)
                {
                    var program = new EmployeeApp.Data.Models.Program
                    {
                        Id = programId,
                        Name = programName,
                        Description = programDescription,
                        CreatedDate = DateTime.UtcNow,
                        CreatedPersonId = 1, // Example value
                        LastModified = DateTime.UtcNow,
                        LastModifiedPersonId = 1 // Example value
                    };
                    programs.Add(program);
                }
            }

            // Process Sections Sheet
            var sectionSheet = workbook.Worksheet("Sections");
            foreach (var row in sectionSheet.RowsUsed().Skip(1))
            {
                var sectionId = row.Cell(1).GetValue<int>();
                var sectionName = row.Cell(2).GetValue<string>();
                var sectionDescription = row.Cell(3).GetValue<string>();
                var programId = row.Cell(4).GetValue<int>();

                // Check if the section already exists in the database
                var existingSection = await _sectionService.GetSectionByIdAsync(sectionId);
                if (existingSection == null)
                {
                    var section = new Section
                    {
                        Id = sectionId,
                        Name = sectionName,
                        Description = sectionDescription,
                        ProgramId = programId,
                        CreatedDate = DateTime.UtcNow,
                        CreatedPersonId = 1, // Example value
                        LastModified = DateTime.UtcNow,
                        LastModifiedPersonId = 1 // Example value
                    };
                    sections.Add(section);
                }
            }

            // Process Topics Sheet
            var topicSheet = workbook.Worksheet("Topics");
            foreach (var row in topicSheet.RowsUsed().Skip(1))
            {
                var topicId = row.Cell(1).GetValue<int>();
                var topicName = row.Cell(2).GetValue<string>();
                var topicDescription = row.Cell(3).GetValue<string>();
                var sectionId = row.Cell(4).GetValue<int>();
                var programId = row.Cell(5).GetValue<int>();

                // Check if the topic already exists in the database
                var existingTopic = await _topicService.GetTopicByIdAsync(topicId);
                if (existingTopic == null)
                {
                    var topic = new Topic
                    {
                        Id = topicId,
                        Name = topicName,
                        Description = topicDescription,
                        ProgramId = programId,
                        SectionId = sectionId,
                        CreatedDate = DateTime.UtcNow,
                        CreatedPersonId = 1, // Example value
                        LastModified = DateTime.UtcNow,
                        LastModifiedPersonId = 1 // Example value
                    };
                    topics.Add(topic);
                }
            }

            // Insert new records
            await _programService.AddProgramsAsync(programs);
            await _sectionService.AddSectionsAsync(sections);
            await _topicService.AddTopicsAsync(topics);
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

        private async Task<List<string>> ConvertActionResultToStringEnumerableAsync(ActionResult<IEnumerable<string>> actionResult)
        {
            if (actionResult.Result is OkObjectResult okResult)
            {
                return okResult.Value as List<string>;
            }
            return null; // Handle other status codes as needed
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
        private async Task<int> ConvertActionResultToIntAsync(ActionResult<int> actionResult)
        {
            _logger.LogInformation("inside ConvertActionResultToStringAsync");
            if (actionResult.Result is OkObjectResult okResult)
            {
                
                return (int)okResult.Value;
            }
            return 0; // Handle other status codes as needed
        }

    }
}
