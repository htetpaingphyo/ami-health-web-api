using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Ami.Health.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ami.Health.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("Policies")]
    public class PoliciesController : Controller
    {
        private HttpClient client;
        private string url = "http://172.20.122.201:7001";
        //private string url = "http://172.20.188.202:7001";

        public PoliciesController()
        {
            try
            {
                client = new HttpClient();
                client.BaseAddress = new System.Uri(url);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            #region TEST_CODE...
            //var testPolicy = "AMI/YGN/LHI/18000004";
            //var base64encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(testPolicy));
            //var message = await client.GetAsync($"/policies/{base64encoded}");
            //message.EnsureSuccessStatusCode();

            //if (!message.IsSuccessStatusCode)
            //    throw new Exception(message.StatusCode.ToString());

            //var policy = await message.Content.ReadAsAsync<Policy>();
            //return policy;
            #endregion

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<Policy> Get(string id /*** id should be Base64Encoded string. ***/)
        {
            var message = await client.GetAsync($"/policies/{id}");
            message.EnsureSuccessStatusCode();

            if (!message.IsSuccessStatusCode)
                throw new Exception(message.StatusCode.ToString());

            var policy = await message.Content.ReadAsAsync<Policy>();
            return policy;
        }
    }
}