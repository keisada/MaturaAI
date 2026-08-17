using MaturaAi.Data;
using MaturaAi.Models;
using MaturaAi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// BAZA DANYCH
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        connectionString,
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure();
        }));

// CONTROLLERY
builder.Services.AddControllers();

// SERVICES
builder.Services.AddScoped<ExamService>();
builder.Services.AddScoped<ExamAttemptService>();
builder.Services.AddHttpClient<IAiService, GeminiAiService>();

// OPEN API
builder.Services.AddOpenApi();

// ASP.NET IDENTITY
builder.Services
    .AddIdentityApiEndpoints<UserApplication>()
    .AddEntityFrameworkStores<AppDbContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// ENDPOINTY IDENTITY
app.MapIdentityApi<UserApplication>();

// CONTROLLERY
app.MapControllers();

// Prosty health-check backendu
app.MapGet("/", () =>
{
    return Results.Ok(new
    {
        application = "MaturaAI API",
        status = "running"
    });
});

app.Run();