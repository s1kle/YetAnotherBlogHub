namespace BlogHub.Data.Exceptions;

internal sealed class NotFoundException : Exception
{
    public NotFoundException(string entity) : base($"Entity '{entity}' not found.") { }
}