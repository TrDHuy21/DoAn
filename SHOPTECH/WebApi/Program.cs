
using System.Text;
using Application.Service.Implementation;
using Application.Service.Interface;
using Infrastructure.Context;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApi.Middleware;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpContextAccessor();

            // cors config
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {

                                      policy.WithOrigins("https://localhost:5048", "http://localhost:5048")
                                          .AllowAnyMethod()
                                          .AllowAnyHeader()
                                          .AllowCredentials();
                                  });
            });

            // connect to database
            builder.Services.AddDbContext<ShopTechContext>(ops =>
                ops.UseSqlServer(builder.Configuration.GetConnectionString("ShopTechConnection")));

            // Add services to the container.
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            builder.Services.AddControllers();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddTransient<IAddressService, AddressService>();
            builder.Services.AddTransient<IAuthService, AuthService>();
            builder.Services.AddTransient(typeof(IBaseEntityService<>), typeof(BaseEntityService<>));
            builder.Services.AddTransient<IBrandService, BrandService>();
            builder.Services.AddTransient<IImageFileService, ImageFileService>();
            builder.Services.AddTransient<ICategoryService, CategoryService>();
            builder.Services.AddTransient<IProductAttributeService, ProductAttributeService>();
            builder.Services.AddTransient<IProductService, ProductService>();
            builder.Services.AddTransient<IProductDetailService, ProductDetailService>();
            builder.Services.AddTransient<IProductDetailAttributeService, ProductDetailAttributeService>();
            builder.Services.AddTransient<ICartService, CartService>();
            builder.Services.AddTransient<IOrderService, OrderService>();
            builder.Services.AddTransient<IUserService, UserService>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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
                options.AddPolicy("RequireCustomerRole", policy => policy.RequireRole("Customer"));
            });

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "My API",
                    Version = "v1",
                    Description = "API có Authentication v?i JWT"
                });

                // add Security Definition cho Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Input JWT token  format: Bearer {token}"
                });

                // add Security Requirement
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(MyAllowSpecificOrigins);
            app.UseMiddleware<JwtMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
