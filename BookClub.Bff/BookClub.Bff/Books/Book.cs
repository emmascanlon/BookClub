namespace BookClub.Bff.Books;

public record Book
{
    public string Title {get; set;}
    public string Author {get; set;}
    public string Summary {get; set;}
    public string Cover {get; set;}
}