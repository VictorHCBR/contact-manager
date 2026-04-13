using ContactManager.Application.Interfaces;
using ContactManager.Domain.Entities;
using ContactManager.Domain.Enums;

namespace ContactManager.Application.Services;

public sealed class ContactService(IContactRepository repository) : IContactService
{
    public Task<IReadOnlyList<Contact>> ListAsync(bool includeInactive, CancellationToken cancellationToken = default)
        => repository.ListAsync(includeInactive, cancellationToken);

    public async Task<Contact> GetByIdAsync(Guid id, bool includeInactive, CancellationToken cancellationToken = default)
    {
        var contact = await repository.GetByIdAsync(id, includeInactive, cancellationToken);
        return contact ?? throw new EntityNotFoundException("Contato não encontrado.");
    }

    public async Task<Contact> CreateAsync(string name, DateOnly birthDate, Gender gender, CancellationToken cancellationToken = default)
    {
        var contact = new Contact(name, birthDate, gender);
        await repository.AddAsync(contact, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        return contact;
    }

    public async Task<Contact> UpdateAsync(Guid id, string name, DateOnly birthDate, Gender gender, CancellationToken cancellationToken = default)
    {
        var contact = await repository.GetByIdAsync(id, includeInactive: false, cancellationToken)
            ?? throw new EntityNotFoundException("Contato ativo não encontrado para edição.");

        contact.Update(name, birthDate, gender);
        await repository.SaveChangesAsync(cancellationToken);
        return contact;
    }

    public async Task ActivateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contact = await repository.GetByIdAsync(id, includeInactive: true, cancellationToken)
            ?? throw new EntityNotFoundException("Contato não encontrado.");

        contact.Activate();
        await repository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeactivateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contact = await repository.GetByIdAsync(id, includeInactive: true, cancellationToken)
            ?? throw new EntityNotFoundException("Contato não encontrado.");

        contact.Deactivate();
        await repository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contact = await repository.GetByIdAsync(id, includeInactive: true, cancellationToken)
            ?? throw new EntityNotFoundException("Contato não encontrado.");

        repository.Remove(contact);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
