# BookDbApi
![.NET Workflow](https://github.com/Gareth8/BookDbApi/actions/workflows/dotnet.yml/badge.svg)

This API interacts with a database for storing books. It uses the [Open Library's API](https://openlibrary.org/developers/api) to retrieve data about a book from its ISBN number, which is then stored within a PostgreSQL database.

---

The API can be queried with the following URLs:
-  `/api/Book/BookExists/{title}` - This URL takes in a string, and if a book exists in the database with a matching title, the API will respond with a code of 200.
- `/api/Book/AddBook/{ISBN}` - This URL takes in an ISBN number as a string, and attempts to add the associated book to the database. The API accepts a valid ISBN 10 or 13 number, and if the book is found, it will be added to the database and will respond with a code of 200. If the book is not found, or an invalid string is passed, the API will instead respond with a code of 404.

---

A .env file is required, placed inside the API folder, to set the below environment variables:
- **DATABASE_HOST={hostname}** - The host name that PostgreSQL is running on.  
- **DATABASE_USERNAME={username}**  - The username to connect with. 
- **DATABASE_PASSWORD={password}** - The password to connect with.
- **DATABASE_TARGET={database}** - The PostgreSQL database to connect to.
