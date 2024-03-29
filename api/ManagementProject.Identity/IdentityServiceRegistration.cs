﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ManagementProject.Identity.JwtModel;
using System;
using System.Text;
using System.Threading.Tasks;
using ManagementProject.Persistence.Context;
using ManagementProject.Persistence.Entity;
using Microsoft.Extensions.Options;

namespace ManagementProject.Identity
{
    public static class IdentityServiceRegistration
    {
        public static void AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
            

            services.AddIdentity<ApplicationUser, IdentityRole>( config =>
            {
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit = false;
                config.Password.RequireUppercase = false;
                config.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<ManagementProjectDbContext>().AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                    };

                    o.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = c =>
                        {
                            if (c.Request.Query.ContainsKey("access_token"))
                            {
                                c.Token = c.Request.Query["access_token"];
                            }
                            else if (c.Request.Cookies.ContainsKey("X-Access-Token"))
                            {
                                c.Token = c.Request.Cookies["X-Access-Token"];
                            }

                            return Task.CompletedTask;
                        },

                        OnAuthenticationFailed = c =>
                        {
                            c.NoResult();
                            c.Response.StatusCode = 500;
                            c.Response.ContentType = "text/plain";
                            if (c.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                c.Response.Headers.Add("Token-Expired", "true");
                                c.Response.Cookies.Delete("X-Access-Token");
                                return Task.CompletedTask;
                            }
                            return c.Response.WriteAsync(c.Exception.ToString());

                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "text/plain";
                            return context.Response.WriteAsync("401 Not authorized");
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = 403;
                            context.Response.ContentType = "text/plain";
                            return context.Response.WriteAsync("403 Not authorized");
                        },
                    };
                });
        }
    }
}
