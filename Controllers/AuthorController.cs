using BookStoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BookStoreAPI.Controllers
{
    public class AuthorController : ApiController
    {
        [HttpGet]
        //[Route("GetAllBooks")]
        public IEnumerable<author> GetAllBooks()
        {
            using (BookStoreEntities1 entities1 = new BookStoreEntities1())
            {
                return entities1.authors.ToList();
            }

        }

        [HttpGet]
        public HttpResponseMessage GetAuthorByID(int id)
        {
            using (BookStoreEntities1 entities1 = new BookStoreEntities1())
            {
                var entity = entities1.authors.FirstOrDefault(e => e.author_id == id);
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }

                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Author Id " + id.ToString() + " not found");
                }
            }

        }

        [HttpPost]
        public HttpResponseMessage AddAuthor([FromBody]author author)
        {
            try
            {
                using (BookStoreEntities1 entities1 = new BookStoreEntities1())
                {
                    entities1.authors.Add(author);
                    entities1.SaveChanges();
                    var message = Request.CreateResponse(HttpStatusCode.Created, author);
                    message.Headers.Location = new Uri(Request.RequestUri + author.author_id.ToString());
                    return message;
                }
            }

            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpDelete]
        public HttpResponseMessage DeleteAuthor(int id)
        {
            try
            {
                using (BookStoreEntities1 entities1 = new BookStoreEntities1())
                {
                    var entity = entities1.authors.FirstOrDefault(e => e.author_id == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Author Id " + id.ToString() + " not found");
                    }

                    else
                    {
                        entities1.authors.Remove(entity);
                        entities1.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }

            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        [HttpPut]
        public HttpResponseMessage UpdateAuthor(int id, [FromBody] author author)
        {
            try
            {
                using (BookStoreEntities1 entities1 = new BookStoreEntities1())
                {
                    var entity = entities1.authors.FirstOrDefault(e => e.author_id == id);

                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Author Id " + id.ToString() + " not found");
                    }

                    else
                    {
                        entity.first_name = author.first_name;
                        entity.middle_name = author.middle_name;
                        entity.last_name = author.last_name;

                        entities1.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }
            }

            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

    }
}
