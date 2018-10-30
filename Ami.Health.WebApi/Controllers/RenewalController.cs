using Ami.Health.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Ami.Health.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RenewalController : ControllerBase
    {
        private HttpClient client;
        private string url = "http://172.20.122.201:7001";
        //private string url = "http://172.20.188.202:7001";

        public RenewalController()
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
            return Ok();
        }

        [HttpPost]
        public async Task<IEnumerable<PolicyRenewal>> GetRenewal([FromBody] RenewalReq renewal)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Model state is not valid");
            }

            try
            {
                var message = await client.PostAsJsonAsync($"/renewal/", renewal);
                message.EnsureSuccessStatusCode();

                if (!message.IsSuccessStatusCode)
                {
                    throw new Exception(message.StatusCode.ToString());
                }

                var policies = await message.Content.ReadAsAsync<IEnumerable<PolicyRenewal>>();
                return policies;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}