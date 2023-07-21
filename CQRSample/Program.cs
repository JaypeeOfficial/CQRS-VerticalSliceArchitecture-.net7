using CQRSample.Data;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(Program).Assembly));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAuthentication(authOptions =>
{
    authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(jwtOptions =>
    {
        var key = builder.Configuration.GetValue<string>("JwtConfig:Key");
        var keyBytes = Encoding.ASCII.GetBytes(key);
        jwtOptions.SaveToken = true;
        jwtOptions.RequireHttpsMetadata = false;
        jwtOptions.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuer = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
            ValidAudience = builder.Configuration["JwtConfig:Audience"]
        };
    });

var connectionString = builder.Configuration.GetConnectionString("DevConnection");
builder.Services.AddDbContext<StoreContext>(x => x.UseSqlServer(connectionString));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
