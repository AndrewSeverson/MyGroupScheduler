using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;

namespace GroupScheduler.Infrastructure.DependencyResolver
{
    public abstract class DependencyRegistrar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyRegistrar"/> class.
        /// </summary>
        /// <param name="containerBuilder">The container builder to register dependencies with.</param>
        protected DependencyRegistrar(ContainerBuilder containerBuilder)
        {
            this.ContainerBuilder = containerBuilder;
        }

        #region Public Methods

        /// <summary>
        /// Instructs this dependency registrar to register all of its dependencies
        /// </summary>
        public abstract void RegisterDependencies();

        #endregion

        /// <summary>
        /// Gets the container builder.
        /// </summary>
        protected ContainerBuilder ContainerBuilder { get; private set; }
    }
}