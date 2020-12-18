using Enyim.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheHelper
{
    public class MemcachedController : MemcachedClient
    {
        public MemcachedController _instance = new MemcachedController();
        public MemcachedController()
        {

        }
    }
}
