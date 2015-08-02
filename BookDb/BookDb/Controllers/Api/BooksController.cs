using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BookDb.DAL;
using BookDb.Infrastructure.Filters;

namespace BookDb.Controllers.Api
{
    [ValidationHttp]
    [RoutePrefix("api/books")]
    public class BooksController : ApiController
    {
        private readonly IBookRepository _repository;

        public BooksController(IBookRepository repository)
        {
            _repository = repository;
        }

        [Route("")]
        public async Task<List<Book>> Get()
        {
            var authors = await _repository.GetByTitleAsync();
            return authors;
        }

        [Route("published")]
        public async Task<List<Book>> GetByPublishedDate()
        {
            var authors = await _repository.GetByPublishedDateAsync();
            return authors;
        }

        [Route("{id:int}")]
        public async Task<Book> Get(int id)
        {
            var book = await _repository.GetAsync(id);
            if (book == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return book;
        }

        [Route("")]
        public async Task<HttpResponseMessage> Post(Book model)
        {
            var book = await _repository.AddAsync(model);

            var response = Request.CreateResponse(HttpStatusCode.Created, book);
            return response;
        }

        [Route("{id:int}")]
        public async Task<HttpResponseMessage> Put(int id, Book model)
        {
            model.Id = id;
            var book = await _repository.UpdateAsync(model);

            var response = Request.CreateResponse(HttpStatusCode.OK, book);
            return response;
        }

        [Route("{id:int}")]
        public async Task<HttpResponseMessage> Delete(int id)
        {
            await _repository.DeleteAsync(id);

            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }
    }
}