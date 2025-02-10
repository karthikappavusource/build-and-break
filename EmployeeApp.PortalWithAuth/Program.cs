using EmployeeApp.Data.Data;
using EmployeeApp.Data.Interfaces.AddressRepo;
using EmployeeApp.Data.Interfaces.CertificationRepo;
using EmployeeApp.Data.Interfaces.GroupRepo;
using EmployeeApp.Data.Interfaces.LeaveRepo;
using EmployeeApp.Data.Interfaces.ProgramApplicationRepo;
using EmployeeApp.Data.Interfaces.ProgramRepo;
using EmployeeApp.Data.Interfaces.RoleRepo;
using EmployeeApp.Data.Interfaces.SectionRepo;
using EmployeeApp.Data.Interfaces.StatusRepo;
using EmployeeApp.Data.Interfaces.TopicRepo;
using EmployeeApp.Data.Interfaces.UserRepo;
using EmployeeApp.Data.Interfaces.UserRoleRepo;
using EmployeeApp.PortalWithAuth.Controllers;
using EmployeeApp.ServiceApi.Controllers.AdressesService;
using EmployeeApp.ServiceApi.Controllers.GroupsService;
using EmployeeApp.ServiceApi.Controllers.UsersService;
using EmployeeApp.Services.AddressServiceFolder;
using EmployeeApp.Services.CertificationServiceFolder;
using EmployeeApp.Services.LeaveServiceFolder;
using EmployeeApp.Services.ProgramApplicationServiceFolder;
using EmployeeApp.Services.ProgramServiceFolder;
using EmployeeApp.Services.RoleServiceFolder;
using EmployeeApp.Services.SectionServiceFolder;
using EmployeeApp.Services.StatusServiceFolder;
using EmployeeApp.Services.TopicServiceFolder;
using EmployeeApp.Services.UserRoleServiceFolder;
using EmployeeApp.Services.UserServiceFolder;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace EmployeeApp.PortalWithAuth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddLogging();
            builder.Services.AddControllersWithViews();
            
            builder.Services.AddDbContext<EmployeeDB2Context>(
            options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );
            //services
            builder.Services.AddScoped<IClaimsTransformation, CustomClaimsTransformation>();
            builder.Services.AddScoped<IUsersService, UsersServiceController>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAddressesService, AddressesServiceController>();
            builder.Services.AddScoped<IGroupsService, GroupsServiceController>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<ILeaveService, LeaveService>();
            builder.Services.AddScoped<ICertificationService, CertificationService>();
            builder.Services.AddScoped<IProgramService, ProgramService>();
            builder.Services.AddScoped<ISectionService, SectionService>();
            builder.Services.AddScoped<ITopicService, TopicService>();
            builder.Services.AddScoped<IProgramApplicationService, ProgramApplicationService>();
            builder.Services.AddScoped<IUserRoleService, UserRoleService>();
            builder.Services.AddScoped<IStatusService,StatusService>();


            //repos
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAddressRepository, AddressRepository>();
            builder.Services.AddScoped<IGroupRepository, GroupRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<ILeaveRepository, LeaveRepository>();
            builder.Services.AddScoped<ICertificationRepository, CertificationRepository>();
            builder.Services.AddScoped<IProgramRepository, ProgramRepository>();
            builder.Services.AddScoped<ISectionRepository, SectionRepository>();
            builder.Services.AddScoped<ITopicRepository, TopicRepository>();
            builder.Services.AddScoped<IProgramApplicationRepository, ProgramApplicationRepository>();
            builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            builder.Services.AddScoped<IStatusRepository, StatusRepository>();

            builder.Services.AddHttpClient();
            var key = Encoding.ASCII.GetBytes("your_secret_key_here");

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                x.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        string returnUrl = "https://localhost:7011" + context.Request.Path;
                        var loginUrl = $"https://localhost:7011/Account/Login?returnUrl={returnUrl}";
                        context.Response.Redirect(loginUrl);
                        return Task.CompletedTask;
                    }
                };
            })
            .AddCookie("Cookies");
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.MaxAge = TimeSpan.FromMinutes(10);

            });
            
            builder.Services.AddAuthorization(options =>
            {
                
                options.AddPolicy("CustomPolicy", policy =>
                {
                    policy.RequireClaim("role", "Admin");
                });
                options.AddPolicy("ClientClaim", policy =>
                {
                    policy.RequireClaim("role", "Client");
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();
            app.Use(async (context, next) =>
            {
                if(context.Session.IsAvailable==false)
                {

                }

                await next();
            });
            app.Use(async (context, next) =>
            {
                Console.WriteLine(context.User.Identity.IsAuthenticated + " " + context.Request.Path + "" + context.Request.Method);
                if (context.Session != null&& context.Session.GetString("JWTtoken")!=null)
                {
                    var tokenHeader = "Bearer" + " " + context.Session.GetString("JWTtoken");
                    context.Request.Headers.Add("Authorization", tokenHeader);

                    Console.WriteLine("this is the tokenHeader " + context.Session.GetString("JWTtoken"));
                    Console.WriteLine("this is the header " + tokenHeader);
                }
                else
                {
                    Console.WriteLine("session not initiated");
                }

                await next();
            });
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Users}/{action=Landing}/{id?}");
            

            app.Run();
        }
    }
}