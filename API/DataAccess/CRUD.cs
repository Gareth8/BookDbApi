using Microsoft.VisualBasic;
using Npgsql;

namespace BookDbApi.DataAccess
{
    public class CRUD
    {
        #region Variables
        static readonly string? dBHost = Environment.GetEnvironmentVariable("DATABASE_HOST");
        static readonly string? dBUsername = Environment.GetEnvironmentVariable("DATABASE_USERNAME");
        static readonly string? dBPassword = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");
        static readonly string? dBTarget = Environment.GetEnvironmentVariable("DATABASE_TARGET");
        
        readonly string m_connectionString = $"Host={dBHost};Username={dBUsername};Password={dBPassword};Database={dBTarget}";
        private NpgsqlConnection m_connection { get; set; }
        #endregion
        public CRUD()
        {
            m_connection = new NpgsqlConnection(m_connectionString);
            m_connection.Open();
        }

        public async Task<bool> Create(Book p_book)
        {
            try
            {
                if (!await BookExistsTitle((p_book.GetTitle())))
                {
                    await using (var insertBook = new NpgsqlCommand("INSERT INTO bookdb.books (title, isbn) VALUES (@title, @isbn);", m_connection))
                    {
                        insertBook.Parameters.AddWithValue("title", p_book.GetTitle());
                        insertBook.Parameters.AddWithValue("isbn", p_book.GetIsbn());

                        await insertBook.ExecuteNonQueryAsync();
                    }
                }

                if (!await AuthorExists(p_book.GetAuthor()))
                {
                    await using (var insertAuthor = new NpgsqlCommand("INSERT INTO bookdb.authors (author_name) VALUES (@author);", m_connection))
                    {
                        insertAuthor.Parameters.AddWithValue("author", p_book.GetAuthor());
                        await insertAuthor.ExecuteNonQueryAsync();
                    }
                }

                if (!await PublisherExists(p_book.GetPublisher()))
                {
                    await using (var insertPublisher = new NpgsqlCommand("INSERT INTO bookdb.publishers (publisher_name) VALUES (@publisher);", m_connection))
                    {
                        insertPublisher.Parameters.AddWithValue("publisher", p_book.GetPublisher());
                        await insertPublisher.ExecuteNonQueryAsync();
                    }
                }
                
                await using (var updateBookPublisherLink = new NpgsqlCommand("UPDATE bookdb.books " +
                    "SET publisher_id = (SELECT publisher_id FROM bookdb.publishers WHERE publisher_name = @publisher) " +
                    "WHERE title = @title;", m_connection))
                {
                    updateBookPublisherLink.Parameters.AddWithValue("publisher", p_book.GetPublisher());
                    updateBookPublisherLink.Parameters.AddWithValue("title", p_book.GetTitle());
                    await updateBookPublisherLink.ExecuteNonQueryAsync();
                }

                await using (var insertAuthorBookLink = new NpgsqlCommand("INSERT INTO bookdb.author_book_link " +
                    "VALUES ((SELECT book_id FROM bookdb.books WHERE title = @title LIMIT 1)," +
                    "(SELECT author_id FROM bookdb.authors WHERE author_name = @author));",  m_connection))
                {
                    insertAuthorBookLink.Parameters.AddWithValue("title", p_book.GetTitle());
                    insertAuthorBookLink.Parameters.AddWithValue("author", p_book.GetAuthor());
                    await insertAuthorBookLink.ExecuteNonQueryAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }

        public async Task<Book> GetBookIsbn(string p_isbn)
        {
            if (!await BookExistsIsbn(p_isbn))
            {
                throw new Exception("Book does not exist");
            }

            string title = "";
            string author = "";
            string publisher = "";
            
            await using (var book = new NpgsqlCommand(@"
                SELECT b.title, a.author_name, p.publisher_name
                FROM bookdb.books b
                JOIN bookdb.publishers p ON b.publisher_ID = p.publisher_ID
                JOIN bookdb.author_book_link abl ON b.book_ID = abl.book_ID
                JOIN bookdb.authors a ON abl.author_ID = a.author_ID
                WHERE b.ISBN = @isbn;", m_connection))
            {
                book.Parameters.AddWithValue("isbn", p_isbn);

                await using var reader = await book.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    title = reader.GetString(0);
                    author = reader.GetString(1);
                    publisher = reader.GetString(2);
                }
                
                return new Book(title, author, publisher, p_isbn);
            }
        }

        #region ExistsCheck

        public async Task<bool> BookExistsTitle(string p_bookTitle)
        {
            await using (var getBook = new NpgsqlCommand("SELECT COUNT(1) FROM bookdb.books WHERE title = @title;", m_connection))
            {
                getBook.Parameters.AddWithValue("title", p_bookTitle);
                var count = (long)await getBook.ExecuteScalarAsync();

                if (count > 0)
                {
                    return true;
                }
            }
            return false;
        }
        public async Task<bool> BookExistsIsbn(string p_bookIsbn)
        {
            await using (var getBook = new NpgsqlCommand("SELECT COUNT(1) FROM bookdb.books WHERE isbn = @isbn;", m_connection))
            {
                getBook.Parameters.AddWithValue("isbn", p_bookIsbn);
                var count = (long)await getBook.ExecuteScalarAsync();

                if (count > 0)
                {
                    return true;
                }
            }
            return false;
        }
        public async Task<bool> AuthorExists(string p_author)
        {
            await using (var getAuthor = new NpgsqlCommand("SELECT COUNT(1) FROM bookdb.authors WHERE author_name = @author;", m_connection))
            {
                getAuthor.Parameters.AddWithValue("author", p_author);
                var count = (long)await getAuthor.ExecuteScalarAsync();

                if (count > 0)
                {
                    return true;
                }
            }
            return false;
        }
        public async Task<bool> PublisherExists(string p_publisher)
        {
            await using (var getPublisher = new NpgsqlCommand("SELECT COUNT(1) FROM bookdb.publishers WHERE publisher_name = @publisher;", m_connection))
            {
                getPublisher.Parameters.AddWithValue("publisher", p_publisher);
                var count = (long)await getPublisher.ExecuteScalarAsync();

                if (count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}
