using BookClub.Bff.Books;

namespace BookClub.Bff.Storage;
public interface IBooksRepo
{
    Task<List<Book>> GetAllBooks();
}