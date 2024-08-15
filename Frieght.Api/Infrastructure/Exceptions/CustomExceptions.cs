namespace Frieght.Api.Infrastructure.Exceptions;
public class DuplicateLoadException : Exception
{
  public DuplicateLoadException(string message) : base(message) { }
}
