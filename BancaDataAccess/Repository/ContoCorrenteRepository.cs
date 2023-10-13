using BancaModels.Models;
using Microsoft.EntityFrameworkCore;
using BancaDataAccess.Data;
using BancaDataAccess.Repository.IRepository;

namespace BancaDataAccess.Repository
{
    public class ContoCorrenteRepository : IContoCorrenteRepository
    {
        private readonly AppDbContext _db;


        public ContoCorrenteRepository(AppDbContext db)
        {
            _db = db;

        }

        public async Task<ContoCorrente?> GetContoCorrente(string _NConto) => await _db.Set<ContoCorrente>().AsNoTracking().Where(m => m.Nconto == _NConto).FirstOrDefaultAsync();

        public async Task<ContoCorrente> CreateConto(ContoCorrente conto)
        {
            _db.Add(conto);
            await _db.SaveChangesAsync();
            return conto;
        }
        public async Task<ContoCorrente> UpdateConto(ContoCorrente conto)
        {
            var model = _db.Entry(conto);
            model.State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return conto;
        }

        public async Task DeleteConto(ContoCorrente conto)
        {
            _db.Remove(conto);
            await _db.SaveChangesAsync();


        }
        public async Task<ContoCorrente?> GetContoCorrente(bool _busy)
        {

            ContoCorrente? conto = await _db._ContoCorrente.FirstOrDefaultAsync(c => c.Busy == _busy);
            if (conto!.Busy == false) { return null; }
            return conto;

        }
        public async Task<List<ContoCorrente>> GetContoCorrente(string? Nconto, string? Iban, string? Ncarta)
        {
            if (Nconto == null && Iban == null && Ncarta == null)
            {
                return await _db._ContoCorrente.ToListAsync();
            }
            else
            {
                return await _db._ContoCorrente.Where(c => c.Nconto == Nconto || c.Iban == Iban || c.Ncarta == Ncarta).ToListAsync();
            }
        }


        //public async Task<ContoCorrente?> AllParameterAsync( string Nconto,  string? Iban,  string? Ncarta)
        //{

        //   var res = await _db._ContoCorrente.FirstOrDefaultAsync(c =>  c.Nconto == Nconto || c.Iban == Iban || c.Ncarta == Ncarta );

        //  var query = _db._ContoCorrente
        //  .Where(x => x.Nconto != Nconto)
        //  .Where(st => st.Iban == (Iban ?? st.Iban))
        //  .Where(st => st.Ncarta == (Ncarta ?? st.Ncarta) )
        //  .ToList();

        //    return res;
        //}


    }
}


