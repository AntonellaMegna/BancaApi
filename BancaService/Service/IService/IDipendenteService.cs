using BancaModels.Models;
using System.Runtime.InteropServices;

 namespace Banca.Service.Service.IService
{
    public interface IDipendenteService
    {
        Task<List<Correntista>> GetCorrentista([Optional] string? Nconto, [Optional] string? CF, [Optional] string? Email, [Optional] string? Nome, [Optional] string? Cognome, [Optional] string? Indirizzo, [Optional] DateTime? DataConto);
        Task<Correntista> CreateCorrentista(Correntista conto);
        Task DeleteCorrentista(Correntista Nconto);
        Task DeleteMovimenti(Movimenti Nconto);
        Task<DipendenteBanca?> GetDipendente(string userName);
        Task<Correntista> UpdateCorrentista(Correntista conto);
        Task<Correntista?> GetCorrentista(string conto);
    }
}