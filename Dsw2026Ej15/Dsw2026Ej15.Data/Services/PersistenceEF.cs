using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dsw2026Ej15.Data.Services;

public class PersistenceEF : IPersistence
{
    private readonly Dsw2026Ej15DbContext _context;
    public PersistenceEF(Dsw2026Ej15DbContext context)
    {
        _context = context;
    }
    public async Task AddDoctorAsync(Doctor doctor)
    {
        _context.Doctors.Add(doctor);
        await _context.SaveChangesAsync();
    }
    public async Task<IEnumerable<Doctor>> GetActiveDoctorsAsync()
    {
        return await _context.Doctors
            .Where(d => d.IsActive)
            .Include(d => d.Speciality)
            .ToListAsync();
    }

    public async Task<Doctor?> GetDoctorByIdAsync(Guid id)
    {
        return await _context.Doctors
            .Include(d => d.Speciality)
            .FirstOrDefaultAsync(d => d.Id == id && d.IsActive);
    }
    public async Task AddSpecialityAsync(Speciality speciality)
    {
        _context.Specialities.Add(speciality);
        await _context.SaveChangesAsync();
    }

    public async Task<Speciality?> GetSpecialityByIdAsync(Guid id)
    {
        return await _context.Specialities.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task UpdateDoctorAsync(Doctor doctor)
    {
        _context.Doctors.Update(doctor);
        await _context.SaveChangesAsync();
    }
}
