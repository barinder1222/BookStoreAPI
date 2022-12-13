using BookStoreAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BookStoreAPI.Controllers
{
    public class BookController : ApiController
    {
        [HttpGet]
        public IEnumerable<book> GetAllBooks()
        {
            using (BookStoreEntities1 entities1 = new BookStoreEntities1())
            {
                return entities1.books.Take(20).ToList();
            }

        }

        [HttpGet]
        public HttpResponseMessage GetBookByID(int id)
        {
            Program.logger.Info("test request");
            Program.logger.Error("test error");
            using (BookStoreEntities1 entities1 = new BookStoreEntities1())
            {
                var entity = entities1.books.FirstOrDefault(e => e.book_id == id);
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }

                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Book Id " + id.ToString() + " not found");
                }
            }

        }

        [HttpPost]
        public HttpResponseMessage AddBook([FromBody] book book)
        {
            try
            {
                Program.logger.Info(JsonConvert.SerializeObject(book));

                using (BookStoreEntities1 entities1 = new BookStoreEntities1())
                {
                    entities1.books.Add(book);
                    entities1.SaveChanges();
                    var message = Request.CreateResponse(HttpStatusCode.Created, book);
                    message.Headers.Location = new Uri(Request.RequestUri + book.book_id.ToString());
                    return message;
                }
            }

            catch (Exception ex)
            {
                Program.logger.Error(ex.Message);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }


        }

        [HttpDelete]
        public HttpResponseMessage DeleteBook(int id)
        {
            try
            {
                using (BookStoreEntities1 entities1 = new BookStoreEntities1())
                {
                    var entity = entities1.books.FirstOrDefault(e => e.book_id == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Book Id " + id.ToString() + " not found");
                    }

                    else
                    {
                        entities1.books.Remove(entity);
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
        public HttpResponseMessage UpdateBook(int id, [FromBody] book book)
        {
            try
            {
                if (ModelState.IsValid)
                {

                }
                using (BookStoreEntities1 entities1 = new BookStoreEntities1())
                {
                    var entity = entities1.books.FirstOrDefault(e => e.book_id == id);

                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Book Id " + id.ToString() + " not found");
                    }

                    else
                    {
                        entity.title = book.title;
                        entity.total_pages = book.total_pages;
                        entity.rating = book.rating;
                        entity.isbn = book.isbn;
                        entity.published_date = book.published_date;
                        entity.publisher_id = book.publisher_id;

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
