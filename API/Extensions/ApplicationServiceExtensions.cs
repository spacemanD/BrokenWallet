using System;
using Application.Coins;
using Application.Core;
using Application.Interfaces;
using Domain;
using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Photos;
using Infrastructure.Security;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using Persistence;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPIv5", Version = "v1" });
            });
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            services.AddDbContext<DataContext>(options =>
            {
                var environmentVariable = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                string connectionString;

                if (environmentVariable == "Development")
                {
                    connectionString = configuration.GetConnectionString("DefaultConnection");
                }
                else
                {
                    var connection = Environment.GetEnvironmentVariable("DATABASE_URL");

                    connection = connection!.Replace("postgres://", string.Empty);
                    var userPassword = connection.Split("@")[0];
                    var dbHost = connection.Split("@")[1];
                    var dbPort = dbHost.Split("/")[0];
                    var database = dbHost.Split("/")[1];
                    var user = userPassword.Split(":")[0];
                    var password = userPassword.Split(":")[1];
                    var host = dbPort.Split(":")[0];
                    var port = dbPort.Split(":")[1];

                    connectionString =
                        $"Server={host};Port={port};User Id={user};Password={password};Database={database}; SSL Mode=Require; Trust Server Certificate=true";
                }

                options.UseNpgsql(connectionString);
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithOrigins("http://localhost:3000");
                });
            });

            services.AddMediatR(typeof(List.Handler).Assembly);
            services.AddAutoMapper(typeof(MappingProfiles).Assembly);
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUserAccessor, UserAccessor>();
            services.AddScoped<IPhotoAccessor, PhotoAccessor>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddIdentityCore<AppUser>()
                .AddEntityFrameworkStores<DataContext>()
                .AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider)
                .AddDefaultTokenProviders();
            services.Configure<CloudinarySettings>(configuration.GetSection("Cloudinary"));
            services.Configure<SmtpSettings>(configuration.GetSection("Smtp"));
            services.AddSignalR(e => 
                e.MaximumReceiveMessageSize = 102400000
            );

            return services;
        }
    }
}