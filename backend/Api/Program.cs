using Api.EndPoints;
using Api.Middleware;
using Core;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Chaîne de connexion MySQL lue depuis appsettings.json (cours : configuration)
string connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("La chaîne de connexion 'Default' est manquante dans appsettings.json.");

// Injection de dépendances : on enregistre les services des couches Core et Infrastructure
// (cours : injection de dépendances / IoC). C'est ici que les abstractions sont reliées aux implémentations.
builder.Services.AddCore();
builder.Services.AddInfrastructure(connectionString);

// CORS : autorise l'application Angular (http://localhost:4200) à appeler l'API
const string CorsPolicy = "AngularApp";
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy, policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

// Middleware global d'exceptions placé en tête du pipeline pour tout intercepter
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseCors(CorsPolicy);

// Branchement des routes (Minimal API) — une méthode d'extension par regroupement
app.MapUserRoutes();
app.MapGameRoutes();
app.MapCartRoutes();

app.Run();
