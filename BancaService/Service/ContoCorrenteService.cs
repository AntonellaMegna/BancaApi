using BancaService.Service.IService;
using BancaDataAccess.Repository.IRepository;
using BancaModels.Models;

namespace BancaService.Service
{
    public class ContoCorrenteService: IContoCorrenteService
    {
        protected readonly IContoCorrenteRepository _contoRepository;
        public ContoCorrenteService(IContoCorrenteRepository contoRepository)
        {
            _contoRepository = contoRepository;

        }

        public async Task<ContoCorrente> CreateConto(ContoCorrente conto) => await _contoRepository.CreateConto(conto);

        public async Task DeleteConto(ContoCorrente conto) => await _contoRepository.DeleteConto(conto);


        public async Task<ContoCorrente?> GetContoCorrente(bool _busy) => await _contoRepository.GetContoCorrente(_busy);


        public async Task<ContoCorrente?> GetContoCorrente(string _NConto) => await _contoRepository.GetContoCorrente(_NConto);


        public async Task<List<ContoCorrente>> GetContoCorrente(string? Nconto, string? Iban, string? Ncarta) => await _contoRepository.GetContoCorrente(Nconto, Iban, Ncarta);

        public async Task<ContoCorrente> UpdateConto(ContoCorrente conto) => await _contoRepository.UpdateConto(conto);

    
    }
    
}
