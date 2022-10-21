using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TurneroAPI.DTO;
using Turnero.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TurneroAPI.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class TimesAPIController : ControllerBase
    {
        public IGetTimeTurnsServices getTimeTurns;

        public TimesAPIController(IGetTimeTurnsServices getTimeTurns)
        {
            this.getTimeTurns = getTimeTurns;
        }

        // GET: api/<TimesAPIController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var times = await this.getTimeTurns.GetTimeTurns();
            return Ok(times.Select(x => new TimeDTO(x)).ToList());
        }
    }
}
