
using BancaModels.Models;
using Microsoft.AspNetCore.Http.HttpResults;
 

namespace BancaService.Service.IService
{
    public interface ICorrentistaService
    {
        Task<Correntista?> GetCorrentista(string Nconto);
        Task<Correntista?> GetCorrentista(string? userName, string? email, string? password);
        Task<Movimenti> CreateMovimenti(Movimenti mov);
        Task<Movimenti?> GetMovimenti(string Nconto);
        Task<List<Movimenti>> GetMovimenti(string Nconto, DateTime? start, DateTime? end, string? tipo);
        Task<Results<Ok, NotFound>> UpdateCorrentista(Correntista corr);
    }
}