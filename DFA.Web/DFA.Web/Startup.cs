using System;
using System.Buffers;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using AutoMapper;

using DFA.Common.Extensions;

using DFA.Web.Api.Events;
using DFA.Web.Authentication;
using DFA.Web.Configuration;
using DFA.Web.Filters;
using DFA.Web.Mapping;
using DFA.Web.Models;
using DFA.Web.Models.Entities;
using DFA.Web.Services;
using DFA.Web.Services.Interfaces;

namespace DFA.Web
{
    public class Startup
    {
        /**********************************************************************/
        #region Constructors

        /// <summary>
        /// Initializes and configures a new instance of the application.
        /// </summary>
        /// <param name="configuration">The value to use for <see cref="Configuration"/>.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            // Initialize AutoMapper
            Mapper.Initialize(mapperConfig =>
            {
                mapperConfig.BuildMappings(System.Reflection.Assembly.GetExecutingAssembly());
            });
        }

        #endregion Constructors

        /**********************************************************************/
        #region Properties

        /// <summary>
        /// Provides configuration data from the "appsettings.json" file.
        /// </summary>
        public IConfiguration Configuration { get; }

        #endregion Properties

        /**********************************************************************/
        #region Methods

        /// <summary>
        /// Adds services to, and configures services within, the application, via the given <see cref="IServiceCollection"/> object.
        /// </summary>
        /// <param name="services">A <see cref="IServiceCollection"/> to which application services can be added, and from which existing services can be retrieved and configured.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Deserialize, and validate, all our IConfiguration options, and save it for easier access by the rest of the application.
            var appConfiguration = Configuration.Get<AppConfiguration>();
            appConfiguration.Validate();
            services.AddSingleton(appConfiguration);

            // Publish the Entity Framework database
            services.AddDbContext<DFAWebDataContext>(options =>
            {
                options.UseSqlite(appConfiguration.DataContext.ConnectionString);
            });

            // Setup JWT Bearer Authentication
            services
                .AddAuthentication(options =>
                {
                    var schemeName = appConfiguration.Authentication.DefaultScheme;

                    options.DefaultScheme = schemeName;
                    options.DefaultAuthenticateScheme = schemeName;
                    options.DefaultChallengeScheme = schemeName;
                    options.DefaultForbidScheme = schemeName;
                })
                .AddJwtBearer(options =>
                {
                    var schemeName = appConfiguration.Authentication.DefaultScheme;
                    var schemeConfiguration = appConfiguration.Authentication.Schemes.First(x => x.Name == schemeName);

                    options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(schemeConfiguration.SigningKey.ToBase64ByteArray());

                    options.TokenValidationParameters.ValidateIssuer = true;
                    options.TokenValidationParameters.ValidIssuer = schemeConfiguration.TokenIssuer;

                    options.TokenValidationParameters.ValidateAudience = true;
                    options.TokenValidationParameters.ValidAudience = schemeConfiguration.TokenAudience;

                    options.TokenValidationParameters.ValidateLifetime = true;

                    options.Events = new AppAuthenticationEvents();
                });

            // Setup authorization policies (just the defaults)
            services.AddAuthorization();

            // Setup SignalR and Events services
            services.AddSignalR();
            services.AddSingleton<IApiEventsService, ApiEventsService>();

            // Register application services
            services.AddSingleton<ILoginCredentialService, LoginCredentialService>();
            services.AddScoped<IAppAuthenticationService, AppAuthenticationService>();
            services.AddScoped<IDeferredActionService, DeferredActionService>();
            services.AddScoped<ILoggingService, LoggingService>();

            // Setup ASP.NET Core MVC services
            services.AddMvc(options =>
            {
                //options.Conventions.Add(new ApiEventSubscriptionConvention());
            });
        }

        /// <summary>
        /// Configures the application itself by defining the request pipeline.
        /// </summary>
        /// <param name="app">An <see cref="IApplicationBuilder"/> which will be used to build the request pipeline.</param>
        /// <param name="env">An <see cref="IHostingEnvironment"/> object containing information about the <see cref="IWebHost"/> that will host the application.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // Allows for live-editing of the client application.
                // This MUST be the first item in the pipeline, as it need to intercept all requests, in order to force a client-side update, when necessary.
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });

                // Display any unhandled exceptions in detail.
                // This also must be near the front of the pipeline, as the application should already has global exception handlers in the pipeline.
                // This simply ensures that if a mistake is made, the developer at least sees SOMETHING.
                app.UseDeveloperExceptionPage();
            }

            // Authenticate the request before any other normal operations.
            app.UseAuthentication();

            // Serve up files in the "wwwroot" directory, if requested.
            app.UseStaticFiles();

            // Accept SignalR connections.
            app.UseSignalR(routes =>
            {
                routes.MapHub<ApiEventsHub>("/api/events");
            });

            // Handle all other requests with MVC Controllers
            app.UseMvc(routes =>
            {
                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "App", action = "Get" });
            });
        }

        #endregion Methods
    }
}
