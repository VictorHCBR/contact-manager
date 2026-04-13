using ContactManager.Domain.Entities;
using ContactManager.Domain.Enums;

namespace ContactManager.Api.Contracts.Responses;

public sealed record ContactResponse(
    Guid Id,
    string Name,
    DateOnly BirthDate,
    Gender Gender,
    int Age,
    bool IsActive,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc)
{
    public static ContactResponse FromEntity(Contact contact) =>
        new(
            contact.Id,
            contact.Name,
            contact.BirthDate,
            contact.Gender,
            contact.Age,
            contact.IsActive,
            contact.CreatedAtUtc,
            contact.UpdatedAtUtc);
}
