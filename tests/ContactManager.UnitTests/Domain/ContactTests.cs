using ContactManager.Domain.Entities;
using ContactManager.Domain.Enums;
using ContactManager.Domain.Exceptions;
using Xunit;

namespace ContactManager.UnitTests.Domain;

public sealed class ContactTests
{
    [Fact]
    public void Constructor_Should_Create_Active_Adult_Contact()
    {
        var birthDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-25));

        var contact = new Contact("Maria Silva", birthDate, Gender.Female);

        Assert.Equal("Maria Silva", contact.Name);
        Assert.True(contact.IsActive);
        Assert.True(contact.Age >= 25 || contact.Age == 24);
    }

    [Fact]
    public void Constructor_Should_Throw_When_BirthDate_Is_In_The_Future()
    {
        var birthDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));

        var exception = Assert.Throws<DomainValidationException>(() =>
            new Contact("João", birthDate, Gender.Male));

        Assert.Contains("data de nascimento", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Constructor_Should_Throw_When_Contact_Is_Underage()
    {
        var birthDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-17));

        var exception = Assert.Throws<DomainValidationException>(() =>
            new Contact("Carlos", birthDate, Gender.Male));

        Assert.Contains("maior de idade", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Update_Should_Change_Main_Fields()
    {
        var contact = new Contact("Nome Antigo", DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-30)), Gender.Other);
        var newBirthDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-40));

        contact.Update("Nome Novo", newBirthDate, Gender.Male);

        Assert.Equal("Nome Novo", contact.Name);
        Assert.Equal(newBirthDate, contact.BirthDate);
        Assert.Equal(Gender.Male, contact.Gender);
    }

    [Fact]
    public void Deactivate_And_Activate_Should_Update_Status()
    {
        var contact = new Contact("Ana", DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-28)), Gender.Female);

        contact.Deactivate();
        Assert.False(contact.IsActive);

        contact.Activate();
        Assert.True(contact.IsActive);
    }
}
