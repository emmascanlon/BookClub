namespace BookClub.Bff.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }

    public string Type { get; set; }
}