using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Utils;

// Utilitaire de hachage des mots de passe.
// (cours C# : classe utilitaire avec membres statiques ; cours POO : encapsulation d'un détail technique)
// Schéma : Base64( SHA-256(mot_de_passe) ). Le même hachage est utilisé dans db.sql pour les comptes de test.
public static class Base64Utils
{
    // Transforme un mot de passe en clair en sa version hachée stockée en base.
    public static string Hash(string motDePasse)
    {
        byte[] octets = Encoding.UTF8.GetBytes(motDePasse);
        byte[] empreinte = SHA256.HashData(octets);
        return Convert.ToBase64String(empreinte);
    }

    // Vérifie qu'un mot de passe en clair correspond au hachage stocké.
    public static bool Verify(string motDePasse, string hashStocke)
    {
        return Hash(motDePasse) == hashStocke;
    }
}
