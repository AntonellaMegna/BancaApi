using BancaApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BancaApi.Repository.IRepository
{
    public interface ICorrentistaRepository
    {
        Task<Movimenti> CreateMovimenti(Movimenti mov);
        Task<Correntista?> GetCorrentista(string Nconto);
        Task<Correntista?> GetCorrentista(string? userName, string? email, string? password);
        Task<Movimenti?> GetMovimenti(string Nconto);
        Task<List<Movimenti>> GetMovimenti(string Nconto, DateTime? start, DateTime? end, string? tipo);
        Task<Results<Ok, NotFound>> UpdateCorrentista(Correntista corr);
    }
}