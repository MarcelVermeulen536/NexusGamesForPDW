using Dapper;
using Infrastructure.Models;
using Infrastructure.Repositories.Abstractions;
using MySql.Data.MySqlClient;

namespace Infrastructure.Repositories;

// Accès aux données « utilisateur » via Dapper (cours Dapper).
// Le repository reçoit la chaîne de connexion par son constructeur (comme le DapperRepo du cours).
public class UserRepository : IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public User? GetByEmail(string email)
    {
        // `using` : la connexion est libérée automatiquement à la fin du bloc (cours Dapper / IDisposable)
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        // Alias SQL (AS) pour faire correspondre les colonnes aux propriétés C# (cours Dapper)
        const string sql = @"
            SELECT id_utilisateur AS Id, pseudo AS Pseudo, email AS Email,
                   mot_de_passe AS MotDePasse, nom AS Nom, prenom AS Prenom,
                   date_inscription AS DateInscription, solde_compte AS SoldeCompte, statut AS Statut
            FROM utilisateurs
            WHERE email = @Email";

        // Paramètre SQL (@Email) pour éviter l'injection SQL (cours Dapper)
        return connection.QuerySingleOrDefault<User>(sql, new { Email = email });
    }

    public bool ExistsByEmailOrPseudo(string email, string pseudo)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        const string sql = @"
            SELECT COUNT(*) FROM utilisateurs
            WHERE email = @Email OR pseudo = @Pseudo";

        int count = connection.ExecuteScalar<int>(sql, new { Email = email, Pseudo = pseudo });
        return count > 0;
    }

    public int Insert(User user)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        // INSERT puis récupération de l'identifiant généré (LAST_INSERT_ID() en MySQL — cours Dapper)
        const string sql = @"
            INSERT INTO utilisateurs (pseudo, email, mot_de_passe, nom, prenom)
            VALUES (@Pseudo, @Email, @MotDePasse, @Nom, @Prenom);
            SELECT LAST_INSERT_ID();";

        return connection.ExecuteScalar<int>(sql, user);
    }
}
