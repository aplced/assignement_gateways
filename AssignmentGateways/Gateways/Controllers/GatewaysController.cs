using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gateways.Models;

namespace Gateways.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GatewaysController : ControllerBase
    {
        private readonly GatewayContext _context;

        public GatewaysController(GatewayContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Gateway>>> GetGateways()
        {
            return await _context.Gateways.Include(g => g.Devices).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Gateway>> GetGateway(string id)
        {
            var gateway = await _context.Gateways.Include(g => g.Devices).FirstOrDefaultAsync(g => g.SerialNumber == id);

            if (gateway == null)
            {
                return NotFound();
            }

            return gateway;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutGateway(string id, Gateway gateway)
        {
            if (id != gateway.SerialNumber)
            {
                return BadRequest();
            }

            _context.Entry(gateway).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GatewayExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Gateway>> PostGateway(Gateway gateway)
        {
            try
            {
                _context.Gateways.Add(gateway);
                await _context.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                if (GatewayExists(gateway.SerialNumber) || ex.GetType() == typeof(System.InvalidOperationException))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetGateway", new { id = gateway.SerialNumber }, gateway);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Gateway>> DeleteGateway(string id)
        {
            var gateway = await _context.Gateways.FindAsync(id);
            if (gateway == null)
            {
                return NotFound();
            }

            _context.Gateways.Remove(gateway);
            await _context.SaveChangesAsync();

            return gateway;
        }

        [HttpPost("{id}/device")]
        public async Task<ActionResult<Gateway>> PostGatewayDevice(string id, Device device)
        {
            var gateway = await _context.Gateways.Include(g => g.Devices).FirstOrDefaultAsync(g => g.SerialNumber == id);

            if (gateway == null)
            {
                return NotFound();
            }

            if(gateway.Devices.Count > 9)
            {
                return Conflict();
            }

            if(gateway.Devices.FirstOrDefault(d => d.UUID == device.UUID) != null)
            {
                return Conflict();
            }

            _context.Add(device);
            gateway.Devices.Add(device);
            _context.Entry(gateway).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGateway", new { id = gateway.SerialNumber }, gateway);
        }

        [HttpDelete("{id}/device/{uuid}")]
        public async Task<ActionResult<Gateway>> DeleteGatewayDevice(string id, long uuid)
        {
            var gateway = await _context.Gateways.Include(g => g.Devices).FirstOrDefaultAsync(g => g.SerialNumber == id);
            if (gateway == null)
            {
                return NotFound();
            }

            var device = gateway.Devices.FirstOrDefault(d => d.UUID == uuid);
            if(device == null)
            {
                return NotFound();
            }
            gateway.Devices.Remove(device);
            await _context.SaveChangesAsync();

            return gateway;
        }

        private bool GatewayExists(string id)
        {
            return _context.Gateways.Any(e => e.SerialNumber == id);
        }
    }
}
