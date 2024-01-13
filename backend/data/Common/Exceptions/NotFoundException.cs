namespace BlogHub.Data.Common.Exceptions;

public sealed class NotFoundException : Exception
{
    public NotFoundException(string entity) : base($"Entity '{entity}' not found.") { }
}