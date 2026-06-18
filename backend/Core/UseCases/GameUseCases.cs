using Core.IGateways;
using Core.Models;
using Core.UseCases.Abstractions;

namespace Core.UseCases;

// Logique métier liée au catalogue de jeux.
public class GameUseCases : IGameUseCases
{
    private readonly IGameGateway _gameGateway;

    public GameUseCases(IGameGateway gameGateway)
    {
        _gameGateway = gameGateway;
    }

    public IEnumerable<Game> GetAll()
    {
        return _gameGateway.GetAll();
    }

    // Récupère un jeu ou lève une exception si introuvable.
    // (cours C# : exceptions ; elle sera traduite en 404 par le middleware d'exception)
    public Game GetById(int id)
    {
        Game? game = _gameGateway.GetById(id);
        if (game is null)
        {
            throw new KeyNotFoundException($"Le jeu #{id} est introuvable.");
        }
        return game;
    }
}
