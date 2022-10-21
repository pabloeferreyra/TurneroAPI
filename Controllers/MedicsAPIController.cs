using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Turnero.Services.Interfaces;
using Turnero.Models;
using TurneroAPI.DTO;

namespace TurneroAPI.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class MedicsAPIController : ControllerBase
    {
        private readonly IGetMedicsServices getMedicsServices;
        private readonly IInsertMedicServices insertMedicServices;
        private readonly IUpdateMedicServices updateMedicServices;
        public ILogger<AdministrationController> Logger { get; }
        public MedicsAPIController(
                               ILogger<AdministrationController> logger,
                               IGetMedicsServices getMedicsServices,
                               IInsertMedicServices insertMedicServices,
                               IUpdateMedicServices updateMedicServices)
        {
            Logger = logger;
            this.getMedicsServices = getMedicsServices;
            this.insertMedicServices = insertMedicServices;
            this.updateMedicServices = updateMedicServices;
        }

        // GET: api/<MedicsAPIController>
        [HttpGet]
        public async Task<IActionResult> GetMedics()
        {
            var medics = await getMedicsServices.GetMedics();
            return Ok(medics.Select(x => new MedicDTO(x)).ToList());
        }

        // GET api/<MedicsAPIController>/5
        [HttpGet("{id}")]

        public async Task<IActionResult> GetMedic(Guid id)
        {
            var medicDTO = new MedicDTO(await getMedicsServices.GetMedicById(id));
            if (medicDTO != null)
                return Ok(medicDTO);
            else
                return BadRequest();

        }

        // POST api/<MedicsAPIController>
        [HttpPost]
        public async Task<IActionResult> UpdateMedic([FromBody] Medic medic)
        {
            bool resUpd = await updateMedicServices.Update(medic);
            if (resUpd)
            {
                return await GetMedic(medic.Id);
            }
            else
            {
                return BadRequest();
            }
        }

        // PUT api/<MedicsAPIController>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Medic medic)
        {
            try
            {
                await insertMedicServices.Create(medic);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<MedicsAPIController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var medic = await getMedicsServices.GetMedicById(id);
            if (medic != null)
            {
                await updateMedicServices.Delete(medic);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
