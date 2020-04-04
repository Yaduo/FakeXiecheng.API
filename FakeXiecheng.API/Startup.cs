using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using FakeXiecheng.API.AuthorizationRequriement;
using FakeXiecheng.API.DbContexts;
using FakeXiecheng.API.Helpers;
using FakeXiecheng.API.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using static FakeXiecheng.API.AuthorizationRequriement.FakeXiechengRequireClaim;

namespace FakeXiecheng.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme) // add DefaultAuthenticateScheme
            //    .AddCookie(options =>
            //    {
            //        options.Cookie.Name = "FakeXiecheng.Cookie";
            //        options.Events.OnRedirectToAccessDenied = AuthenticationHelper.CookieAuthReplaceRedirector(
            //            HttpStatusCode.Forbidden,
            //            options.Events.OnRedirectToAccessDenied
            //        );
            //        options.Events.OnRedirectToLogin = AuthenticationHelper.CookieAuthReplaceRedirector(
            //            HttpStatusCode.Unauthorized,
            //            options.Events.OnRedirectToLogin
            //        );
            //        //options.LoginPath = "/api";
            //    });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var secret = "sui_bian_xie_dian_zifuchuan";
                    var secretByte = Encoding.UTF8.GetBytes(secret);
                    var signingKey = new SymmetricSecurityKey(secretByte);

                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = "fakeXiecheng.com",

                        ValidateAudience = true,
                        ValidAudience = "fakeXiecheng.com",

                        ValidateLifetime = true,

                        IssuerSigningKey = signingKey
                    };
                });

            services.AddAuthorization(option =>
            {
                //var defaultAuthBuilder = new AuthorizationPolicyBuilder();
                //var defaultAuthPolicy = defaultAuthBuilder
                //.RequireAuthenticatedUser()  
                //.RequireClaim(ClaimTypes.Email)
                //.Build();
                //option.DefaultPolicy = defaultAuthPolicy;

                //option.AddPolicy("ClaimEmailMustToHave", policyBuilder => 
                //{
                //    policyBuilder.RequireClaim(ClaimTypes.Email);
                //});

                option.AddPolicy("ClaimEmailMustToHave", policyBuilder =>
                {
                    policyBuilder.AddRequirements(new FakeXiechengRequireClaim(ClaimTypes.Email));
                });

                // 可选, 使用方式：[Authorize(Policy = "Admin")]
                //option.AddPolicy("Admin", policyBuilder => policyBuilder.RequireClaim(ClaimTypes.Role, "Admin"));

            });

            services.AddScoped<IAuthorizationHandler, FakeXiechengRequireClaimHandler>();

            services.AddResponseCaching();

            services.AddControllers(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
                setupAction.CacheProfiles.Add(
                    "240SecondsCacheProfile",
                    new CacheProfile()
                    {
                        Duration = 240
                    }
                );
            })
            .AddNewtonsoftJson(setupAction =>
            {
                setupAction.SerializerSettings.ContractResolver =
                   new CamelCasePropertyNamesContractResolver();
            })
            .AddXmlDataContractSerializerFormatters()
            .ConfigureApiBehaviorOptions(setupAction =>
            {
                setupAction.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Type = "https://api.fakexiecheng.com/modelvalidationproblem",
                        Title = "One or more model validation errors occurred.",
                        Status = StatusCodes.Status422UnprocessableEntity,
                        Detail = "See the errors property for details.",
                        Instance = context.HttpContext.Request.Path
                    };
                    problemDetails.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);
                    return new UnprocessableEntityObjectResult(problemDetails)
                    {
                        ContentTypes = { "application/problem+json" }
                    };
                };
            });

            services.Configure<MvcOptions>(config =>
            {
                var newtonsoftJsonOutputFormatter = config.OutputFormatters
                      .OfType<NewtonsoftJsonOutputFormatter>()?.FirstOrDefault();

                if (newtonsoftJsonOutputFormatter != null)
                {
                    newtonsoftJsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.fakeXiecheng.hateoas+json");
                }
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<ITouristRouteRepository, TouristRouteRepository>();

            services.AddDbContext<TouristLibraryContext>(options =>
            {
                options.UseSqlServer(
                    @"Server=localhost; Database=FakeXiechengData; User Id=sa; Password=PaSSword12!;");
            });

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddTransient<IPropertyMappingService, PropertyMappingService>();

            services.AddTransient<IPropertyCheckerService, PropertyCheckerService>();

            services.AddHttpClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
                    });
                });
            }

            app.UseResponseCaching();

            // where you are?
            app.UseRouting();

            // who you are?
            app.UseAuthentication();

            // are you allowed?
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
