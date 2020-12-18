using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Utilities;

namespace MvcProject.Controllers
{
    public class ThumbController : ApiController
    {
        // GET api/<controller>   : url to use => api/thumb
        public IHttpActionResult Get(int w, int h, string path)
        {
            //int w = 200;
            //int h = 200;
            //    string path = "/Upload/Product/2019_2_16/system2e30ede4-dce4-45a6-8b40-6a30773e9215.jpg";
            var filePath = System.Web.Hosting.HostingEnvironment.MapPath(path);
            MemoryStream mem = ImageBusinnes.Crop(Image.FromFile(filePath), w, h, ImageBusinnes.AnchorPosition.Center);
            return new ImageResult(mem, Request);
        }
        public IHttpActionResult Get(int w, string path)
        {
            //int w = 200;
            //    string path = "/Upload/Product/2019_2_16/system2e30ede4-dce4-45a6-8b40-6a30773e9215.jpg";
            var filePath = System.Web.Hosting.HostingEnvironment.MapPath(path);
            MemoryStream mem = ImageBusinnes.ConstrainProportions(Image.FromFile(filePath), w, ImageBusinnes.Dimensions.Width);
            return new ImageResult(mem, Request);
        }
        public class ImageResult : IHttpActionResult
        {
            MemoryStream memory;
            HttpRequestMessage httpRequestMessage;
            HttpResponseMessage httpResponseMessage;
            public ImageResult(MemoryStream _memory, HttpRequestMessage request)
            {
                memory = _memory;
                httpRequestMessage = request;
            }
            public System.Threading.Tasks.Task<HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
            {
                httpResponseMessage = httpRequestMessage.CreateResponse(HttpStatusCode.OK);
                httpResponseMessage.Content = new ByteArrayContent(memory.ToArray());
                httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

                return System.Threading.Tasks.Task.FromResult(httpResponseMessage);
            }
        }
    }
}