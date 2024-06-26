using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ServerApp.Data;
using ServerApp.Models;
using Newtonsoft.Json;
using AutoMapper;
using System.IO;
using Microsoft.Extensions.FileProviders;

namespace ServerApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.

         readonly string MyAllowOrigins = "_MyAllowOrigins";
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ShopContext>(x=>x.UseSqlServer("server=.\\SQLEXPRESS; database=Shop.db; integrated Security=SSPI;"));
            services.AddIdentity<User,Role>().AddEntityFrameworkStores<ShopContext>();
            services.AddScoped<IProductRepository,ProductRepository>();
            services.AddScoped<ISupplierRepository,SupplierRepository>();
            services.AddScoped<IUserRepository,UserRepository>();
            services.AddScoped<ICategoryRepository,CategoryRepository>();
            services.Configure<IdentityOptions>(options=>{
                options.Password.RequireDigit=true;
                options.Password.RequireLowercase=true;
                options.Password.RequiredLength=6;
                options.Password.RequireNonAlphanumeric = false;

                options.User.AllowedUserNameCharacters="abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail=true;
            });
            services.AddAutoMapper(typeof(Startup));
            services.AddControllers().AddNewtonsoftJson(options =>{
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            services.AddCors(options=>{
                options.AddPolicy(
                    name:MyAllowOrigins,
                    builder=>{
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();                    
                    }
                );
            });
            services.AddAuthentication(x=>{
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                 x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(x =>{
               x.RequireHttpsMetadata=false;
               x.SaveToken=true;
               x.TokenValidationParameters=new TokenValidationParameters{
                ValidateIssuerSigningKey=true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes( Configuration.GetSection("AppSettings:Secret").Value)),
                ValidateIssuer=false,
                ValidateAudience=false
                
               };

             });
             services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("Customer", policy => policy.RequireRole("Customer"));
    
});                                    
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();
             
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(env.ContentRootPath, "wwwroot")),
    RequestPath = ""
});



            app.UseRouting();


            app.UseAuthentication();

            app.UseCors(MyAllowOrigins);

            app.UseAuthorization();

            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
