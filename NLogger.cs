using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStoreAPI
{
    public class NLogger
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
    }
}