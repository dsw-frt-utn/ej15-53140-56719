using Dsw2026Ej15.Domain.Interfaces;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Dsw2026Ej15.Api.Models;

namespace Dsw2026Ej15.Controllers;

[ApiController]
[Route("api/doctors")]
public class DoctorsController : ControllerBase
{
    private readonly IPersistence _persistence;

    public DoctorsController(IPersistence persistence)
    {
        _persistence = persistence;
    }

    [HttpPost]
    public async Task<IActionResult> CreateDoctor([FromBody] DoctorModel.Request doctor)
    {

       if (string.IsNullOrEmpty(doctor.Name))
            throw new ValidationException("Doctor name is required.");
       if (string.IsNullOrEmpty(doctor.LicenseNumber))
            throw new ValidationException("Doctor license number is required.");

       var speciality = await _persistence.GetSpecialityByIdAsync(doctor.SpecialityId);
       if (speciality == null)
            throw new ValidationException("Speciality not found.");

        var createdDoctor = new Doctor(doctor.Name, doctor.LicenseNumber, speciality);

        await _persistence.AddDoctorAsync(createdDoctor);

        return StatusCode(201, createdDoctor);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DoctorModel.Response>>> GetActiveDoctors()
    {
        var doctorsActives = await _persistence.GetActiveDoctorsAsync();

        try
        {
            var doctorsDtos = doctorsActives.Select(d => new DoctorModel.Response(
                Name: d.Name,
                LicenseNumber: d.LicenseNumber,
                Speciality: d.Speciality?.Name
                )).ToList();

            return Ok(doctorsDtos);
        }
        catch (Exception)
        {
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DoctorModel.Response>> GetDoctorById(Guid id)
    {
        var doctor = await _persistence.GetDoctorByIdAsync(id);

        if(doctor == null)
        {
            return NotFound();
        }

        var doctorDto = new DoctorModel.Response(
            Name: doctor.Name, LicenseNumber: doctor.LicenseNumber, Speciality: doctor.Speciality?.Name);

        return Ok(doctorDto);
    }
}