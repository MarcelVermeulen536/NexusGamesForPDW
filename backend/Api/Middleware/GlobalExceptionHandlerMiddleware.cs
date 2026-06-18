namespace Api.Middleware;

// Middleware global : intercepte les exceptions remontées par les couches inférieures
// et les traduit en réponses HTTP cohérentes (cours C# : gestion des exceptions ; ASP.NET : middleware).
public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // On laisse la requête continuer dans le pipeline...
            await _next(context);
        }
        catch (Exception ex)
        {
            // ... et on attrape toute exception non gérée pour renvoyer une réponse propre.
            _logger.LogError(ex, "Exception non gérée");

            // Le type d'exception détermine le code HTTP (cours : on lève des exceptions adaptées)
            int statusCode = ex switch
            {
                KeyNotFoundException => StatusCodes.Status404NotFound,        // jeu introuvable
                ArgumentException => StatusCodes.Status400BadRequest,         // données invalides
                InvalidOperationException => StatusCodes.Status409Conflict,   // ex : compte déjà existant
                _ => StatusCodes.Status500InternalServerError,
            };

            // On n'expose le détail interne que pour les erreurs « métier » (pas pour une 500)
            string message = statusCode == StatusCodes.Status500InternalServerError
                ? "Une erreur interne est survenue."
                : ex.Message;

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new { error = message });
        }
    }
}
