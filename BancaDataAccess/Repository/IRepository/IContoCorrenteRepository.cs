using BancaModels.Models;

namespace BancaDataAccess.Repository.IRepository
{
    public interface IContoCorrenteRepository
    {
        Task<ContoCorrente> CreateConto(ContoCorrente conto);
        Task DeleteConto(ContoCorrente conto);
        Task<ContoCorrente?> GetContoCorrente(bool _busy);
        Task<ContoCorrente?> GetContoCorrente(string _NConto);
        Task<List<ContoCorrente>> GetContoCorrente(string? Nconto, string? Iban, string? Ncarta);
        Task<ContoCorrente> UpdateConto(ContoCorrente conto);
    }
}