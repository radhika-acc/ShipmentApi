using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShipmentApi.Services;

namespace ShipmentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentsController : ControllerBase
    {
        private readonly ShipmentService _shipmentService;

        public ShipmentsController(ShipmentService shipmentService)
        {
            _shipmentService = shipmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetShipmentStatuses([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate == default || endDate == default)
            {
                return BadRequest("Invalid date range.");
            }

            var statuses = await _shipmentService.GetShipmentStatuses(startDate, endDate);

            return Ok(statuses);
        }
    }
}
