using Microsoft.AspNetCore.Mvc;

namespace BookClub.Bff.Books;

[ApiController]
[Route("api/[controller]/[action]")]
public class BooksController: ControllerBase
{
    private readonly IBooksService _booksService;
    public BooksController(IBooksService booksService)
    {
        _booksService = booksService;
    }

    [HttpGet]
    public async Task<List<Book>> GetBooks()
    {
        var response = await _booksService.GetAllBooks();
        return response;
    }

}