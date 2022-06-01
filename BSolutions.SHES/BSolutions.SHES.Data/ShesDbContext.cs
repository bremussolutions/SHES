using BSolutions.SHES.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace BSolutions.SHES.Data
{
    public class ShesDbContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }

        public DbSet<Building> Buildings { get; set; }

        public DbSet<ProjectItem> ProjectItems { get; set; }

        public DbSet<BuildingPart> BuildingParts { get; set; }

        public DbSet<Floor> Floors { get; set; }

        public DbSet<Corridor> Corridors { get; set; }

        public DbSet<Cabinet> Cabinets { get; set; }

        public DbSet<Stair> Stairs { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<Device> Devices { get; set; }

        public ShesDbContext()
        {
        }

        public ShesDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Create database directory
            string userDocumentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string shesDatabasePath = Path.Combine(userDocumentPath, "SHES");
            Directory.CreateDirectory(shesDatabasePath);

            optionsBuilder.UseSqlite($"Data Source={Path.Combine(shesDatabasePath, "shes.db")};");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>().ToTable("Projects")
                .HasMany(p => p.Buildings)
                .WithOne(p => p.Project)
                .HasForeignKey(b => b.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProjectItem>().ToTable("ProjectItems")
                .HasMany(i => i.Children)
                .WithOne(i => i.Parent)
                .HasForeignKey(i => i.ParentId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
