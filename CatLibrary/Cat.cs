using Microsoft.Extensions.Logging;

namespace CatLibrary
{
    public class Cat
    {
        private readonly ILogger<Cat> _logger;

        public Cat(ILogger<Cat> logger)
        {
            _logger = logger;
        }

        public string MakeSound()
        {
            _logger.LogInformation("Cat is going to meow now...");
            return "Meow!";
        }
    }
}