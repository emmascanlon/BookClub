namespace BookClub.Bff.Exceptions;

public class AggregateDomainException : DomainException
{
    public List<string> Errors { get; set; }
    public AggregateDomainException(string message) : base(message)
    { }
    public AggregateDomainException(string message, List<string> errors) : base(message)
    {
        Errors = errors;
    }
}
