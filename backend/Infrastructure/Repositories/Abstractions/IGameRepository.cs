using Infrastructure.Models;

namespace Infrastructure.Repositories.Abstractions;

// Contrat d'accès aux données « jeux ».
public interface IGameRepository
{
    IEnumerable<Game> GetAll();
    Game? GetById(int id);
}
