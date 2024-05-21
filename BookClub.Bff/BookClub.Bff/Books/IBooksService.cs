namespace BookClub.Bff.Books;
public interface IBooksService
{
    Task<List<Book>> GetAllBooks();
}