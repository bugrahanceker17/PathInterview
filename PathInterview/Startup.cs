using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace PathInterview
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            services.AddHttpContextAccessor();
            
            services.AddAutoMapper(typeof(Startup));

            // services.AddDbContext<ProjectDbContext>();
            
            // services.AddIdentity<User, IdentityUser>(_ =>
            //     {
            //         _.Password.RequireNonAlphanumeric = false;
            //         _.Password.RequireLowercase = false;
            //         _.Password.RequireUppercase = false;
            //         _.User.RequireUniqueEmail = true;
            //     })
            //     .AddRoles<IdentityRole>()
            //     .AddEntityFrameworkStores<ProjectDbContext>();
            

            // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //     .AddJwtBearer(options =>
            //     {
            //         options.TokenValidationParameters = new TokenValidationParameters
            //         {
            //             ValidateIssuer = true,
            //             ValidateAudience = true,
            //             ValidateLifetime = true,
            //             ValidIssuer = tokenOptions.Issuer,
            //             ValidAudience = tokenOptions.Audience,
            //             ValidateIssuerSigningKey = true,
            //             IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey),
            //             LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null && expires > DateTime.UtcNow,
            //             NameClaimType = ClaimTypes.Name,
            //             RoleClaimType = ClaimTypes.Role,
            //             SaveSigninToken = true
            //         };
            //     });

            services.AddCors(options => 
                options.AddDefaultPolicy(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
            
            // services.SwaggerRegister("MsInfra-WebService", "v1");
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors();

            // app.SwaggerConfigure("MsInfra-WebService", "v1");

            app.UseHsts();

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseAuthentication();

            app.UseAuthorization();
            
            using (IServiceScope scope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope())
            {
                // scope?.ServiceProvider.GetRequiredService<ProjectDbContext>().Database.Migrate();
            }

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            // app.UseEndpoints(endpoints => { endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World!"); }); });
        }
    }
}