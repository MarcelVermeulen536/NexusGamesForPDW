using Core.Models;

namespace Core.IGateways;

// Contrat d'accès aux données « jeux » (catalogue).
public interface IGameGateway
{
    // Tous les jeux du catalogue (avec développeur, plateforme, genres et promo éventuelle).
    IEnumerable<Game> GetAll();

    // Un jeu par son identifiant, ou null s'il n'existe pas.
    Game? GetById(int id);
}
