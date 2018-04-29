using CatLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreWebApplication.Controllers
{
    [Route("api/[controller]")]
    public class AnimalController : Controller
    {
        private readonly Cat _cat;
        private readonly ILogger<AnimalController> _logger;

        public AnimalController(Cat cat, ILogger<AnimalController> logger)
        {
            _cat = cat;
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            var result = _cat.MakeSound();
            _logger.LogInformation("Get was called on Values controller.");
            return result;
        }
    }
}