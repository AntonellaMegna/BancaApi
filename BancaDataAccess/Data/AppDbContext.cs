using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BancaModels.Models;
using BancaModels.Models.DTO;

namespace BancaDataAccess.Data
{
    public class AppDbContext: IdentityDbContext<ApplicationUser>
    {
      
        public AppDbContext(DbContextOptions<AppDbContext>options):base(options) 
        {
            
        }
        public DbSet<DipendenteBanca> _DipendenteBanca { get; set; }
        public DbSet<ContoCorrente> _ContoCorrente { get; set; }
        public DbSet<Correntista> _Correntista { get; set; }
        public DbSet<Movimenti> _Movimenti { get; set; }
        public DbSet<Ruoli> Role { get; set; }
        public DbSet<User> User { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Correntista>().HasAlternateKey(p => p.NcontoCorr);
        //    modelBuilder.Entity<Movimenti>().HasAlternateKey(p => p.Nconto);
        //}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<ContoCorrente>()
        //       .Property(a => a.Nconto).IsConcurrencyToken();
        //}

        /* protected override void OnModelCreating(ModelBuilder model )
         {
             model.Entity<ContoCorrente>().HasData(
                 new ContoCorrente { Amount = 0, Busy = true, Iban = "It345678", Ncarta = "456789K", Nconto = "1ee23456789" },
                  new ContoCorrente { Amount = 0, Busy = true, Iban = "IT00RTU9097", Ncarta = "678934F", Nconto = "678934F", Pin = "1234" }
              



                 );

         }*/
    }
}
