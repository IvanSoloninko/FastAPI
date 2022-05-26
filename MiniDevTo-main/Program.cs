global using FastEndpoints;
global using FastEndpoints.Security;
global using FluentValidation;
global using MiniDevTo.Auth;
global using MongoDB.Driver;
global using MongoDB.Entities;
using System.Text.Json;
using FastEndpoints.Swagger;
using MiniDevTo.Migrations;

var builder = WebApplication.CreateBuilder();
builder.Services.AddFastEndpoints();
builder.Services.AddAuthenticationJWTBearer(builder.Configuration["JwtSigningKey"]);
builder.Services.AddSwaggerDoc();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.UseFastEndpoints(s => s.SerializerOptions = o => o.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);
app.UseOpenApi();
app.UseSwaggerUi3(c => c.ConfigureDefaults());


await DB.InitAsync("MiniDevTo",
    MongoClientSettings.FromConnectionString(
        "mongodb+srv://Ivan:Iwanko007@cluster0.gvdg6.mongodb.net/?retryWrites=true&w=majority"));
_001_seed_initial_admin_account.SuperAdminPassword = app.Configuration["SuperAdminPassword"];
await DB.MigrateAsync();

app.Run();