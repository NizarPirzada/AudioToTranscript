using Microsoft.Data.SqlClient;
using Polly;
using Polly.Registry;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.RetryPolicies
{
    internal static class SqlAsyncRetryPolicy
    {
        private const string RetryPolicyKey = "SqlRetryPolicy";
        private readonly static int[] SqlRetryCodes = { 1204, 1205, 17197, 8645, 8675 };

        private static IAsyncPolicy GetRetryPolicy()
        {
            return Policy
                    .Handle<SqlException>(exception =>

                        /*
                         * Ref:
                         *	https://msdn.microsoft.com/en-us/library/cc231199.aspx
                         *
                         */

                        SqlRetryCodes.Contains(exception.ErrorCode)
                    )
                    .WaitAndRetryAsync(3, retryAttempt =>
                        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    );
        }

        public static PolicyRegistry RegisterSqlAsyncRetryPolicy(this PolicyRegistry policyRegistry)
        {
            policyRegistry.Add(RetryPolicyKey, GetRetryPolicy());
            return policyRegistry;
        }

        public static IAsyncPolicy GetSqlAsyncRetryPolicy(this IReadOnlyPolicyRegistry<string> policyRegistry)
        {
            return policyRegistry.Get<IAsyncPolicy>(RetryPolicyKey);
        }

        public static Task<T> UseSqlRetryPolicy<T>(this Task<T> task)
        {
            return GetRetryPolicy().ExecuteAsync(() => task);
        }
    }
}
