namespace BookDbApi
{
    public class Book
    {
        private string m_title { get; set; }
        private string m_author { get; set; } 
        private string m_publisher { get; set; }
        private string m_isbn { get; set; }
        private List<string> m_genres { get; set; } = new List<string>();

        public Book(string title, string author, string publisher, string isbn)
        {
            m_title = title;
            m_author = author;
            m_publisher = publisher;
            m_isbn = isbn;
        }

        public Book(string title, string author, string publisher, string isbn, List<string> genres)
        {
            m_title = title;
            m_author = author;
            m_publisher = publisher;
            m_isbn = isbn;
            m_genres = genres;
        }

        public override string ToString()
        {
            if (m_genres.Any())
            {
                return $"{m_title} written by {m_author}, published by {m_publisher}. ISBN: {m_isbn}. Genres: {string.Join(", ", m_genres)}";
            }
            
            return $"{m_title} written by {m_author}, published by {m_publisher}. ISBN: {m_isbn}";
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
        public List<string> GetGenres()
        {
            return m_genres;
        }

        public void SetGenres(List<string> p_genres)
        {
            m_genres = p_genres;
        }
    }
}
