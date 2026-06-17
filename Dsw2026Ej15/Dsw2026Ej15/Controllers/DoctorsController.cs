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

       var createdDoctor = new Doctor
       {
           Id = Guid.NewGuid(),
           Name = doctor.Name.Trim(),
           LicenseNumber = doctor.LicenseNumber.Trim(),
           IsActive = true,
           SpecialityId = doctor.SpecialityId
        };

        await _persistence.AddDoctorAsync(createdDoctor);

        return StatusCode(201, createdDoctor);
    }


}