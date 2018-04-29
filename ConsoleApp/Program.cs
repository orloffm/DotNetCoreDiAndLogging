using System;
using Autofac;
using CatLibrary;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Autofac container.
            var builder = new ContainerBuilder();

            // The type Cat is added to container so that the container
            // would be able to provide instances of it.
            builder.RegisterType<Cat>();

            // Create Logger<T> when ILogger<T> is required.
            builder.RegisterGeneric(typeof(Logger<>))
                .As(typeof(ILogger<>));

            // Use NLogLoggerFactory as a factory required by Logger<T>.
            builder.RegisterType<NLogLoggerFactory>()
                .AsImplementedInterfaces().InstancePerLifetimeScope();

            // Finish registrations and prepare the container that can resolve things.
            var container = builder.Build();

            // Entry point. This provides our logger instance to a Cat's constructor.
            var cat = container.Resolve<Cat>();

            // Run.
            var result = cat.MakeSound();
            Console.WriteLine(result);
        }
    }
}