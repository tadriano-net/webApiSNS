using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PushNotification.Models;

namespace PushNotification.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public string Post(Receive receive)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\tadriano\Downloads\teste\WriteLines2.txt", true))
            {
                file.WriteLine(receive.Token + "aul");
            }
            return receive.Token + "sd";
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }

   
}