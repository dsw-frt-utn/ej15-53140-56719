using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Dsw2026Ej15.Data.Services;

public class PersistenceInMemory : IPersistence
{
    private readonly List<Speciality> _specialities = new();
    private readonly List<Doctor> _doctors = new();

    public PersistenceInMemory()
    {
        LoadSpecialities();
    }

    //doctors
    public async Task AddDoctorAsync(Doctor doctor)
    {
        _doctors.Add(doctor);
        await Task.CompletedTask;
    }
    public async Task<Doctor?> GetDoctorByIdAsync(Guid id)
    {
        return await Task.FromResult(_doctors.FirstOrDefault(d => d.Id == id && d.IsActive));
    }

    public async Task<IEnumerable<Doctor>> GetActiveDoctorsAsync()
    {
        return await Task.FromResult(_doctors.Where(d => d.IsActive).ToList());
    }

    public async Task<bool> UpdateDoctorAsync(Doctor doctor)
    {
        var existingDoctor = _doctors.FirstOrDefault(d => d.Id == doctor.Id && d.IsActive);
        if (existingDoctor != null)
        {
            existingDoctor.Name = doctor.Name;
            existingDoctor.LicenseNumber = doctor.LicenseNumber;
            existingDoctor.IsActive = doctor.IsActive;
            existingDoctor.speciality = doctor.speciality;
            return await Task.FromResult(true);
        }
        return await Task.FromResult(false);
    }

    public async Task<bool> DeleteDoctorAsync(Guid id)
    {
        var doctor = _doctors.FirstOrDefault(d => d.Id == id && d.IsActive);
        if (doctor != null)
        {
            doctor.IsActive = false;
            return await Task.FromResult(true);
        }
        return await Task.FromResult(false);
    }

    //specialities
    public async Task<IEnumerable<Speciality>> GetAllSpecialitiesAsync()
    {
        return await Task.FromResult((IEnumerable<Speciality>)_specialities.AsReadOnly());
    }
    public async Task AddSpecialityAsync(Speciality speciality)
    {
        _specialities.Add(speciality);
        await Task.CompletedTask;
    }

    public async Task<Speciality?> GetSpecialityByIdAsync(Guid id)
    {
        return await Task.FromResult(_specialities.FirstOrDefault(s => s.Id == id));
    }

    private void LoadSpecialities()
    {
        string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "specialities.json");
        if (File.Exists(fileName))
        {
            var jsonString = File.ReadAllText(fileName);
            var specialitiesFromJson = JsonSerializer.Deserialize<List<Speciality>>(jsonString);
            if (specialitiesFromJson != null)
            {
                _specialities.AddRange(specialitiesFromJson);
            }
        }
    }
}
