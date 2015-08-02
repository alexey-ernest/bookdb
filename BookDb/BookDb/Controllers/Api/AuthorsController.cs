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
    [RoutePrefix("api/authors")]
    public class AuthorsController : ApiController
    {
        private readonly IAuthorRepository _repository;

        public AuthorsController(IAuthorRepository repository)
        {
            _repository = repository;
        }

        [Route("")]
        public async Task<List<Author>> Get()
        {
            var authors = await _repository.GetByNameAsync();
            return authors;
        }

        [Route("{id:int}")]
        public async Task<Author> Get(int id)
        {
            var author = await _repository.GetAsync(id);
            if (author == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return author;
        }

        [Route("")]
        public async Task<HttpResponseMessage> Post(Author model)
        {
            var author = await _repository.AddAsync(model);

            var response = Request.CreateResponse(HttpStatusCode.Created, author);
            return response;
        }

        [Route("{id:int}")]
        public async Task<HttpResponseMessage> Put(int id, Author model)
        {
            model.Id = id;
            var author = await _repository.UpdateAsync(model);

            var response = Request.CreateResponse(HttpStatusCode.OK, author);
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