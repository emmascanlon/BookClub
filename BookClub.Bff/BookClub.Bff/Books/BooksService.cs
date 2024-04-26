
using System.ComponentModel;
using BookClub.Bff.Storage;

namespace BookClub.Bff.Books;

public class BooksService : IBooksService
{
    private readonly IBooksRepo _repo;
    public BooksService(IBooksRepo repo)
    {
        _repo = repo;
    }
    public async Task<List<Book>> GetAllBooks()
    {
        return await _repo.GetAllBooks();
    }
}