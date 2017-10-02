using PeterSkerrittELearning.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PeterSkerrittELearning.Controllers
{
    public class MediaStreamingController : ApiController
    {
        public HttpResponseMessage GetMediaContent()
        {
            var httpResponse = Request.CreateResponse();

            httpResponse.Content = new PushStreamContent((Action<Stream, HttpContent, TransportContext>) WriteMediaToStream);
            return httpResponse;
        }

        public async void WriteMediaToStream(Stream outputStream, HttpContent media, TransportContext transportContext)
        {
            string fileName = null;

            int Id = 0;
            
            using (ApplicationDbContext data = new ApplicationDbContext())
            {
                fileName = data.Material.FirstOrDefault(f => f.Id == Id).Name;
            }

            var filePath = System.Web.HttpContext.Current.Server.MapPath("~/Media/" + fileName);

            int bufferSize = 1000;
            byte[] buffer = new byte[bufferSize];

            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                int totalSize = (int)fileStream.Length;

                while(totalSize > 0)
                {
                    int count = totalSize > bufferSize ? bufferSize : totalSize;

                    int readBufferSize = fileStream.Read(buffer, 0, count);

                    await outputStream.WriteAsync(buffer, 0, readBufferSize);

                    totalSize -= readBufferSize;
                }
            }

        }
        
        
        
        
        
        
        
        
        //// GET api/<controller>
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<controller>
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}
    }
}