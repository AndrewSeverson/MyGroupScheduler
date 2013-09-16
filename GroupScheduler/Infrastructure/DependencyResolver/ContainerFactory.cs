using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Integration.Mvc;
using GroupScheduler.Infrastructure.Data;

namespace GroupScheduler.Infrastructure.DependencyResolver
{
    public class ContainerFactory
    {
        #region Constants and Fields

        /// <summary>
        /// Dependencies will be registered with this ContainerBuilder object.
        /// </summary>
        private readonly ContainerBuilder containerBuilder;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerBuilder"/> class.
        /// </summary>
        public ContainerFactory()
        {
            this.containerBuilder = new ContainerBuilder();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates the Autofac container builder. This method should register all dependencies used by this project with the container.
        /// </summary>
        /// <returns>Autofac Container Builder</returns>
        public ContainerBuilder CreateContainerBuilder()
        {
            // Sets up all controllers for dependency injection
            this.containerBuilder.RegisterControllers(Assembly.GetExecutingAssembly());

            // Register database classes and such for portal apps & modules
            this.RegisterDependencyResolvers();

            /* Automatically injects HttpContextBase, HttpRequestBase, HttpResponseBase, etc.
             * See: http://code.google.com/p/autofac/wiki/Mvc3Integration#Injecting_HTTP_Abstractions
             */
            this.containerBuilder.RegisterModule(new AutofacWebTypesModule());

            return this.containerBuilder;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Scans project for any classes that implement the IDependencyRegistrar. For each found class, this method will
        /// instantiate it and request that it register its dependencies with the current container builder.
        /// </summary>
        private void RegisterDependencyResolvers()
        {
            Type dependencyRegistrarType = typeof(DependencyRegistrar);
            Type[] initTypes = dependencyRegistrarType.Assembly.GetTypes();

            // Avoids trying to create an instance of an abstract class, which is impossible.
            IEnumerable<Type> dependencyRegistrarTypes = initTypes.Where(t => dependencyRegistrarType.IsAssignableFrom(t) && t.IsConcrete());

            // Uses reflection to create an instance of each concrete class.
            IEnumerable<DependencyRegistrar> dependencyRegistrars =
                dependencyRegistrarTypes
                .Select(t => Activator.CreateInstance(t, this.containerBuilder))
                .Cast<DependencyRegistrar>();

            foreach (DependencyRegistrar dependencyRegistrar in dependencyRegistrars)
            {
                dependencyRegistrar.RegisterDependencies();
            }
        }

        #endregion

    }
}