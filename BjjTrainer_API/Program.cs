using BjjTrainer_API.Data;
using BjjTrainer_API.Models.Users;
using BjjTrainer_API.Services_API.Calendars;
using BjjTrainer_API.Services_API.Coaches;
using BjjTrainer_API.Services_API.Goals;
using BjjTrainer_API.Services_API.Moves;
using BjjTrainer_API.Services_API.Schools;
using BjjTrainer_API.Services_API.Trainings;
using BjjTrainer_API.Services_API.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configure the DbContext to use PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

var stripeSettings = builder.Configuration.GetSection("Stripe");
Stripe.StripeConfiguration.ApiKey = stripeSettings["SecretKey"];

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddScoped<MoveService>();
builder.Services.AddScoped<TrainingService>();
builder.Services.AddScoped<TrainingGoalService>();
builder.Services.AddScoped<CalendarService>();
builder.Services.AddScoped<SchoolService>();
builder.Services.AddScoped<UserProgressService>();
builder.Services.AddScoped<CoachService>();


// In Program.cs or wherever you configure JSON serialization
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();

// Configure JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        // Fix for CS8604: Ensure the configuration value is not null before using it.
        var secretKey = builder.Configuration["Jwt:SecretKey"];
        if (string.IsNullOrEmpty(secretKey))
        {
            throw new InvalidOperationException("JWT SecretKey is not configured.");
        }

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ClockSkew = TimeSpan.FromMinutes(5),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNextJs",
        policy => policy
            .WithOrigins("http://localhost:3000", "http://localhost:3001")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Enable CORS for Next.js
app.UseCors("AllowNextJs");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();