using ContactManager.Application.Interfaces;
using ContactManager.Domain.Entities;
using ContactManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ContactManager.Infrastructure.Repositories;

public sealed class ContactRepository(AppDbContext dbContext) : IContactRepository
{
    public async Task<IReadOnlyList<Contact>> ListAsync(bool includeInactive, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Contacts.AsQueryable();

        if (!includeInactive)
        {
            query = query.Where(contact => contact.IsActive);
        }

        return await query
            .OrderBy(contact => contact.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Contact?> GetByIdAsync(Guid id, bool includeInactive, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Contacts.AsQueryable();

        if (!includeInactive)
        {
            query = query.Where(contact => contact.IsActive);
        }

        return await query.FirstOrDefaultAsync(contact => contact.Id == id, cancellationToken);
    }

    public Task AddAsync(Contact contact, CancellationToken cancellationToken = default)
        => dbContext.Contacts.AddAsync(contact, cancellationToken).AsTask();

    public void Remove(Contact contact)
        => dbContext.Contacts.Remove(contact);

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => dbContext.SaveChangesAsync(cancellationToken);
}
