using Microsoft.EntityFrameworkCore;
using Verkehrs_Informationen.Database.Models;
using Verkehrs_Informationen.Models;

public class MyDatabaseContext : DbContext
{
    public DbSet<WarningItemEntity> WarningItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=VerkehrsDB;Trusted_Connection=True;");
    }
}