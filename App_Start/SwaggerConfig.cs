using System.Web.Http;
using WebActivatorEx;
using BookStoreAPI;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace BookStoreAPI
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "BookStoreAPI");                        
                    })
                .EnableSwaggerUi(c =>
                    {
                        
                    });
        }
    }
}
