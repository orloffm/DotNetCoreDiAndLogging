using System;
using Autofac;
using CatLibrary;
using NLog.Extensions.Logging;

namespace ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Autofac container.
            var builder = new ContainerBuilder();
            // Specifically create a single instance of a logger.
            var logger = new NLogLoggerFactory().CreateLogger("TEST");
            // The logger instance is auto-registered as implementation of ILogger.
            builder.RegisterInstance(logger);
            // The type Cat is added to container so that it would be able to provide instances of it.
            builder.RegisterType<Cat>();
            var container = builder.Build();

            // Entry point. This provides our logger instance to a Cat's constructor.
            var cat = container.Resolve<Cat>();

            // Run.
            var result = cat.MakeSound();
            Console.WriteLine(result);

            // Pause app before exiting.
            Console.Read();
        }
    }
}