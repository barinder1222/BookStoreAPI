using BookStoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BookStoreAPI.Controllers
{
    public class BookV2Controller : ApiController
    {
        [HttpGet]
        [Route("latestbooksbygenre/V2/{genreId}/books/latest")]
        public HttpResponseMessage GetLatestBooksByGenre(int genreId)
        {
            try
            {
                using (BookStoreEntities1 entities1 = new BookStoreEntities1())
                {
                    List<book> books = new List<book>();

                    books = entities1.books.Where(b => b.genres.Any(a => a.genre_id.Equals(genreId))).OrderByDescending(b => b.published_date).Take(20).ToList();

                    if (books != null && books.Count != 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, books);
                    }

                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Books with Genre Id " + genreId.ToString() + " not found");
                    }
                }
            }

            catch (Exception ex)
            {
                NLogger.logger.Error(ex.Message);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
