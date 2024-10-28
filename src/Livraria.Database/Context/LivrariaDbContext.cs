using System;
using Livraria.Domain.Contracts.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace Livraria.Database.Context;
public class LivrariaDbContext : DbContext
{
    public LivrariaDbContext(DbContextOptions<LivrariaDbContext> options)
            : base(options)
    {
        
    }

    public DbSet<Book> Books { get; set; }
}
