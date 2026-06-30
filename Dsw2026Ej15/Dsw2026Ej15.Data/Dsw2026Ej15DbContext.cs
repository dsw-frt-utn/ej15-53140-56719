using Dsw2026Ej15.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dsw2026Ej15.Data;

public class Dsw2026Ej15DbContext: DbContext
{
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Speciality> Specialities { get; set; }
    public Dsw2026Ej15DbContext(DbContextOptions<Dsw2026Ej15DbContext> options): base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.ToTable("Doctors");
            entity.HasKey(d => d.Id);
            entity.Property(d => d.Name).HasMaxLength(30).IsRequired();
            entity.Property(d => d.LicenseNumber).HasMaxLength(20).IsRequired();
            entity.HasIndex(d => d.LicenseNumber).IsUnique();
        });

        modelBuilder.Entity<Speciality>(entity =>
        {
            entity.ToTable("Specialities");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Name).HasMaxLength(30).IsRequired();
            entity.Property(s => s.Description).HasMaxLength(150);
        });
    }

}
