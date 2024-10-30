using Microsoft.AspNetCore.Mvc;
using Racen.Backend.App.Services;

namespace Racen.Backend.App.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MotorcycleController : ControllerBase
    {
        private readonly MotorcycleService _motorcycleService;

        public MotorcycleController(MotorcycleService motorcycleService)
        {
            _motorcycleService = motorcycleService;
        }
    }
}