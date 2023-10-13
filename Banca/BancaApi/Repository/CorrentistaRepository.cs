using BancaApi.Controllers;
using BancaApi.Data;
using BancaApi.Models;
using BancaApi.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BancaApi.Repository
{
    public class CorrentistaRepository : ICorrentistaRepository
    {
        private readonly AppDbContext _db;
        private readonly ILogger<CorrentistaRepository> _logger;
        public CorrentistaRepository(AppDbContext db, ILogger<CorrentistaRepository> logger)
        {
            _db = db;
            _logger = logger;
        }


        public async Task<Correntista?> GetCorrentista(string? userName, string? email, string? password)
        {

            //  var query = _db._Correntista.AsQueryable();


            //query = query.Where(st => st.UserName == (userName ?? st.UserName))
            //   .Where(st => st.Pin == (pwd ?? st.Pin))
            //   .Where(st => st.Email == (email ?? st.Email));
            //var result = await query.FirstOrDefaultAsync();
            var query = await _db._Correntista
               .Where(st => st.UserName == (userName ?? st.UserName))
               .Where(st => st.Email == (email ?? st.Email))
               .FirstOrDefaultAsync();

            return query;

        }
        public async Task<Movimenti> CreateMovimenti(Movimenti mov)
        {
            var model = _db.Entry(mov);
            model.State = EntityState.Added;
            await _db.SaveChangesAsync();
            return mov;
        }

        public async Task<Movimenti?> GetMovimenti(string Nconto) => await _db._Movimenti.Include(c => c.NumConto).FirstOrDefaultAsync();

        public async Task<Correntista?> GetCorrentista(string Nconto) => await _db._Correntista.FirstOrDefaultAsync(n => n.NcontoCorr == Nconto);



        public async Task<Results<Ok, NotFound>> UpdateCorrentista(Correntista corr)
        {

            var query = await _db._Correntista
                   .Where(model => model.UserName == corr.UserName)
                   .ExecuteUpdateAsync(setters => setters
                   .SetProperty(m => m.Pin, corr.Pin));
            if (query < 1) { _logger.LogError("UP Pwd Correntista ",query); }

            return query == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        }

        public async Task<List<Movimenti>> GetMovimenti(string Nconto, DateTime? start, DateTime? end, string? tipo)
        {
           
            var query = await _db._Movimenti
            .Where(x => x.Nconto == Nconto)
            .Where(st => st.TipoMov == (tipo ?? st.TipoMov))
            .Where(st => st.DataContoMov >= (start ?? st.DataContoMov))
            .Where(st => st.DataContoMov <= (end ?? st.DataContoMov))
            .OrderByDescending(x => x.DataContoMov).ToListAsync();

            return query;
        }


    }
}


    

