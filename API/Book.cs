namespace BookDbApi
{
    public class Book
    {
        private string m_title { get; set; }
        private string m_author { get; set; } 
        private string m_publisher { get; set; }
        private string m_isbn { get; set; }

        public Book(string title, string author, string publisher, string isbn)
        {
            m_title = title;
            m_author = author;
            m_publisher = publisher;
            m_isbn = isbn;
        }

        public override string ToString()
        {
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

        
        /*
         * Genere needs to be added.
         * The main issue comes from one book having multiple genres, so need to figure out a good way to deal with that.
         */
        public string GetGenre()
        {
            return "";
        }
    }
}
