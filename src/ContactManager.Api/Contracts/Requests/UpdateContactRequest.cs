using ContactManager.Domain.Enums;

namespace ContactManager.Api.Contracts.Requests;

public sealed record UpdateContactRequest(
    string Name,
    DateOnly BirthDate,
    Gender Gender);
