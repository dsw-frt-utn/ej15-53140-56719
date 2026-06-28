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

        var responseDto = new DoctorModel.Response(
        createdDoctor.Name,
        createdDoctor.LicenseNumber,
        createdDoctor.Speciality?.Name,
        createdDoctor.Id
        );

        return StatusCode(201, responseDto);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Doctor>>> GetActiveDoctors()
    {
        var doctorsActives = await _persistence.GetActiveDoctorsAsync();

        var doctorsDtos = doctorsActives.Select(d => new DoctorModel.Response(
        Name: d.Name,
        LicenseNumber: d.LicenseNumber,
        Speciality: d.Speciality?.Name,
        Id : d.Id
        ));

        return Ok(doctorsDtos);

    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDoctorById([FromRoute] Guid id)
    {
        var doctor = (await GetDoctor(id));
        return Ok(new DoctorModel.Response(
            doctor.Name,
            doctor.LicenseNumber,
            doctor.Speciality?.Name,
            doctor.Id
        ));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDoctor([FromRoute] Guid id)
    {
        var doctor = (await GetDoctor(id));
        doctor.Deactivate();
        await _persistence.UpdateDoctorAsync(doctor);
        return NoContent();
    }

    private async Task<Doctor> GetDoctor(Guid id)
    {
        return await _persistence.GetDoctorByIdAsync(id) ?? throw new ValidationException("Medico no encontrado");
    }
}