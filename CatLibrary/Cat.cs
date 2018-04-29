using System;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace CatLibrary
{
    public class Cat
    {
        private readonly ILogger<Cat> _logger;

        public Cat(ILogger<Cat> logger)
        {
            Type t = logger.GetType();
            var f = t.GetField("_logger", BindingFlags.NonPublic |
                                          BindingFlags.Instance);
            var fieldValue = f.GetValue(logger);
            Type ft = fieldValue.GetType();

            _logger = logger;
        }

        public string MakeSound()
        {
            _logger.LogInformation("Cat is going to meow now...");
            return "Meow!";
        }
    }
}
