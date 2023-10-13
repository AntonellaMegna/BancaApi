using BancaDataAccess.Repository.IRepository;
using BancaModels.Models;
using System.Runtime.InteropServices;
using Banca.Service.Service.IService;

namespace BancaService.Service
{
    public class DipendenteService : IDipendenteService
    {
        private readonly IDipendenteRepository _dipRep;

        public DipendenteService(IDipendenteRepository dipRep)
        {
            _dipRep = dipRep;

        }

        public async Task<List<Correntista>> GetCorrentista([Optional] string? Nconto, [Optional] string? CF, [Optional] string? Email, [Optional] string? Nome, [Optional] string? Cognome, [Optional] string? Indirizzo, [Optional] DateTime? DataConto)
        => await _dipRep.GetCorrentista(Nconto, CF, Email, Nome, Cognome, Indirizzo, DataConto);
        

        public async Task<Correntista> CreateCorrentista(Correntista conto)=> await _dipRep.CreateCorrentista(conto);

        public async Task DeleteCorrentista(Correntista Nconto) => await _dipRep.DeleteCorrentista(Nconto);


        public async Task DeleteMovimenti(Movimenti Nconto) => await _dipRep.DeleteMovimenti(Nconto);


        public async Task<Correntista?> GetCorrentista(string conto)=> await  _dipRep.GetCorrentista(conto);


        public async Task<DipendenteBanca?> GetDipendente(string userName)=> await _dipRep.GetDipendente(userName);


        public async Task<Correntista> UpdateCorrentista(Correntista conto)=> await _dipRep.UpdateCorrentista(conto);    
         
    }
}
