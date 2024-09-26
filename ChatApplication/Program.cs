using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:8080"); // Custom port 8080
// Add services to the container.
builder.Services.AddRouting();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) { app.UseDeveloperExceptionPage(); }

app.UseDefaultFiles();

app.UseStaticFiles();

app.UseRouting();

//app.UseAuthorization();

//app.MapRazorPages();
app.MapFallbackToFile("login.html");
app.Run();

