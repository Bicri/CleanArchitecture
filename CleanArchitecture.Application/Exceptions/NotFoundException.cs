namespace CleanArchitecture.Application.Exceptions;

public class NotFoundException : ApplicationException
{
    public NotFoundException(string name, Object key) : base ($"Entity \"{name}\" ({key}) no fue encontrado")
    {    
    }
}
