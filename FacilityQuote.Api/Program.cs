using System.Text.Json.Serialization;
using FacilityQuote.Application.Availability;
using FacilityQuote.Application.Requests;
using FacilityQuote.Application.Services;
using FacilityQuote.Infrastructure.Persistence;
using FacilityQuote.Infrastructure.Repositories;
using FacilityQuote.Infrastructure.Seeding;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using System.Security.Claims;
using System.Text.Json;
using Microsoft.OpenApi;
using FacilityQuote.Application.Customers;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<RequestService>();
builder.Services.AddScoped<ServicesService>();
builder.Services.AddScoped<AvailabilityService>();
builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IAvailabilityRepository, AvailabilityRepository>();

builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.AddDbContext<FacilityQuoteDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("FacilityQuote"))
    .UseSeeding((context, _) =>
    {
        FacilityQuoteDataSeeder.Seed(
            (FacilityQuoteDbContext)context);
    }));

    builder.Services.AddSwaggerGen(c =>
    {
        // 1. Definition für das JWT Bearer Token festlegen
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header
        });

        c.AddSecurityRequirement(document =>
            {
                return new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
                };
            });
    });

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority =
            "http://localhost:8080/realms/facilityquote";

        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateAudience = false,

                RoleClaimType = ClaimTypes.Role
            };

        options.Events =
            new JwtBearerEvents
            {
                OnTokenValidated = context =>
                {
                    var realmAccess =
                        context.Principal?.FindFirst("realm_access");

                    if (realmAccess is not null)
                    {
                        using var document =
                            JsonDocument.Parse(
                                realmAccess.Value);

                        if (document.RootElement.TryGetProperty(
                            "roles",
                            out var roles))
                        {
                            var identity =
                                context.Principal?.Identity
                                as ClaimsIdentity;

                            if (identity is not null)
                            {
                                foreach (var role in roles.EnumerateArray())
                                {
                                    identity.AddClaim(
                                        new Claim(
                                            ClaimTypes.Role,
                                            role.GetString()!));
                                }
                            }
                        }
                    }

                    return Task.CompletedTask;
                }
            };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();