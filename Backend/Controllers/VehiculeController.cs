using System.Collections.Concurrent;
using System.Threading;
using Backend.DataTest;
using Backend.Models.entities;
using Backend.Models.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiculeController : ControllerBase
    {
        private readonly List<Vehicule> _vehicules;
        private readonly VehiculeServices _vehiculeServices;
        private readonly ILogger<VehiculeController> _logger;

        public VehiculeController(ILogger<VehiculeController> logger)
        {
            _vehicules = VehiculeTestData._vehicules;
            _vehiculeServices = new VehiculeServices();
            _logger = logger;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Vehicule>>> GetAllVehicule()
        {
            await Task.Run(() =>
            {
                foreach (var v in _vehicules)
                {
                    _vehiculeServices.CheckAndLogCoordinate(v.Coordinate);
                }
            });

            return Ok(_vehicules);
        }

        [HttpGet("{id}")]
        public ActionResult<Vehicule> GetVehicule(int id)
        {
            var vehicule = _vehicules.FirstOrDefault(v => v.Id == id);
            if (vehicule == null)
            {
                return NotFound();
            }

            return Ok(vehicule);
        }

        [HttpGet("coordinates")]
        public ActionResult<IEnumerable<object>> GetAllCoordinates()
        {
            var coordinates = _vehicules.Select(v => new
            {
                v.Id,
                v.Immatricule,
                v.Coordinate.Latitude,
                v.Coordinate.Longitude,
                v.Coordinate.Timestamp
            });

            return Ok(coordinates);
        }
    }
}
