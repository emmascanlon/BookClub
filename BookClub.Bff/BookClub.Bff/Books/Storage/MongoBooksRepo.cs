using BookClub.Bff.Books;
using MongoDB.Driver;

namespace BookClub.Bff.Storage;

public class MongoBooksRepo : IBooksRepo
{
    private readonly IMongoCollection<Book> _booksCollection;
    private const string DbName = "book-club";
    private const string CollectionName = "books";

    public MongoBooksRepo(IMongoClient database)
    {
        _booksCollection = database.GetDatabase(DbName).GetCollection<Book>(CollectionName);
    }
    public async Task<List<Book>> GetAllBooks()
    {
        var gettingBooks = _booksCollection.Find(Builders<Book>.Filter.Empty)
        .Project<Book>(Builders<Book>.Projection.Exclude("_id"))
        .ToListAsync();

        var books = await gettingBooks;

        return books;
    }
}