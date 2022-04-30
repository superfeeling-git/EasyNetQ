using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EasyNetQ.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EasyController : ControllerBase
    {
        private readonly IBus bus;

        public EasyController(IBus bus)
        {
            this.bus = bus;
        }

        [HttpPost]
        public async Task<IActionResult> Get([FromQuery]MessageDto dto)
        {
            await bus.PubSub.PublishAsync<MessageDto>(dto);
            return Ok();
        }
    }
}
