using Dsw2026Ej15.Domain.Interfaces;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Dsw2026Ej15.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly IPersistence _persistence;

    public DoctorsController(IPersistence persistence)
    {
        _persistence = persistence;
    }

    //[HttpPost]
    //public async Task<IActionResult> CreateDoctor([FromBody] Doctor doctor)
    //{
    //    try
    //    {
    //        if(string.IsNullOrEmpty(doctor.Name))
    //            throw new ValidationException("Doctor name is required.");
    //        if(string.IsNullOrEmpty(doctor.LicenseNumber))
    //            throw new ValidationException("Doctor license number is required.");
            
    //        var speciality = await _persistence.GetSpecialityByIdAsync(doctor.specialityId);
    //        if(speciality == null)
    //            throw new ValidationException("Speciality not found.");

    //        var createdDoctor = new Doctor
    //        {

    //        };

    //        return StatusCode(201);

    //    catch(ValidationException ex) 
    //    {
    //        return BadRequest(ex.Message);
    //    }
    //    catch(Exception)
    //    {
    //        throw;
    //    }
}
