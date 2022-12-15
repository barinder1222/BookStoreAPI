using BookStoreAPI.Helper;
using BookStoreAPI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BookStoreAPI.Controllers
{
    public class AuthenticateController : ApiController
    {
        [Route("Users/Authenticate")]
        [HttpPost]
        public CustomResponse UserLogin(LoginVM objVM)
        {
            try
            {

                //perform login
                if (objVM.UserName.ToLower() == "adminuser" && objVM.Password == "Password1234")
                {

                    var token = TokenManager.GenerateToken(objVM.UserName);
                    return new CustomResponse()
                    {
                        Token = token,
                        Status = "Success"
                    };
                }
                else
                {
                    throw new Exception("Invalid username or password");
                }
            }
            catch (Exception ex)
            {
                NLogger.logger.Error(ex.ToString());

                return new CustomResponse()
                {
                    Status = "Invalid",
                    ErrorMessage = ex.Message,

                };
            }
        }
    }
}
