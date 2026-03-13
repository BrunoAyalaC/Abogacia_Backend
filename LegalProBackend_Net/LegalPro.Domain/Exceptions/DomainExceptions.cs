namespace LegalPro.Domain.Exceptions;

/// <summary>
/// Base exception for domain rule violations.
/// These should be caught by the Application layer and converted to user-friendly messages.
/// </summary>
public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
    public DomainException(string message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
/// Thrown when an entity is not found by its identifier.
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string entityName, object key)
        : base($"La entidad '{entityName}' con identificador ({key}) no fue encontrada.") { }
}

/// <summary>
/// Thrown when a business rule forbids the operation.
/// </summary>
public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException() : base("No tiene permisos para realizar esta acción.") { }
    public ForbiddenAccessException(string message) : base(message) { }
}
