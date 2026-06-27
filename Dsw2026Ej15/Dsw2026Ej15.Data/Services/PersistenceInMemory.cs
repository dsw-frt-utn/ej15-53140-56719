using Dsw2026Ej15.Data.DTOs;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Dsw2026Ej15.Data.Services;

public class PersistenceInMemory : IPersistence
{
    private List<Speciality> _specialities = [];
    private List<Doctor> _doctors = [];

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
        return await Task.FromResult(_doctors.SingleOrDefault(d => d.Id == id && d.IsActive));
    }

    public async Task<IEnumerable<Doctor>> GetActiveDoctorsAsync()
    {
       var activeDoctors = _doctors.Where(d => d.IsActive).ToList();
        foreach (var doctor in activeDoctors)
        {
            doctor.Speciality = _specialities.FirstOrDefault(s => s.Id == doctor.Speciality?.Id);
        }
        return await Task.FromResult(activeDoctors);
    }

    public async Task DeactivateDoctorAsync(Guid id)
    {
        var doctor = _doctors.SingleOrDefault(d => d.Id == id);
        if (doctor != null)
        {
            doctor.IsActive = false;
        }
        await Task.CompletedTask;
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
        return await Task.FromResult(_specialities.SingleOrDefault(s => s.Id == id));
    }

    private void LoadSpecialities()
    {
          try
          { 
         
            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Sources","specialities.json");
            var json = File.ReadAllText(fileName);
            var specialityDtos = JsonSerializer.Deserialize<List<SpecialityDto>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? [];

            _specialities = specialityDtos.Select(s => new Speciality(s.Name, s.Description, s.Id)).ToList();
          }
          catch (Exception)
          {
              Console.WriteLine("Error loading specialities from JSON file.");
          }
    }       
}
