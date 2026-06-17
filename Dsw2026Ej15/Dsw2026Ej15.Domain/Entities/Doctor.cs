using System;
using System.Collections.Generic;
using System.Text;

namespace Dsw2026Ej15.Domain.Entities;

public class Doctor : BaseEntity
{
    public string? Name { get; set; }
    public string? LicenseNumber { get; set; }
    public bool IsActive { get; set; }
    public Guid SpecialityId { get; set; }
    public Speciality? Speciality { get; set; }

    public Doctor()
    {
        Name = string.Empty;
        LicenseNumber = string.Empty;
        Speciality = null;
    }
    public Doctor(string name, string licenseNumber, Speciality speciality, Guid? id = null) : base()
    {
        Name = name;
        LicenseNumber = licenseNumber;
        SpecialityId = speciality.Id;
        Speciality = speciality;
        IsActive = true;
    }
}
