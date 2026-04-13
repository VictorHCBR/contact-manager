using ContactManager.Domain.Enums;

namespace ContactManager.Api.Contracts.Requests;

public sealed record CreateContactRequest(
    string Name,
    DateOnly BirthDate,
    Gender Gender);
