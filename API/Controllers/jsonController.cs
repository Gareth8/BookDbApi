using Microsoft.AspNetCore.Mvc;
using BookDbApi.DataAccess;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace BookDbApi.Controllers
{
    public class jsonController : BaseController
    {
        private readonly CRUD crud = new CRUD();

        [HttpPost]
        [ActionName("AddBook")]
        public async Task<IActionResult> AddBookJson()
        {
            var reader = new StreamReader(Request.Body);
            string body = await reader.ReadToEndAsync();

            if (body is "" or "{}")
            {
                return Problem(
                    title: "Empty JSON provided.",
                    detail: "Empty JSON. Please fill the JSON with a book's details.",
                    statusCode: StatusCodes.Status422UnprocessableEntity,
                    instance: HttpContext.Request.Path
                );
            }
            
            JsonWrapper? json = JsonSerializer.Deserialize<JsonWrapper>(body);

            if (json == null)
            {
                return BadRequest();
            }

            if (await crud.BookExistsIsbn(json.isbn))
            {
                return Problem(
                    title: "Book already exists",
                    detail: "A book with the same ISBN already exists.",
                    statusCode: StatusCodes.Status409Conflict,
                    instance: HttpContext.Request.Path
                );
            }

            if (json.genres.Length != 0)
            {
                await crud.Create(new Book(json.title, json.author, json.publisher, json.isbn, json.genres.ToList()));
            }
            else
            {
                await crud.Create(new Book(json.title, json.author, json.publisher, json.isbn));
            }
            return Ok();
        }
    }

    public class JsonWrapper
    {
        public string title { get; set; } = "";
        public string author { get; set; } = "";
        public string publisher { get; set; } = "";
        public string isbn { get; set; } = "";
        public string[] genres { get; set; } = [];

        public override string ToString()
        {
            return $"{title} {author} {publisher} {isbn}";
        }
    }
}