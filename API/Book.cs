namespace BookDbApi
{
    public class Book
    {
        public enum Genre
        {
            Fiction,
            NonFiction,
            Science,
            History,
            Fantasy,
            Mystery,
            Romance,
            Thriller
        }
        private string m_title { get; set; } = string.Empty;
        private string m_author { get; set; } = string.Empty;
        private string m_publisher { get; set; } = string.Empty;
        private string m_isbn { get; set; } = string.Empty;
        private Genre m_genre { get; set; } = Genre.Fiction;

        public Book(string title, string author, string publisher, string isbn, Genre genre)
        {
            m_title = title;
            m_author = author;
            m_publisher = publisher;
            m_isbn = isbn;
            m_genre = genre;
        }

        public string GetTitle()
        {
            return m_title;
        }

        public string GetAuthor()
        {
            return m_author;
        }
        public string GetPublisher()
        {
            return m_publisher;
        }
        public string GetIsbn()
        {
            return m_isbn;
        }

        public Genre GetGenre()
        {
            return m_genre;
        }
    }
}
