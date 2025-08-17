# BookDbApi
![.NET Workflow](https://github.com/Gareth8/BookDbApi/actions/workflows/dotnet.yml/badge.svg)

This API interacts with a database for storing books. It uses the [Open Library's API](https://openlibrary.org/developers/api) to retrieve data about a book from its ISBN number, which is then stored within a PostgreSQL database.
The API will accept either an ISBN 10 or ISBN 13 number for its functionality.

---

The API can be queried with the following URLs:
-  `/api/title/BookExists/{title}` - **GET** - This URL takes in a string, and if a book exists in the database with a matching title, the API will respond with a code of 200.
- `/api/isbn/AddBook/{ISBN}` - **POST** - This URL takes in an ISBN number as a string, and attempts to add the associated book to the database. If the book is found, it will be added to the database and will respond with a code of 200. If the book is not found, or an invalid string is passed, the API will instead respond with a code of 404.
- `/api/isbn/GetBook?isbn={ISBN}` - **GET** - This URL takes in an ISBN from query, and attempts to find the associated book within the database. If the value from query is empty, the API will respond with a code of 422 (Unprocessable Entity) and a message informing the client of the required format.
  If the passed ISBN is associated with a book in the database, then the API will respond with a code of 200 and the stored information about the book. If the ISBN does not have an associated book, then a 400 response is returned with the message "Book does not exist".

---

A .env file is required, placed inside the API folder, to set the below environment variables:
- **DATABASE_HOST={hostname}** - The host name that PostgreSQL is running on.  
- **DATABASE_USERNAME={username}**  - The username to connect with. 
- **DATABASE_PASSWORD={password}** - The password to connect with.
- **DATABASE_TARGET={database}** - The PostgreSQL database to connect to.
