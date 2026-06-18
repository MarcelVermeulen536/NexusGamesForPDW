using Dapper;
using Infrastructure.Models;
using Infrastructure.Repositories.Abstractions;
using MySql.Data.MySqlClient;

namespace Infrastructure.Repositories;

// Accès aux données « jeux » via Dapper, avec jointures développeur/plateforme/genres + promotion active.
public class GameRepository : IGameRepository
{
    private readonly string _connectionString;

    public GameRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    // Requête commune : un jeu + ses jointures. Une ligne par genre (à recombiner via multi-mapping).
    // La sous-requête sur promotions ne renvoie le prix réduit que si une promo est ACTIVE (NOW() entre début et fin).
    private const string BaseSelect = @"
        SELECT j.id_jeu AS Id, j.titre AS Titre, j.description AS Description, j.prix AS Prix,
               j.date_sortie AS DateSortie, j.score_metascore AS ScoreMetascore,
               d.nom_developpeur AS Developpeur, p.nom_plateforme AS Plateforme,
               (SELECT pr.prix_reduit FROM promotions pr
                  WHERE pr.id_jeu = j.id_jeu AND NOW() BETWEEN pr.date_debut AND pr.date_fin
                  ORDER BY pr.prix_reduit ASC LIMIT 1) AS PrixReduit,
               g.nom_genre AS GenreName
        FROM jeux j
        INNER JOIN developpeurs d ON d.id_developpeur = j.id_developpeur
        INNER JOIN plateformes p ON p.id_plateforme = j.id_plateforme
        LEFT JOIN jeux_genres jg ON jg.id_jeu = j.id_jeu
        LEFT JOIN genres g ON g.id_genre = jg.id_genre";

    public IEnumerable<Game> GetAll()
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        const string sql = BaseSelect + " ORDER BY j.id_jeu";
        return QueryGames(connection, sql, null);
    }

    public Game? GetById(int id)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        const string sql = BaseSelect + " WHERE j.id_jeu = @Id";
        return QueryGames(connection, sql, new { Id = id }).FirstOrDefault();
    }

    // Multi-mapping de Dapper (cours Dapper : jointures) : on assemble chaque jeu avec sa liste de genres.
    // Un dictionnaire évite les doublons quand un jeu a plusieurs genres (plusieurs lignes SQL).
    private static IEnumerable<Game> QueryGames(MySqlConnection connection, string sql, object? parameters)
    {
        var gamesById = new Dictionary<int, Game>();

        connection.Query<Game, string, Game>(
            sql,
            (game, genreName) =>
            {
                if (!gamesById.TryGetValue(game.Id, out var existing))
                {
                    existing = game;
                    gamesById.Add(game.Id, existing);
                }
                // Le genre peut être null (LEFT JOIN) si le jeu n'a aucun genre
                if (!string.IsNullOrEmpty(genreName))
                {
                    existing.Genres.Add(genreName);
                }
                return existing;
            },
            parameters,
            // Indique à Dapper où commencer le 2e type mappé (le nom du genre)
            splitOn: "GenreName"
        );

        return gamesById.Values;
    }
}
