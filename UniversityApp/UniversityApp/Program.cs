// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using System.Text;
using BL.AutoMapper;
using BL.Configuration;
using BL.Managers;
using BL.Services;
using DAL;
using DAO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using UniversityApp.Helpers;


var logger = LogManager.Setup()
    .LoadConfigurationFromAppSettings()
    .GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddHttpClient();
    builder.Services.AddDbContext<UniversityContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

    builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

    builder.Services.AddMemoryCache();

    builder.Services.Configure<JWTConfiguration>(builder.Configuration.GetSection("JWTConfiguration"));

    builder.Services.AddScoped<IAuthManager, AuthManager>();
    builder.Services.AddScoped<IDepartmentManager, DepartmentManager>();
    builder.Services.AddScoped<ICourseManager, CourseManager>();
    builder.Services.AddScoped<IClassManager, ClassManager>();
    builder.Services.AddScoped<IAttendanceManager, AttendanceManager>();
    builder.Services.AddScoped<IAssignmentManager, AssignmentManager>();
    builder.Services.AddScoped<ISubmissionManager, SubmissionManager>();
    builder.Services.AddScoped<INotificationManager, NotificationManager>();
    builder.Services.AddScoped<IEmailService, EmailService>();
    builder.Services.AddScoped<IFileStorageService, FileStorageService>();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll",
            builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
    });



    string secretKey = builder.Configuration["JWTConfiguration:SecretKey"];
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        if (secretKey != null)
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["JWTConfiguration:ValidIssuer"],
                ValidateAudience = true,
                ValidAudience = builder.Configuration["JWTConfiguration:ValidAudience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        }
        else
        {
            throw new InvalidOperationException("JWT Secret Key is not configured");
        }
    });

    builder.Services.AddAuthorization();

    builder.Services.AddSwaggerGen(swagger =>
    {
        swagger.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "University Management System API",
            Description = "ASP.NET Core Web API for School Management System"
        });

        swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
        });

        swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
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


    app.UseCors("AllowAll");
    app.UseErrorHandlingMiddleware();

    app.UseExceptionHandler("/Error");
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), "uploads")),
        RequestPath = "/uploads"
    });

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}

