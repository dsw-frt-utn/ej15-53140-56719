namespace Dsw2026Ej15.Domain.Entities;

public abstract class BaseEntity
{
    public Guid? Id { get; private set; }
    protected BaseEntity(Guid? id)
    {
        Id = id ?? Guid.NewGuid();
    }
}
