using BookStoreAPI.Helper;
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
        private BookStoreEntities1 bookStore;
        public AuthorController()
        {
            bookStore = new BookStoreEntities1();
        }


        [HttpGet]
        [Route("author/getallauthors")]
        public IEnumerable<AuthorViewModel> GetAllAuthors()
        { 
            NLogger.logger.Info(Request.ToString());
            return bookStore.authors.Select(a => new AuthorViewModel() { author_id = a.author_id, first_name = a.first_name, middle_name = a.middle_name, last_name = a.last_name }).ToList();
        }

        [HttpPost]
        [Route("author/addauthor")]
        public HttpResponseMessage AddAuthor([FromBody]author author)
        {
            try
            {
                NLogger.logger.Info(Request.ToString());
                string token = Request.Headers.Contains("token") ? Request.Headers.GetValues("token").FirstOrDefault() : "";
                if (!TokenManager.ValidateToken(token))
                {
                    throw new Exception("User not Authorized");
                }
                bookStore.authors.Add(author);
                bookStore.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.Created, author);
            }

            catch (Exception ex)
            {
                NLogger.logger.Error(ex.ToString());
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.ToString());
            }
        }

        [HttpDelete]
        [Route("author/deleteauthor/{id}")]
        public HttpResponseMessage DeleteAuthor(int id)
        {
            try
            {
                NLogger.logger.Info(Request.ToString());
                string token = Request.Headers.Contains("token") ? Request.Headers.GetValues("token").FirstOrDefault() : "";
                if (!TokenManager.ValidateToken(token))
                {
                    throw new Exception("User not Authorized");
                }
                var entityauthor = bookStore.authors.FirstOrDefault(e => e.author_id == id);
                if (entityauthor == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Author Id " + id.ToString() + " not found");
                }

                else
                {
                    bookStore.authors.Remove(entityauthor);
                    bookStore.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }

            catch (Exception ex)
            {
                NLogger.logger.Error(ex.ToString());
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.ToString());
            }
        }


        [HttpPut]
        [Route("author/updateauthor/{id}")]
        public HttpResponseMessage UpdateAuthor(int id, [FromBody] author author)
        {
            try
            {
                
                NLogger.logger.Info(Request.ToString());
                string token = Request.Headers.Contains("token") ? Request.Headers.GetValues("token").FirstOrDefault() : "";
                if (!TokenManager.ValidateToken(token))
                {
                    throw new Exception("User not Authorized");
                }
                var entityauthor = bookStore.authors.FirstOrDefault(e => e.author_id == id);
                if (entityauthor == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Author Id " + id.ToString() + " not found");
                }

                else
                {
                    entityauthor.first_name = author.first_name;
                    entityauthor.middle_name = author.middle_name;
                    entityauthor.last_name = author.last_name;
                    bookStore.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, entityauthor);
                }
            }

            catch (Exception ex)
            {
                NLogger.logger.Error(ex.ToString());
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.ToString());
            }
        }

    }
}
