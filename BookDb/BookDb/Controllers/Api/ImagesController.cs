using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using BookDb.Constants;
using BookDb.Infrastructure.Threading;

namespace BookDb.Controllers.Api
{
    [RoutePrefix("api/images")]
    public class ImagesController : ApiController
    {
        private const string ImagesPath = "~/Images";
        private const int MaxFileLength = 1024*1024*30; // 30 Mb

        private readonly List<string> _contentTypes = new List<string>
        {
            "image/jpeg",
            "image/jpg",
            "image/png",
            "image/gif"
        };

        [Route("")]
        public async Task<HttpResponseMessage> Post()
        {
            string tempFilePath = null;
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                var provider = new MultipartFormDataStreamProvider(Path.GetTempPath());
                provider = await Request.Content.ReadAsMultipartAsync(provider);

                var fileContent = provider.Contents.First();
                var contentType = fileContent.Headers.ContentType.MediaType.ToLowerInvariant();
                if (!_contentTypes.Contains(contentType))
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                // checking file length
                tempFilePath = provider.FileData.First().LocalFileName;
                var fileLength = new FileInfo(tempFilePath).Length;
                if (fileLength > MaxFileLength)
                {
                    throw new HttpResponseException(HttpStatusCode.RequestEntityTooLarge);
                }

                // copy to local folder
                var filename = Guid.NewGuid() +
                               Path.GetExtension(fileContent.Headers.ContentDisposition.FileName.Trim('\"'));
                var destFile = Path.Combine(HttpContext.Current.Server.MapPath(ImagesPath), filename);
                File.Copy(tempFilePath, destFile);

                var response = Request.CreateResponse(HttpStatusCode.Created);
                var uri = Url.Link(RouteNames.ImagesApi, new {filename});
                response.Headers.Location = new Uri(uri);

                return response;
            }
            finally
            {
                // delete temp file in background
                if (!string.IsNullOrEmpty(tempFilePath))
                {
                    Task.Run(() => File.Delete(tempFilePath)).NoWarning();
                }
            }
        }

        [Route("{*filename}", Name = RouteNames.ImagesApi)]
        public HttpResponseMessage Get(string filename)
        {
            var file = Path.Combine(HttpContext.Current.Server.MapPath(ImagesPath), filename);
            if (!File.Exists(file))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var contentType = MimeMapping.GetMimeMapping(Path.GetExtension(file));

            var result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(file, FileMode.Open);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            return result;
        }
    }
}