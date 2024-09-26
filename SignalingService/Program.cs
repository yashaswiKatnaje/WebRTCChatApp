using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();
builder.Services.AddDbContext<SignalRDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // Update this line

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", builder =>
    {
        builder.WithOrigins("https://localhost:44326") // Allow your SignalR client origin
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials(); // Allow credentials for SignalR
    });
});

builder.Services.AddSignalR();
builder.Services.AddControllers();

var app = builder.Build();

// Enable CORS
app.UseCors("AllowSpecificOrigins");

app.MapHub<ChatHub>("/SignalRHub");
app.MapControllers();

app.Run();
