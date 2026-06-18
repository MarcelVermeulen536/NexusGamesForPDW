using Dapper;
using Infrastructure.Models;
using Infrastructure.Repositories.Abstractions;
using MySql.Data.MySqlClient;

namespace Infrastructure.Repositories;

// Accès aux données « achat / bibliothèque » via Dapper.
public class CartRepository : ICartRepository
{
    private readonly string _connectionString;

    public CartRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void Checkout(int userId, IEnumerable<int> gameIds, string methodePaiement)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        // Transaction : tous les achats réussissent, ou aucun (cours Dapper : transactions)
        using var transaction = connection.BeginTransaction();
        try
        {
            // Prix calculé côté serveur : prix réduit si promo active, sinon prix normal
            // (on ne fait pas confiance au prix envoyé par le client)
            const string sqlPrix = @"
                SELECT COALESCE(
                    (SELECT pr.prix_reduit FROM promotions pr
                       WHERE pr.id_jeu = @Id AND NOW() BETWEEN pr.date_debut AND pr.date_fin
                       ORDER BY pr.prix_reduit ASC LIMIT 1),
                    (SELECT prix FROM jeux WHERE id_jeu = @Id)
                )";

            const string sqlInsert = @"
                INSERT INTO achats (id_utilisateur, id_jeu, prix_paye, methode_paiement, statut_achat)
                VALUES (@UserId, @GameId, @Prix, @Methode, 'complete')";

            foreach (int gameId in gameIds)
            {
                decimal prix = connection.ExecuteScalar<decimal>(
                    sqlPrix, new { Id = gameId }, transaction);

                connection.Execute(
                    sqlInsert,
                    new { UserId = userId, GameId = gameId, Prix = prix, Methode = methodePaiement },
                    transaction);
                // Le trigger after_achat_add_to_library ajoute automatiquement le jeu à la bibliothèque.
            }

            transaction.Commit();
        }
        catch
        {
            // En cas d'erreur : annulation pour préserver l'intégrité (cours Dapper)
            transaction.Rollback();
            throw; // on propage l'exception (cours C# : throw; conserve la pile d'appels)
        }
    }

    public IEnumerable<LibraryItem> GetLibrary(int userId)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        const string sql = @"
            SELECT b.id_jeu AS GameId, j.titre AS Titre, b.date_acquisition AS DateAcquisition,
                   b.temps_jeu_heures AS TempsJeuHeures, b.completion_pourcentage AS CompletionPourcentage
            FROM bibliotheque b
            INNER JOIN jeux j ON j.id_jeu = b.id_jeu
            WHERE b.id_utilisateur = @UserId
            ORDER BY b.date_acquisition DESC, j.titre";

        return connection.Query<LibraryItem>(sql, new { UserId = userId });
    }
}
