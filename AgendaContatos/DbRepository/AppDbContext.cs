using AgendaContatos.Models;
using Microsoft.EntityFrameworkCore;

namespace AgendaContatos.DbRepository 
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
   
        public DbSet<Contato> Contatos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Contato>().HasData(new Contato { id = 1, nome = "Giovanni", telefone = "", email = "giovanniamendola_0@yahoo.com.br" });
        }


    }

    
}
