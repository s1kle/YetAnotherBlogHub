namespace BlogHub.Data.Exceptions;

public sealed class NotFoundException : Exception
{
    public NotFoundException(string entity) : base($"Entity '{entity}' not found.") { }
}