using Microsoft.EntityFrameworkCore;
using MimicAPI.V1.Models;

namespace MimicAPI.Database
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options)
            : base(options)
        {
            
        }

        public DbSet<Palavra> Palavras { get; set; }
    }
}
