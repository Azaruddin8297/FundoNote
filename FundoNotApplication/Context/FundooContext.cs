using FundoNotApplication.Entities;
using Microsoft.EntityFrameworkCore;

namespace FundoNotApplication.Context;

public class FundooContext : DbContext
{
    public FundooContext(DbContextOptions options) : base(options)
    {

    }


    public DbSet<UserEntity> Users { get; set; }
    public DbSet<NotesEntity> Notes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<UserEntity>().HasNoDiscriminator().HasManualThroughput(400).HasKey(x => x.EmailId);
        modelBuilder.Entity<UserEntity>().ToContainer("UserContainer").HasPartitionKey(x => x.EmailId);

        modelBuilder.Entity<NotesEntity>().HasNoDiscriminator().HasManualThroughput(600).HasKey(x => x.NoteId);
        modelBuilder.Entity<NotesEntity>().ToContainer("NotesContainer").HasPartitionKey(x => x.EmailId);
    }
}
