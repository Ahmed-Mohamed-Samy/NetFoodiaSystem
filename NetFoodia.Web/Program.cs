using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NetFoodia.Domain.Contracts;
using NetFoodia.Domain.Entities.IdentityModule;
using NetFoodia.Persistence.Data.DbContexts;
using NetFoodia.Persistence.IdentityData.DataSeed;
using NetFoodia.Persistence.Repositories;
using NetFoodia.Services;
using NetFoodia.Services.MappingProfiles;
using NetFoodia.Services.Security;
using NetFoodia.Services.Validators.AuthenticationValidators;
using NetFoodia.Services_Abstraction;
using NetFoodia.Web.CustomMiddlewares;
using NetFoodia.Web.Extensions;
using NetFoodia.Web.Factories;
using System.Text;
using System.Text.Json.Serialization;


namespace NetFoodia.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            #region Services

            builder.Services.AddControllers()
            // To Convert Enum Value To String
            .AddJsonOptions(o =>
                                        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())); ;
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            //builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter JWT Token like this: Bearer {your token}"
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
            });

            builder.Services.AddDbContext<NetFoodiaDbContext>(Options =>
            {
                var isHosted = builder.Configuration.GetValue<bool>("Deploy");
                if (isHosted)
                    Options.UseSqlServer(
    builder.Configuration.GetConnectionString("HostConnection")
                    );
                else
                {
                    Options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    );
                }

            });
            builder.Services.AddCors(options =>
                {
                    options.AddPolicy(
                        "DevelopmentPolicy",
                        builder =>
                        {
                            builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
                        }
                    );
                });

            builder.Services.AddIdentityCore<ApplicationUser>()
                                .AddRoles<IdentityRole>()
                                .AddEntityFrameworkStores<NetFoodiaDbContext>();

            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

            var jwt = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()!;

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwt.Issuer,
                    ValidAudience = jwt.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SecretKey))
                };
                //options.Events = new JwtBearerEvents
                //{
                //    OnChallenge = context =>
                //    {
                //        context.HandleResponse();
                //        context.Response.StatusCode = 401;
                //        context.Response.ContentType = "application/json";
                //        return context.Response.WriteAsync(
                //            """{"success":false,"statusCode":401,"message":"You must be logged in"}"""
                //        );
                //    }
                //};
            });



            builder.Services.Configure<ApiBehaviorOptions>(options =>
                {
                    options.InvalidModelStateResponseFactory =
                        ApiResponseFactory.GenerateApiValidationResponse;
                });


            #endregion

            #region BusinessServices
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<ICharityService, CharityService>();
            builder.Services.AddScoped<IAdminCharityService, AdminCharityService>();
            builder.Services.AddAutoMapper(typeof(CharityMappingProfile).Assembly);

            //builder.Services.AddKeyedScoped<IDataInatializer, DataInatializer>("Default");
            builder.Services.AddKeyedScoped<IDataInatializer, IdentityDataInatializer>("Identity");
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<RegisterValidator>();

            #endregion

            var app = builder.Build();

            #region Data Seeding
            //await app.MigrateDatabaseAsync();
            await app.MigrateIdentityDatabaseAsync();
            //await app.SeedDatabaseAsync();
            await app.SeedIdentityDatabaseAsync();
            #endregion

            #region Configure the HTTP request pipeline.
            app.UseMiddleware<ExceptionHandlerMiddleWare>();
            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.DisplayRequestDuration();
                options.EnableFilter();
            });
            //}

            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseCors("DevelopmentPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapGet("/", () => Results.Redirect("/swagger/index.html"));
            app.MapControllers();

            try
            {
                await app.RunAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("🔥 HOST CRASH");
                Console.WriteLine(ex);
                throw;
            }
            #endregion
        }
    }
}
