using MaturaAi.Data;
using MaturaAi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// BAZA DANYCH
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));


// CONTROLLERY
builder.Services.AddControllers();


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


// Identity / autoryzacja
app.UseAuthentication();
app.UseAuthorization();


// GOTOWE ENDPOINTY IDENTITY
app.MapIdentityApi<UserApplication>();


// STRONA GŁÓWNA Z REKORDAMI Z BAZY
app.MapGet("/", async (AppDbContext db) =>
{
    var top3Exams = await db.Exam
        .AsNoTracking()
        .Take(3)
        .ToListAsync();

    string rowsHtml;

    if (top3Exams.Count == 0)
    {
        rowsHtml =
            "<tr><td colspan='4' style='padding: 15px; text-align: center; color: #ef4444; font-weight: bold;'>Tabela Exam w bazie jest pusta! (Brak rekordów)</td></tr>";
    }
    else
    {
        rowsHtml = string.Join("", top3Exams.Select(e => $@"
            <tr>
                <td style='padding: 10px; border-bottom: 1px solid #334155;'>{e.Id}</td>
                <td style='padding: 10px; border-bottom: 1px solid #334155; font-weight: bold;'>{e.Subject}</td>
                <td style='padding: 10px; border-bottom: 1px solid #334155;'>{e.Title}</td>
                <td style='padding: 10px; border-bottom: 1px solid #334155; color: #38bdf8;'>{e.ExamMonth} {e.ExamYear}</td>
            </tr>
        "));
    }

    return Results.Content($@"
        <!DOCTYPE html>
        <html lang='pl'>
        <head>
            <meta charset='UTF-8'>
            <title>MaturaAI Backend</title>
            <style>
                body {{ font-family: system-ui, sans-serif; background: #0f172a; color: #f8fafc; display: flex; justify-content: center; align-items: center; min-height: 100vh; margin: 0; }}
                .card {{ background: #1e293b; padding: 2rem; border-radius: 16px; box-shadow: 0 10px 25px rgba(0,0,0,0.5); text-align: center; width: 90%; max-width: 600px; border: 1px solid #334155; }}
                h1 {{ color: #38bdf8; margin-bottom: 0.5rem; font-size: 1.8rem; }}
                p {{ color: #94a3b8; }}
                table {{ width: 100%; border-collapse: collapse; margin-top: 1.5rem; text-align: left; font-size: 0.95rem; }}
                th {{ background: #0f172a; color: #94a3b8; padding: 10px; border-bottom: 2px solid #334155; }}
                .badge {{ background: #064e3b; color: #34d399; padding: 4px 12px; border-radius: 20px; font-weight: bold; display: inline-block; margin-bottom: 1rem; }}
            </style>
        </head>
        <body>
            <div class='card'>
                <h1>🚀 MaturaAI Backend</h1>
                <div class='badge'>● Azure SQL Connected</div>
                <p>3 pierwsze rekordy pobrane z tabeli <strong>Exam</strong>:</p>
                <table>
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>Przedmiot</th>
                            <th>Tytuł</th>
                            <th>Termin</th>
                        </tr>
                    </thead>
                    <tbody>
                        {rowsHtml}
                    </tbody>
                </table>
            </div>
        </body>
        </html>
    ", "text/html");
});


app.MapControllers();
app.MapGet("/ef-model", (AppDbContext db) =>
{
    return db.Model
        .GetEntityTypes()
        .Select(e => new
        {
            Entity = e.Name,
            Table = e.GetTableName()
        })
        .ToList();
});
app.Run();