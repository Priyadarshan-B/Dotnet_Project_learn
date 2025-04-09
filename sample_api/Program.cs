using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using sample_api.Services;
using Supabase;
using sample_api.Data;
using MongoDB.Driver;
using Microsoft.JSInterop.Infrastructure;
var builder = WebApplication.CreateBuilder(args);

var supabaseUrl = "https://qoyvfgklgyvoqxrdhrzh.supabase.co";
var supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InFveXZmZ2tsZ3l2b3F4cmRocnpoIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDIyNzg4NDMsImV4cCI6MjA1Nzg1NDg0M30.0wIFDdDRE6duzNFbeBpzCwOvvC4JXFwVvB5S5H6UVqo";
var options = new Supabase.SupabaseOptions
{
    AutoConnectRealtime = true
};
var supabase = new Supabase.Client(supabaseUrl, supabaseKey, options);
await supabase.InitializeAsync();
builder.Services.AddSingleton(supabase);

builder.Services.AddSingleton<IMongoClient>(s =>
    new MongoClient("mongodb://localhost:27017"));


// Add services to the container.
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<MenuService>();
builder.Services.AddScoped<FavoritesService>();

//Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
        policy.WithOrigins("http://localhost:5222") 
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});

builder.Services.AddControllers();
builder.Services.AddSingleton<AuthService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();
