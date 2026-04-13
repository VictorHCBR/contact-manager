using ContactManager.Api.Contracts.Requests;
using ContactManager.Api.Contracts.Responses;
using ContactManager.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContactManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ContactsController(IContactService contactService) : ControllerBase
{
    /// <summary>
    /// Lista contatos ativos por padrão. Envie includeInactive=true para incluir os inativos.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ContactResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ContactResponse>>> GetAll([FromQuery] bool includeInactive = false, CancellationToken cancellationToken = default)
    {
        var contacts = await contactService.ListAsync(includeInactive, cancellationToken);
        return Ok(contacts.Select(ContactResponse.FromEntity));
    }

    /// <summary>
    /// Busca um contato pelo identificador.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ContactResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContactResponse>> GetById(Guid id, [FromQuery] bool includeInactive = false, CancellationToken cancellationToken = default)
    {
        var contact = await contactService.GetByIdAsync(id, includeInactive, cancellationToken);
        return Ok(ContactResponse.FromEntity(contact));
    }

    /// <summary>
    /// Cria um novo contato.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ContactResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ContactResponse>> Create(CreateContactRequest request, CancellationToken cancellationToken = default)
    {
        var contact = await contactService.CreateAsync(request.Name, request.BirthDate, request.Gender, cancellationToken);
        var response = ContactResponse.FromEntity(contact);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    /// <summary>
    /// Atualiza um contato ativo.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ContactResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContactResponse>> Update(Guid id, UpdateContactRequest request, CancellationToken cancellationToken = default)
    {
        var contact = await contactService.UpdateAsync(id, request.Name, request.BirthDate, request.Gender, cancellationToken);
        return Ok(ContactResponse.FromEntity(contact));
    }

    /// <summary>
    /// Ativa um contato previamente desativado.
    /// </summary>
    [HttpPatch("{id:guid}/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Activate(Guid id, CancellationToken cancellationToken = default)
    {
        await contactService.ActivateAsync(id, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Desativa um contato.
    /// </summary>
    [HttpPatch("{id:guid}/deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken = default)
    {
        await contactService.DeactivateAsync(id, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Remove permanentemente um contato.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        await contactService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
