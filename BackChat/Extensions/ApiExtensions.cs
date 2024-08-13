﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OnionTest.Infastucture;
using System.Text;

namespace BackChat.Extensions
{
    public static class ApiExtensions
    {
        public static void AddApiAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
            



            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    var secretKeyString = configuration.GetValue<string>("SECRET_KEY");

                    if (secretKeyString == null)
                    {
                        throw new InvalidOperationException("Secret key cannot be null.");
                    }

                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = configuration.GetValue<string>("JwtOptions:ValidIssuer"),
                        ValidateAudience = true,
                        ValidAudience = configuration.GetValue<string>("JwtOptions:ValidAudience"),
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKeyString))
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["tasty-cookie"];

                            Console.WriteLine($"Received token: {context.Token}");

                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            Console.WriteLine("Token validated successfully");
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy =>
                {
                    policy.RequireClaim("Role", "1");
                });
                options.AddPolicy("user", policy =>
                {
                    policy.RequireClaim("Role", "2");
                });

            });
        }
    }
}
