using Core.Models;

namespace Core.UseCases.Abstractions;

// Cas d'utilisation « jeux » (catalogue).
public interface IGameUseCases
{
    IEnumerable<Game> GetAll();
    Game GetById(int id);
}
