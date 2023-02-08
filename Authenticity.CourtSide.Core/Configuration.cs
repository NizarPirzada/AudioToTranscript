using Authenticity.CourtSide.Core.RetryPolicies;
using NSerio.Utils.SimpleInjector;
using Polly.Registry;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Authenticity.CourtSide.Test")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Authenticity.CourtSide.Core
{
    internal static class Configuration
    {
        private static IEnumerable<Assembly> Assemblies { get; } = new Assembly[]
        {
            Assembly.GetExecutingAssembly()
        };

        internal static void RegisterCore(this Container container)
        {
            container.RegisterCommon();
            container.RegisterPollyRetryPolicies();

            // register from type (dynamically)
            container.RegisterAllServiceTypeImplementations<IInjectable>(Assemblies);
        }

        private static void RegisterCommon(this Container container)
        {

        }

        private static void RegisterPollyRetryPolicies(this Container container)
        {
            var policyRegistry = new PolicyRegistry();

            policyRegistry.RegisterSqlAsyncRetryPolicy();

            container
                .RegisterInstance<IReadOnlyPolicyRegistry<string>>(policyRegistry);
        }
    }
}