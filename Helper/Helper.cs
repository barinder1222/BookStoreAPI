using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BookStoreAPI.Helper
{
    public static class Helper
    {
        public async static Task<string> GetBodyContentAsStringAsync(this HttpRequestBase request)
        {
            string content = string.Empty;

            using (Stream receiveStream = request.InputStream)
            using (StreamReader readStream = new StreamReader(receiveStream, request.ContentEncoding))
            { content = await readStream.ReadToEndAsync(); }

            return content;
        }
    }
}