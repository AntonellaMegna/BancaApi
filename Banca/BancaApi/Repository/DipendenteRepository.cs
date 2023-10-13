using BancaApi.Data;
using BancaApi.Models;
using BancaApi.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace BancaApi.Repository
{
    public class DipendenteRepository : IDipendenteRepository
    {
        private readonly AppDbContext _db;


        public DipendenteRepository(AppDbContext db)
        {
            _db = db;

        }

        public async Task<DipendenteBanca?> GetDipendente(string userName) => await _db.Set<DipendenteBanca>().AsNoTracking().Where(dip => dip.UserName == userName ).FirstOrDefaultAsync();


        //public async Task<DipendenteBanca> LoginDip(DipendenteBanca dipendente)
        //{
        //    var res  = await _db._DipendenteBanca.FirstOrDefaultAsync(dip => dip.Email == dipendente.Email &&
        //    dip.Pwd == dipendente.Pwd && dip.Nome == dipendente.Nome
        //    && dip.Cognome == dipendente.Cognome);
        //    return res!;
        //}
        public async Task<List<Correntista>> GetCorrentista([Optional] string? Nconto, [Optional] string? CF, [Optional] string? Email, [Optional] string? Nome, [Optional] string? Cognome, [Optional] string? Indirizzo, [Optional] DateTime? DataConto)
        {
            if (Nconto == null && CF == null && Email == null && Nome == null && Cognome == null && Indirizzo == null && DataConto == null)
            {
                return await _db._Correntista.ToListAsync();
            }
            else
            {

                return await _db._Correntista.Where(cor => cor.CF == CF || cor.Email == Email || cor.Nome == Nome || cor.Cognome == Cognome || cor.Indirizzo == Indirizzo || cor.DataConto == DataConto || cor.NcontoCorr == Nconto).ToListAsync();
            }
        }



        public async Task<Correntista> CreateCorrentista(Correntista conto)
        {
            var model = _db.Entry(conto);
            model.State = EntityState.Added;
            await _db.SaveChangesAsync();
            return conto;
        }

        public async Task DeleteCorrentista(Correntista Nconto)
        {
            _db._Correntista.Remove(Nconto);
            await _db.SaveChangesAsync();


        }

        public async Task<Correntista> UpdateCorrentista(Correntista corr)
        {

            var model = _db._Correntista.Entry(corr);
            model.State = EntityState.Modified;

            await _db.SaveChangesAsync();
            return corr;
        }

        public async Task DeleteMovimenti(Movimenti Nconto)
        {
            _db._Movimenti.Remove(Nconto);
            await _db.SaveChangesAsync();
        }


        //public async Task<Movimenti> UpdateMovAsync(Movimenti conto)
        //{
        //    _db.Update(conto);
        //    await _db.SaveChangesAsync();
        //    return conto;
        //}

          public async Task<Correntista?> GetCorrentista(string conto) => await _db._Correntista.FirstOrDefaultAsync(n => n.NcontoCorr == conto);




    }


}
