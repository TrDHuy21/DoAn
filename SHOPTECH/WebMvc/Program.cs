using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WebMvc.Config;
using WebMvc.Middleware;

namespace WebMvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.WithOrigins("https://localhost:5114", "http://localhost:5114")
                                          .AllowAnyMethod()
                                          .AllowAnyHeader()
                                          .AllowCredentials();
                                  });
            });

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpClient();
            builder.Services.AddHttpContextAccessor();


            // cấu hình jwwt
            var jwtSettings = builder.Configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? throw new InvalidOperationException("Jwt key missing;"));
            if (key.Length < 32)
            {
                throw new InvalidOperationException("Jwt key must be at least 32 characters long;");
            }

            // Thêm xác thực JWT
            builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     ValidIssuer = jwtSettings["Issuer"],
                     ValidAudience = jwtSettings["Audience"],
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? throw new InvalidOperationException("Jwt key missing;"))),
                 };
                 options.Events = new JwtBearerEvents
                 {
                     OnAuthenticationFailed = context =>
                     {
                         Console.WriteLine("Authentication failed: " + context.Exception.Message);
                         return Task.CompletedTask;
                     },
                     OnMessageReceived = context =>
                     {
                         var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                         Console.WriteLine($"Token received by middleware: {token}");
                         return Task.CompletedTask;
                     }
                 };

             });
            // Thêm phân quyền dựa trên role
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
                options.AddPolicy("RequireSalerRole", policy => policy.RequireRole("Saler"));
                options.AddPolicy("RequireUserRole", policy => policy.RequireRole("Customer"));
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // start configuration
            WebMvc.Models.AdminApiString.Initialize(builder.Configuration);
            WebMvc.Models.CustomerApiString.Initialize(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {

                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);
            app.UseMiddleware<JwtMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllerRoute(
               name: "Admin",
               pattern: "{area}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "productCategory",
                pattern: "productdetail/{categoryName}",
                defaults: new { controller = "ProductDetail", action = "ListProductDetailWithCategory" });

            app.MapControllerRoute(
                name: "productdetail",
                pattern: "productdetail/info/{id}",
                defaults: new { controller = "ProductDetail", action = "Info" });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
