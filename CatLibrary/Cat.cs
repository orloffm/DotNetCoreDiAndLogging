using System;
using Microsoft.Extensions.Logging;

namespace CatLibrary
{
    public class Cat
    {
        private readonly ILogger _logger;

        public Cat(ILogger logger)
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
