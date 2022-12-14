using BookStoreAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BookStoreAPI.Helper;

namespace BookStoreAPI.Controllers
{
    public class BookController : ApiController
    {
        BookStoreEntities1 bookStore;
        public BookController()
        {
            bookStore = new BookStoreEntities1();
        }
        [HttpGet]
        [Route("book/getallbooks")]
        public IEnumerable<book> GetAllBooks()
        {
            Program.logger.Info(Request.ToString());

            return bookStore.books.ToList();

        }



        [HttpPost]
        [Route("book/addbook")]
        public HttpResponseMessage AddBook([FromBody] book book)
        {
            try
            {
                string token = Request.Headers.Contains("token") ? Request.Headers.GetValues("token").FirstOrDefault() : "";

                if (!TokenManager.ValidateToken(token))
                {
                    throw new Exception("Invalid user");
                }

                Program.logger.Info(Request);

                bookStore.books.Add(book);
                bookStore.SaveChanges();
                var message = Request.CreateResponse(HttpStatusCode.Created, book);
                message.Headers.Location = new Uri(Request.RequestUri + book.book_id.ToString());
                return message;

            }

            catch (Exception ex)
            {
                Program.logger.Error(ex.Message);
                Program.logger.Error(ex.InnerException);

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }




        [HttpDelete]
        [Route("book/deletebook/{id}")]
        public HttpResponseMessage DeleteBook(int id)
        {
            try
            {
                Program.logger.Info(JsonConvert.SerializeObject(id));
                {
                    var entity = bookStore.books.FirstOrDefault(e => e.book_id == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Book Id " + id.ToString() + " not found");
                    }

                    else
                    {
                        bookStore.books.Remove(entity);
                        bookStore.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }

                }
            }

            catch (Exception ex)
            {
                Program.logger.Error(ex.Message);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }




        [HttpPut]
        [Route("book/updatebook/{id}")]
        public HttpResponseMessage UpdateBook(int id, [FromBody] book book)
        {
            try
            {
                string token = Request.Headers.GetValues("token").First();

                if (!TokenManager.ValidateToken(token))
                {
                    throw new Exception("Invalid user");
                }

                Program.logger.Info(Request.ToString());


                var entity = bookStore.books.FirstOrDefault(e => e.book_id == id);

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

                    bookStore.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
            }

            catch (Exception ex)
            {
                Program.logger.Error(ex.Message);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }




        [HttpGet]
        [Route("books/{id}")]
        public HttpResponseMessage GetBookByID(int id)
        {
            try
            {

                Program.logger.Info(Request);
                var entity = bookStore.books.FirstOrDefault(e => e.book_id == id);
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }

                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Book Id " + id.ToString() + " not found");
                }
            }

            catch (Exception ex)
            {
                Program.logger.Error(ex.Message);
                Program.logger.Error(JsonConvert.SerializeObject(ex));
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }




        [HttpGet]
        [Route("booksbyauthor/{authorId}/books")]
        public HttpResponseMessage GetBooksByAuthor(int authorId)
        {
            try
            {
                Program.logger.Info(Request.ToString());

                List<book> books = new List<book>();

                books = bookStore.books.Where(b => b.authors.Any(a => a.author_id.Equals(authorId))).ToList();

                if (books != null && books.Count != 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, books);
                }

                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Books with Author Id " + authorId.ToString() + " not found");
                }
            }

            catch (Exception ex)
            {
                Program.logger.Error(ex.Message);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }





        [HttpGet]
        [Route("booksbygenre/{genreId}/books")]
        public HttpResponseMessage GetBooksByGenre(int genreId)
        {
            try
            {
                Program.logger.Info(Request.ToString());

                List<book> books = new List<book>();

                books = bookStore.books.Where(b => b.genres.Any(a => a.genre_id.Equals(genreId))).ToList();

                if (books != null && books.Count != 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, books);
                }

                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Books with Genre Id " + genreId.ToString() + " not found");
                }
            }


            catch (Exception ex)
            {
                Program.logger.Error(ex.Message);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }





        [HttpGet]
        [Route("booksbypublisher/{publisherId}/books")]
        public HttpResponseMessage GetBooksByPublisher(int publisherId)
        {
            try
            {
                Program.logger.Info(Request.ToString());

                List<book> books = new List<book>();
                books = bookStore.books.Where(e => e.publisher_id == publisherId).ToList();

                if (books != null && books.Count != 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, books);
                }

                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Books with Publisher Id " + publisherId.ToString() + " not found");
                }
            }
            catch (Exception ex)
            {
                Program.logger.Error(ex.Message);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }




        [HttpGet]
        [Route("latestbooksbygenre/{genreId}/books/latest")]
        public HttpResponseMessage GetLatestBooksByGenre(int genreId)
        {
            try
            {
                Program.logger.Info(Request.ToString());

                List<book> books = new List<book>();

                books = bookStore.books.Where(b => b.genres.Any(a => a.genre_id.Equals(genreId))).OrderByDescending(b => b.published_date).Take(10).ToList();

                if (books != null && books.Count != 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, books);
                }

                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Books with Genre Id " + genreId.ToString() + " not found");
                }
            }

            catch (Exception ex)
            {
                Program.logger.Error(ex.Message);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }





        [HttpPut]
        [Route("rateabook/rate/{Id}")]
        public HttpResponseMessage RateBookById(int id, [FromBody] book book)
        {
            try
            {
                Program.logger.Info(JsonConvert.SerializeObject(book));
                var entity = bookStore.books.FirstOrDefault(e => e.book_id == id);

                if (entity == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Book Id " + id.ToString() + " not found");
                }

                else
                {
                    entity.rating = book.rating;
                    bookStore.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
            }

            catch (Exception ex)
            {
                Program.logger.Error(ex.Message);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }





        [HttpGet]
        [Route("bookbyauthorgenre/{authorId}/{genreid}/books")]
        public HttpResponseMessage GetBooksByAuthorAndGenre(int authorId, int genreid)
        {
            try
            {
                Program.logger.Info(Request.ToString());

                List<book> books = new List<book>();
                books = bookStore.books.Where(b => b.authors.Any(a => a.author_id.Equals(authorId)) && b.genres.Any(g => g.genre_id.Equals(genreid))).ToList();

                if (books != null && books.Count != 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, books);
                }

                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Books with Author Id " + authorId.ToString() + " and Genre Id " + genreid.ToString() + " not found");
                }
            }

            catch (Exception ex)
            {
                Program.logger.Error(ex.Message);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
