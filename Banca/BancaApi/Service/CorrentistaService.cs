using BancaApi.Data;
using BancaApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using BancaApi.Service.IService;
using Microsoft.AspNetCore.Mvc;
using BancaApi.Repository.IRepository;

namespace BancaApi.Service
{

    public class CorrentistaService : ICorrentistaService
    {
        private readonly ICorrentistaRepository _corRepository;
        public CorrentistaService(ICorrentistaRepository corRepository)
        {
            _corRepository = corRepository;
        }



        public async Task<Correntista?> GetCorrentista(string? userName, string? email,string? password) => await _corRepository.GetCorrentista(userName, email, password);

        public async Task<Movimenti> CreateMovimenti(Movimenti mov) => await _corRepository.CreateMovimenti(mov);

        public async Task<Movimenti?> GetMovimenti(string Nconto) => await _corRepository.GetMovimenti(Nconto);

        public async Task<Correntista?> Correntista(string Nconto) => await _corRepository.GetCorrentista(Nconto);

        public async Task<Results<Ok, NotFound>> UpdateCorrentista(Correntista corr) => await _corRepository.UpdateCorrentista(corr);

        public async Task<List<Movimenti>> GetMovimenti(string Nconto, DateTime? start, DateTime? end, string? tipo) => await _corRepository.GetMovimenti(Nconto, start, end, tipo);

        public Task<Correntista?> GetCorrentista(string Nconto)
        {
            throw new NotImplementedException();
        }

         

        

         
    }
}
