using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Turnero.Models;
using Turnero.Services.Interfaces;
using TurneroAPI.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TurneroAPI.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class TurnsAPIController : ControllerBase
    {

        public IInsertTurnsServices insertTurns;
        public IGetTurnsServices getTurns;
        public IUpdateTurnsServices updateTurns;
        public IGetMedicsServices getMedics;
        public IGetTimeTurnsServices getTimeTurns;
        public IExportService exportService;

        public TurnsAPIController(IInsertTurnsServices insertTurns, 
                        IGetTurnsServices getTurns, 
                        IUpdateTurnsServices updateTurns, 
                        IGetMedicsServices getMedics, 
                        IGetTimeTurnsServices getTimeTurns, 
                        IExportService exportService)
        {
            this.insertTurns = insertTurns;
            this.getTurns = getTurns;
            this.updateTurns = updateTurns;
            this.getMedics = getMedics;
            this.getTimeTurns = getTimeTurns;
            this.exportService = exportService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var turns = await this.getTurns.GetTurns(DateTime.Today, null);
            return Ok(turns.Select(x => new TurnDTO(x)).ToList());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var turn = new TurnDTO(await this.getTurns.GetTurn(id));
            if (turn != null)
                return Ok(turn);
            else
                return BadRequest();
        }

        [HttpGet("{TurnId}")]
        public async Task<IActionResult> Accessed(Guid TurnId)
        {
            var turn = await this.getTurns.GetTurn(TurnId);
            if (turn != null)
            {
                try
                {
                    updateTurns.Accessed(turn);
                    return Ok();
                }
                catch
                {
                    return BadRequest();
                }
            }
            else
                return BadRequest();
        }

        [HttpGet("{date}")]
        public async Task<IActionResult> Get(DateTime date)
        {
            var turns = await this.getTurns.GetTurns(date, null);
            return Ok(turns.Select(x => new TurnDTO(x)).ToList());
        }

        [HttpGet("{date}, {medicId}")]
        public async Task<IActionResult> Get(DateTime date, Guid medicId)
        {
            var turns = await this.getTurns.GetTurns(date, medicId);
            return Ok(turns.Select(x => new TurnDTO(x)).ToList());
        }

        [HttpGet("{medicId}")]
        public async Task<IActionResult> GetBymedic(Guid medicId)
        {
            var turns = await this.getTurns.GetTurns(DateTime.Today, medicId);
            return Ok(turns.Select(x => new TurnDTO(x)).ToList());
        }

        // POST api/<TurnsAPIController>
        [HttpPost]
        public IActionResult Post([FromBody] Turn turn)
        {
            if (!this.getTurns.Exists(turn.Id))
                return BadRequest();
            else
            {
                this.updateTurns.Update(turn);
                return Ok(new TurnDTO(turn));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] TurnAddDTO turnDto)
        {
            Turn turn = new Turn
            {
                Name = turnDto.Name,
                Accessed = false,
                DateTurn = turnDto.DateTurn,
                TimeId = turnDto.Time,
                Dni = turnDto.Dni,
                MedicId = turnDto.Medic,
                Reason = turnDto.Reason,
                SocialWork = turnDto.SocialWork
            };
            bool resInsert = await this.insertTurns.CreateTurnAsync(turn);
            if (resInsert)
                return Ok(new TurnDTO(turn));
            else
                return BadRequest();
        }

        // DELETE api/<TurnsAPIController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var turn = await this.getTurns.GetTurn(id);
            updateTurns.Delete(turn);
            return Ok();
        }
    }
}
