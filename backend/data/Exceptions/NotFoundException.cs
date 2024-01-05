namespace BlogHub.Data.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string entity) : base($"Entity '{entity}' not found.") { }
}