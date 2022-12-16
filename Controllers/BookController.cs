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
        private BookStoreEntities1 bookStore;
        public BookController()
        {
            bookStore = new BookStoreEntities1();
        }
        
        
        [HttpGet]
        [Route("book/getallbooks")]
        public IEnumerable<book> GetAllBooks()
        {
            NLogger.logger.Info(Request.ToString());
            return bookStore.books.ToList();
        }



        [HttpPost]
        [Route("book/addbook")]
        public HttpResponseMessage AddBook([FromBody] book book)
        {
            try
            {
                NLogger.logger.Info(Request.ToString());
                string token = Request.Headers.Contains("token") ? Request.Headers.GetValues("token").FirstOrDefault() : "";
                if (!TokenManager.ValidateToken(token))
                {
                    throw new Exception("User not Authorized");
                }              

                bookStore.books.Add(book);
                bookStore.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.Created, book);                
            }

            catch (Exception ex)
            {
                NLogger.logger.Error(ex.ToString());
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.ToString());
            }
        }




        [HttpDelete]
        [Route("book/deletebook/{id}")]
        public HttpResponseMessage DeleteBook(int id)
        {
            try
            {
                NLogger.logger.Info(Request.ToString());
                string token = Request.Headers.Contains("token") ? Request.Headers.GetValues("token").FirstOrDefault() : "";
                if (!TokenManager.ValidateToken(token))
                {
                    throw new Exception("User not Authorized");
                }

                var entitybook = bookStore.books.FirstOrDefault(e => e.book_id == id);
                if (entitybook == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Book Id " + id.ToString() + " not found");
                }

                else
                {
                    bookStore.books.Remove(entitybook);
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
        [Route("book/updatebook/{id}")]
        public HttpResponseMessage UpdateBook(int id, [FromBody] book book)
        {
            try
            {
                NLogger.logger.Info(Request.ToString());
                string token = Request.Headers.Contains("token") ? Request.Headers.GetValues("token").FirstOrDefault() : "";
                if (!TokenManager.ValidateToken(token))
                {
                    throw new Exception("User not Authorized");
                }

                var entitybook = bookStore.books.FirstOrDefault(e => e.book_id == id);
                if (entitybook == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Book Id " + id.ToString() + " not found");
                }

                else
                {
                    entitybook.title = book.title;
                    entitybook.total_pages = book.total_pages;
                    entitybook.rating = book.rating;
                    entitybook.isbn = book.isbn;
                    entitybook.published_date = book.published_date;
                    entitybook.publisher_id = book.publisher_id;

                    bookStore.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, entitybook);
                }
            }

            catch (Exception ex)
            {
                NLogger.logger.Error(ex.ToString());
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.ToString());
            }
        }




        [HttpGet]
        [Route("books/{id}")]
        public HttpResponseMessage GetBookByID(int id)
        {
            try
            {
                NLogger.logger.Info(Request.ToString());
                var entitybook = bookStore.books.FirstOrDefault(e => e.book_id == id);
                if (entitybook != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entitybook);
                }

                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Book Id " + id.ToString() + " not found");
                }
            }

            catch (Exception ex)
            {
                NLogger.logger.Error(ex.ToString());
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.ToString());
            }
        }




        [HttpGet]
        [Route("booksbyauthor/{authorId}/books")]
        public HttpResponseMessage GetBooksByAuthor(int authorId)
        {
            try
            {
                NLogger.logger.Info(Request.ToString());                
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
                NLogger.logger.Error(ex.ToString());
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.ToString());
            }
        }





        [HttpGet]
        [Route("booksbygenre/{genreId}/books")]
        public HttpResponseMessage GetBooksByGenre(int genreId)
        {
            try
            {
                NLogger.logger.Info(Request.ToString());                
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
                NLogger.logger.Error(ex.ToString());
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.ToString());
            }
        }





        [HttpGet]
        [Route("booksbypublisher/{publisherId}/books")]
        public HttpResponseMessage GetBooksByPublisher(int publisherId)
        {
            try
            {
                NLogger.logger.Info(Request.ToString());
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
                NLogger.logger.Error(ex.ToString());
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.ToString());
            }
        }




        [HttpGet]
        [Route("latestbooksbygenre/{genreId}/books/latest")]
        public HttpResponseMessage GetLatestBooksByGenre(int genreId)
        {
            try
            {
                NLogger.logger.Info(Request.ToString());
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
                NLogger.logger.Error(ex.ToString());
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.ToString());
            }
        }





        [HttpPut]
        [Route("rateabook/rate/{Id}")]
        public HttpResponseMessage RateBookById(int id, [FromBody] book book)
        {
            try
            {                
                NLogger.logger.Info(Request.ToString());
                string token = Request.Headers.Contains("token") ? Request.Headers.GetValues("token").FirstOrDefault() : "";
                if (!TokenManager.ValidateToken(token))
                {
                    throw new Exception("User not Authorized");
                }
                var entitybook = bookStore.books.FirstOrDefault(e => e.book_id == id);

                if (entitybook == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Book Id " + id.ToString() + " not found");
                }

                else
                {
                    entitybook.rating = book.rating;
                    bookStore.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, entitybook);
                }
            }

            catch (Exception ex)
            {
                NLogger.logger.Error(ex.ToString());
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.ToString());
            }
        }





        [HttpGet]
        [Route("bookbyauthorgenre/{authorId}/{genreid}/books")]
        public HttpResponseMessage GetBooksByAuthorAndGenre(int authorId, int genreid)
        {
            try
            {
                NLogger.logger.Info(Request.ToString());
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
                NLogger.logger.Error(ex.ToString());
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.ToString());
            }
        }
    }
}
