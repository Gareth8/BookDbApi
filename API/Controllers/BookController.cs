using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookDbApi.DataAccess;

namespace BookDbApi.Controllers
{
    public class BookController : BaseController
    {
        private readonly CRUD crud = new CRUD();

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Book API is running");
        }

        [HttpGet]
        [Route("{title}")]
        public async Task<IActionResult> BookExists(string title)
        {
            bool result = await crud.BookExists(title);

            if (result)
            {
                return Ok($"Book with title '{title}' exists.");
            }
            else
            {
                return NotFound($"Book with title '{title}' not found.");
            }
        }

        [HttpPost]
        [Route("{ISBN}")]
        public async Task<IActionResult> AddBook(string ISBN)
        {
            Book book = await OpenLibraryAccess.GetBook(ISBN);

            bool addSuccess = await crud.Create(book);

            if (addSuccess)
            {
                return Ok($"Book with ISBN '{ISBN}' created.");
            }
            
            return NotFound($"Book with ISBN '{ISBN}' not found.");
        }
    }
}
