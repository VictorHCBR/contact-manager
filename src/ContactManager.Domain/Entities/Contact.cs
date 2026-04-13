using ContactManager.Domain.Enums;
using ContactManager.Domain.Exceptions;

namespace ContactManager.Domain.Entities;

public sealed class Contact
{
    private Contact()
    {
    }

    public Contact(string name, DateOnly birthDate, Gender gender)
    {
        Id = Guid.NewGuid();
        SetName(name);
        SetBirthDate(birthDate);
        Gender = gender;
        IsActive = true;
        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public DateOnly BirthDate { get; private set; }
    public Gender Gender { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime UpdatedAtUtc { get; private set; }

    public int Age => CalculateAge(BirthDate, DateOnly.FromDateTime(DateTime.UtcNow));

    public void Update(string name, DateOnly birthDate, Gender gender)
    {
        EnsureCanUseBirthDate(birthDate);
        SetName(name);
        BirthDate = birthDate;
        Gender = gender;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Activate()
    {
        if (IsActive)
        {
            return;
        }

        IsActive = true;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        if (!IsActive)
        {
            return;
        }

        IsActive = false;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainValidationException("O nome do contato é obrigatório.");
        }

        Name = name.Trim();
    }

    private void SetBirthDate(DateOnly birthDate)
    {
        EnsureCanUseBirthDate(birthDate);
        BirthDate = birthDate;
    }

    private static void EnsureCanUseBirthDate(DateOnly birthDate)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        if (birthDate > today)
        {
            throw new DomainValidationException("A data de nascimento não pode ser maior que a data atual.");
        }

        var age = CalculateAge(birthDate, today);

        if (age <= 0)
        {
            throw new DomainValidationException("A idade do contato não pode ser igual ou inferior a zero.");
        }

        if (age < 18)
        {
            throw new DomainValidationException("O contato deve ser maior de idade.");
        }
    }

    private static int CalculateAge(DateOnly birthDate, DateOnly today)
    {
        var age = today.Year - birthDate.Year;

        if (birthDate > today.AddYears(-age))
        {
            age--;
        }

        return age;
    }
}
