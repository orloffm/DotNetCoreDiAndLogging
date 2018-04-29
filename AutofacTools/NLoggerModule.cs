using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Module = Autofac.Module;

namespace AutofacTools
{
    public class NLoggerModule : Module
    {
        /// <summary>
        ///     The factory provided by NLog.Extensions.Logging
        ///     that creates instances of <see cref="ILogger" /> taking a string.
        /// </summary>
        private readonly NLogLoggerProvider _factory;

        public NLoggerModule()
        {
            _factory = new NLogLoggerProvider();
        }

        /// <summary>
        ///     Attaches a callback to the process of preparing for resolving items.
        /// </summary>
        protected override void AttachToComponentRegistration(
            IComponentRegistry componentRegistry,
            IComponentRegistration registration)
        {
            registration.Preparing += Registration_Preparing;
        }

        /// <summary>
        ///     Called when the module loads. As <see cref="CreateLogger" />
        ///     returns an <see cref="ILogger" />, we are registering
        ///     this function as the creator if loggers. That is,
        ///     if someone explicitly requests a logger, <see cref="CreateLogger" />
        ///     will serve that request.
        /// </summary>
        /// <param name="builder">Autofac's builder of types.</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(CreateLogger).AsImplementedInterfaces();
        }

        /// <summary>
        ///     Creates an instance of <see cref="ILogger" />.
        /// </summary>
        /// <param name="_">Not used.</param>
        /// <param name="parameters">
        ///     Parameters. Expects a Type parameter that specifies
        ///     the type for which the logger is resolved.
        /// </param>
        private ILogger CreateLogger(IComponentContext _, IEnumerable<Parameter> parameters)
        {
            // Type of the class this logger is created for.
            var parentType = parameters.TypedAs<Type>();

            // Create a logger instance that takes the full name of the type it will be used in.
            var logger = _factory.CreateLogger(parentType.FullName);
            return logger;
        }

        /// <summary>
        ///     The callback that is called when preparing to resolve some object.
        /// </summary>
        private void Registration_Preparing(object sender, PreparingEventArgs args)
        {
            // What object type is going to be created?
            var forType = args.Component.Activator.LimitType;

            // Does the type's constructor has an ILogger in its arguments?
            bool IsLoggerPresentAsArgument(ParameterInfo p, IComponentContext c)
            {
                return p.ParameterType == typeof(ILogger);
            }

            // Resolves a logger to be used as that argument.
            object ResolveLoggerForType(ParameterInfo p, IComponentContext c)
            {
                // Provide the type that is being resolved as an argument.
                return c.Resolve<ILogger>(TypedParameter.From(forType));
            }

            // Add a logger parameter. It will be checked, and if an ILogger
            // is an argument in the created class, it will create an instance of it.
            var loggerParameter = new ResolvedParameter(
                IsLoggerPresentAsArgument,
                ResolveLoggerForType);
            args.Parameters = args.Parameters.Union(new[] {loggerParameter});
        }
    }
}