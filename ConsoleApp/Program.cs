using System;
using Autofac;
using AutofacTools;
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
            builder.RegisterModule<NLoggerModule>();
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