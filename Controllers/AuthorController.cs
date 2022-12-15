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
        public IEnumerable<author> GetAllAuthors()
        { 
            Program.logger.Info(Request.ToString());
            return bookStore.authors.ToList(); 
        }

        [HttpPost]
        [Route("author/addauthor")]
        public HttpResponseMessage AddAuthor([FromBody]author author)
        {
            try
            {
                Program.logger.Info(Request.ToString());
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
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpDelete]
        [Route("author/deleteauthor/{id}")]
        public HttpResponseMessage DeleteAuthor(int id)
        {
            try
            {
                Program.logger.Info(Request.ToString());
                string token = Request.Headers.Contains("token") ? Request.Headers.GetValues("token").FirstOrDefault() : "";
                if (!TokenManager.ValidateToken(token))
                {
                    throw new Exception("User not Authorized");
                }
                var entity = bookStore.authors.FirstOrDefault(e => e.author_id == id);
                if (entity == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Author Id " + id.ToString() + " not found");
                }

                else
                {
                    bookStore.authors.Remove(entity);
                    bookStore.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        [HttpPut]
        [Route("author/updateauthor/{id}")]
        public HttpResponseMessage UpdateAuthor(int id, [FromBody] author author)
        {
            try
            {
                Program.logger.Info(Request.ToString());
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
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

    }
}
