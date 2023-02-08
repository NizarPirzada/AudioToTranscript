using SimpleInjector;
using SimpleInjector.Lifestyles;
using System;

namespace Authenticity.CourtSide.Core
{
    internal class DomainManager : IDomainManager
    {
        internal Lazy<Scope> Scope { get; }
        internal Container Container => Scope.Value.Container;

        public DomainManager()
        {
            Scope = new Lazy<Scope>(BuildScopeBase);
        }

        protected virtual Scope BuildScopeBase()
        {
            // container
            var container = new Container();

            // container options
            container.Options.DefaultLifestyle = Lifestyle.Scoped;
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            // container options
            container.RegisterCore();

            // verify container
            container.Verify();

            // scope
            return AsyncScopedLifestyle.BeginScope(container);
        }

        public virtual T CreateProxy<T>() where T : class, IDomain
        {
            return Container.GetInstance<T>();
        }

        #region [ IDisposable ]

        protected bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Scope?.Value?.Dispose();
                }

                _disposed = true;
            }
        }

        ~DomainManager()
        {
            Dispose(false);
        }

        #endregion [ IDisposable ]
    }
}