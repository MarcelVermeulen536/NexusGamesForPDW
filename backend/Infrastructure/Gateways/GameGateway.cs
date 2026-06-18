using Core.IGateways;
using Infrastructure.Repositories.Abstractions;
using CoreModels = Core.Models;
using DbModels = Infrastructure.Models;

namespace Infrastructure.Gateways;

// Implémente Core.IGateways.IGameGateway : mappe les modèles « base de données » vers le domaine.
public class GameGateway : IGameGateway
{
    private readonly IGameRepository _gameRepository;

    public GameGateway(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

    public IEnumerable<CoreModels.Game> GetAll()
    {
        // LINQ Select pour projeter chaque modèle DB vers le modèle de domaine (cours C# : LINQ)
        return _gameRepository.GetAll().Select(ToDomain);
    }

    public CoreModels.Game? GetById(int id)
    {
        DbModels.Game? dbGame = _gameRepository.GetById(id);
        return dbGame is null ? null : ToDomain(dbGame);
    }

    private static CoreModels.Game ToDomain(DbModels.Game g) => new()
    {
        Id = g.Id,
        Titre = g.Titre,
        Description = g.Description,
        Prix = g.Prix,
        DateSortie = g.DateSortie,
        ScoreMetascore = g.ScoreMetascore,
        Developpeur = g.Developpeur,
        Plateforme = g.Plateforme,
        PrixReduit = g.PrixReduit,
        Genres = g.Genres,
    };
}
