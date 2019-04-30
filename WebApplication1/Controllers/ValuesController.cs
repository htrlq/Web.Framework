using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Distriobuted.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpPost("Import")]
        public async Task<TestModel> ImportAsync(TestModel model, [FromServices] IDistributedConfigruation distributed)
        {
            var response = await distributed.TryImportAsync("Test", model);

            if (response)
                return model;

            throw new Exception("Fail");
        }

        [HttpDelete("Delete")]
        public async Task<bool> DeleteAsync([FromForm]string key, [FromServices] IDistributedConfigruation distributed)
        {
            var response = await distributed.TryDeleteAsync(key);

            return response;
        }

        [HttpGet("Get")]
        public TestModel Get([FromServices] IOptions<TestModel> options)
        {
            return options.Value;
        }
    }

    public class TestModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
