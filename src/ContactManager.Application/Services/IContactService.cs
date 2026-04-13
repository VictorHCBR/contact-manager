using ContactManager.Domain.Entities;
using ContactManager.Domain.Enums;

namespace ContactManager.Application.Services;

public interface IContactService
{
    Task<IReadOnlyList<Contact>> ListAsync(bool includeInactive, CancellationToken cancellationToken = default);
    Task<Contact> GetByIdAsync(Guid id, bool includeInactive, CancellationToken cancellationToken = default);
    Task<Contact> CreateAsync(string name, DateOnly birthDate, Gender gender, CancellationToken cancellationToken = default);
    Task<Contact> UpdateAsync(Guid id, string name, DateOnly birthDate, Gender gender, CancellationToken cancellationToken = default);
    Task ActivateAsync(Guid id, CancellationToken cancellationToken = default);
    Task DeactivateAsync(Guid id, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
