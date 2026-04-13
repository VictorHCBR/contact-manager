using ContactManager.Domain.Entities;

namespace ContactManager.Application.Interfaces;

public interface IContactRepository
{
    Task<IReadOnlyList<Contact>> ListAsync(bool includeInactive, CancellationToken cancellationToken = default);
    Task<Contact?> GetByIdAsync(Guid id, bool includeInactive, CancellationToken cancellationToken = default);
    Task AddAsync(Contact contact, CancellationToken cancellationToken = default);
    void Remove(Contact contact);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
