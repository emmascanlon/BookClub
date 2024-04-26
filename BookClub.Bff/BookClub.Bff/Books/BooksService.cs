
namespace BookClub.Bff.Books;

public class BooksService : IBooksService
{
    public async Task<List<Book>> GetAllBooks()
    {
        var response =  await Task.FromResult(new List<Book>{new() {Title = "Tomorrow and Tomorrow and Tomorrow", Author = "Bob Jones", Cover = "Something", Summary="something"}});
        return response;
    }
}