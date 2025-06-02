using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using sample_api.Services;
using Supabase;
using sample_api.Data;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Get config values
var config = builder.Configuration;
var supabaseUrl = config["Supabase:Url"];
var supabaseKey = config["Supabase:Key"];
var mongoConnectionString = config["MongoDB:ConnectionString"];

// Setup Supabase
var options = new Supabase.SupabaseOptions
{
    AutoConnectRealtime = true
};
var supabase = new Supabase.Client(supabaseUrl, supabaseKey, options);
await supabase.InitializeAsync();
builder.Services.AddSingleton(supabase);

// Setup MongoDB
builder.Services.AddSingleton<IMongoClient>(_ =>
    new MongoClient(mongoConnectionString));

// Add services to the container
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<MenuService>();
builder.Services.AddScoped<FavoritesService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
        policy.WithOrigins(
            "http://localhost:5222",
            "https://dotnet-project-learn.onrender.com"
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Pipeline config
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
