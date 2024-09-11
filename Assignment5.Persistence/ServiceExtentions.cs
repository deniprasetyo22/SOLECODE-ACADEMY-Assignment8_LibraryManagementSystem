﻿using Assignment5.Application.Interfaces.IRepositories;
using Assignment5.Application.Interfaces.IService;
using Assignment5.Application.Services;
using Assignment5.Domain.Models;
using Assignment5.Persistence.Context;
using Assignment5.Persistence.Repositories;
using Assignment7.Application.Interfaces.IRepositories;
using Assignment7.Application.Interfaces.IService;
using Assignment7.Application.Services;
using Assignment7.Domain.Models;
using Assignment7.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment5.Persistence
{
    public static class ServiceExtentions
    {
        public static IServiceCollection ConfigurePersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<LibraryContext>(options => options.UseNpgsql(connectionString));
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBookRequestRepository, BookRequestRepository>();
            services.AddScoped<IBookRequestService, BookRequestService>();
            services.AddScoped<IBorrowRepository, BorrowRepository>();
            services.AddScoped<IBorrowService, BorrowService>();
            services.AddScoped<IDashboardService, DashboardService>();

            services.AddScoped<IAuthService, AuthService>();

            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.SignIn.RequireConfirmedEmail = true;
            }).AddEntityFrameworkStores<LibraryContext>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                options.DefaultChallengeScheme =
                options.DefaultForbidScheme =
                options.DefaultScheme =
                options.DefaultSignInScheme =
                options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = configuration["JWT:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = configuration["JWT:Audience"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SigningKey"])),

                    };
                });

            services.AddHttpContextAccessor();

            return services;
        }
    }
}
