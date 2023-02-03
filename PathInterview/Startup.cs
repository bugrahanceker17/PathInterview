using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PathInterview.Core.Extensions;
using PathInterview.Core.Security;
using PathInterview.Core.Security.Encryption;
using PathInterview.DataAccess.Concrete;
using PathInterview.DataAccess.DataSeeding;
using PathInterview.Entities.Entity;
using PathInterview.Infrastructure.Abstract.Query;
using PathInterview.Infrastructure.Abstract.Service;
using PathInterview.Infrastructure.Concrete.Query;
using PathInterview.Infrastructure.Concrete.Service;

namespace PathInterview
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddHttpContextAccessor();

            services.AddAutoMapper(typeof(Startup));

            services.AddDbContext<ProjectDbContext>();

            services.AddSingleton<IAuthQuery, AuthQuery>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddSingleton<ITokenHelper, JwtHelper>();

            services.AddIdentity<User, IdentityRole>(_ =>
                {
                    _.Password.RequireNonAlphanumeric = false;
                    _.Password.RequireLowercase = false;
                    _.Password.RequireUppercase = false;
                    _.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ProjectDbContext>();
            
            TokenOptionsModel tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptionsModel>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = tokenOptions.Issuer,
                        ValidAudience = tokenOptions.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey),
                        LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null && expires > DateTime.UtcNow,
                        NameClaimType = ClaimTypes.Name,
                        RoleClaimType = ClaimTypes.Role,
                        SaveSigninToken = true
                    };
                });

            services.AddCors(options =>
                options.AddDefaultPolicy(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

            services.SwaggerRegister("Path-Interview", "v1");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                DataSeeding.Seed(app);
            }

            app.UseCors();

            app.SwaggerConfigure("Path-Interview", "v1");

            app.UseHsts();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            using (IServiceScope scope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope())
            {
                scope?.ServiceProvider.GetRequiredService<ProjectDbContext>().Database.Migrate();
            }

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            // app.UseEndpoints(endpoints => { endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World!"); }); });
        }
    }
}