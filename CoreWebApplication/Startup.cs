using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutofacTools;
using CatLibrary;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreWebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddControllersAsServices()
                ;

            ContainerBuilder builder = new ContainerBuilder();
            builder.Populate(services);
          //  builder.RegisterModule<NLoggerModule>();
            builder.RegisterType<Cat>();
            IContainer container = builder.Build();

            return new AutofacServiceProvider2(container);
        }
    }

    public class AutofacServiceProvider2 : IServiceProvider, ISupportRequiredService
    {
        private readonly IComponentContext _componentContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Autofac.Extensions.DependencyInjection.AutofacServiceProvider" /> class.
        /// </summary>
        /// <param name="componentContext">
        /// The component context from which services should be resolved.
        /// </param>
        public AutofacServiceProvider2(IComponentContext componentContext)
        {
            this._componentContext = componentContext;
        }

        /// <summary>
        /// Gets service of type <paramref name="serviceType" /> from the
        /// <see cref="T:Autofac.Extensions.DependencyInjection.AutofacServiceProvider" /> and requires it be present.
        /// </summary>
        /// <param name="serviceType">
        /// An object that specifies the type of service object to get.
        /// </param>
        /// <returns>
        /// A service object of type <paramref name="serviceType" />.
        /// </returns>
        /// <exception cref="T:Autofac.Core.Registration.ComponentNotRegisteredException">
        /// Thrown if the <paramref name="serviceType" /> isn't registered with the container.
        /// </exception>
        /// <exception cref="T:Autofac.Core.DependencyResolutionException">
        /// Thrown if the object can't be resolved from the container.
        /// </exception>
        public object GetRequiredService(Type serviceType)
        {

            return this._componentContext.Resolve(serviceType);
        }

        /// <summary>Gets the service object of the specified type.</summary>
        /// <param name="serviceType">
        /// An object that specifies the type of service object to get.
        /// </param>
        /// <returns>
        /// A service object of type <paramref name="serviceType" />; or <see langword="null" />
        /// if there is no service object of type <paramref name="serviceType" />.
        /// </returns>
        public object GetService(Type serviceType)
        {
            return this._componentContext.ResolveOptional(serviceType);
        }
    }
}