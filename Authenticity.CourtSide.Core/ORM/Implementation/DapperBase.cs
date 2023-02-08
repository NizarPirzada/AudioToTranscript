using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.ORM.Implementation
{
    public class DapperBase<T> : IDapperBase<T> where T : class
    {
        private SqlConnection sqlConnection;
        private readonly IConfiguration _configuration;

        private const int POLICY_NUMBER_OF_RETRIES = 3;
        private TimeSpan PAUSE_BETWEEN_FAILURES = TimeSpan.FromMilliseconds(500);

        public DapperBase(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private async Task<SqlConnection> GetSqlConnection()
        {
            await Task.Yield();
            string connectionString = _configuration.GetConnectionString("Courtside.ConnectionString");
            return new SqlConnection(connectionString);
        }

        public async Task<int> SaveAsync(string sql, object parameters = null)
        {
            int result = await Policy.Handle<Exception>()
            .WaitAndRetryAsync(POLICY_NUMBER_OF_RETRIES, i => PAUSE_BETWEEN_FAILURES)
            .ExecuteAsync(async () =>
            {
                using (sqlConnection = await GetSqlConnection())
                {
                    int createdId = await sqlConnection.QuerySingleOrDefaultAsync<int>(sql, parameters);
                    return createdId;
                }
            });
            return result;
        }

        public async Task<TResult> SaveAsync<TResult>(string sql, object parameters = null)
        {
            TResult result = await Policy.Handle<Exception>()
            .WaitAndRetryAsync(POLICY_NUMBER_OF_RETRIES, i => PAUSE_BETWEEN_FAILURES)
            .ExecuteAsync(async () =>
            {
                using (sqlConnection = await GetSqlConnection())
                {
                    TResult queryResult = await sqlConnection.QuerySingleOrDefaultAsync<TResult>(sql, parameters);
                    return queryResult;
                }
            });
            return result;
        }

        public async Task<int> DeleteAsync(string sql, object parameters = null)
        {
            int result = await Policy.Handle<Exception>()
            .WaitAndRetryAsync(POLICY_NUMBER_OF_RETRIES, i => PAUSE_BETWEEN_FAILURES)
            .ExecuteAsync(async () =>
            {
                using (sqlConnection = await GetSqlConnection())
                {
                    int affectedRows = await sqlConnection.ExecuteAsync(sql, parameters);
                    return affectedRows;
                }
            });
            return result;
        }

        public async Task<TResult> DeleteAsync<TResult>(string sql, object parameters = null)
        {
            TResult result = await Policy.Handle<Exception>()
            .WaitAndRetryAsync(POLICY_NUMBER_OF_RETRIES, i => PAUSE_BETWEEN_FAILURES)
            .ExecuteAsync(async () =>
            {
                using (sqlConnection = await GetSqlConnection())
                {
                    TResult queryResult = await sqlConnection.QuerySingleOrDefaultAsync<TResult>(sql, parameters);
                    return queryResult;
                }
            });
            return result;
        }

        public async Task<IEnumerable<T>> GetAllAsync(string sql, object parameters = null)
        {
            IEnumerable<T> result = await Policy.Handle<Exception>()
            .WaitAndRetryAsync(POLICY_NUMBER_OF_RETRIES, i => PAUSE_BETWEEN_FAILURES)
            .ExecuteAsync(async () =>
            {
                using (sqlConnection = await GetSqlConnection())
                {
                    IEnumerable<T> objects = await sqlConnection.QueryAsync<T>(sql, parameters);
                    return objects;
                }
            });
            return result;
        }

        public async Task<T> GetByIdAsync(string sql, object parameters = null)
        {
            T result = await Policy.Handle<Exception>()
            .WaitAndRetryAsync(POLICY_NUMBER_OF_RETRIES, i => PAUSE_BETWEEN_FAILURES)
            .ExecuteAsync(async () =>
            {
                using (sqlConnection = await GetSqlConnection())
                {
                    T model = await sqlConnection.QuerySingleOrDefaultAsync<T>(sql, parameters);
                    return model;
                }
            });
            return result;
        }

        public async Task<int> ExecuteCommandAsync(string sql, object parameters = null)
        {
            int result = await Policy.Handle<Exception>()
            .WaitAndRetryAsync(POLICY_NUMBER_OF_RETRIES, i => PAUSE_BETWEEN_FAILURES)
            .ExecuteAsync(async () =>
            {
                using (sqlConnection = await GetSqlConnection())
                {
                    int affectedRows = await sqlConnection.ExecuteAsync(sql, parameters);
                    return affectedRows;
                }
            });
            return result;
        }

        public async Task<IEnumerable<T>> GetByIdWithRelationsAsync<TRelation1>(string sql, Func<T, TRelation1, IDictionary<int, T>, T> mapperBody, string splitOn, object parameters = null)
        {
            IEnumerable<T> result = await Policy.Handle<Exception>()
            .WaitAndRetryAsync(POLICY_NUMBER_OF_RETRIES, i => PAUSE_BETWEEN_FAILURES)
            .ExecuteAsync(async () =>
            {
                IEnumerable<T> model = null;
                using (SqlConnection sqlConnection = await GetSqlConnection())
                {
                    var modelRelations = new Dictionary<int, T>();
                    var response = await sqlConnection.QueryAsync<T, TRelation1, T>(sql,
                       (pd, pp) =>
                       {
                           var modelRow = mapperBody(pd, pp, modelRelations);
                           return modelRow;
                       }, splitOn: splitOn, param: parameters);
                    model = modelRelations.Values.AsEnumerable();
                }
                return model;
            });

            return result;
        }

        public async Task<IEnumerable<T>> GetByIdWithRelationsAsync<TRelation1, TRelation2>(string sql, Func<T, TRelation1, TRelation2, IDictionary<int, T>, T> mapperBody, string splitOn, object parameters = null)
        {
            IEnumerable<T> result = await Policy.Handle<Exception>()
            .WaitAndRetryAsync(POLICY_NUMBER_OF_RETRIES, i => PAUSE_BETWEEN_FAILURES)
            .ExecuteAsync(async () =>
            {
                IEnumerable<T> model = null;
                using (SqlConnection sqlConnection = await GetSqlConnection())
                {
                    var modelRelations = new Dictionary<int, T>();
                    var response = await sqlConnection.QueryAsync<T, TRelation1, TRelation2, T>(sql,
                       (mainModel, relation1Model, relation2Model) =>
                       {
                           var modelRow = mapperBody(mainModel, relation1Model, relation2Model, modelRelations);
                           return modelRow;
                       }, splitOn: splitOn, param: parameters);
                    model = modelRelations.Values.AsEnumerable();
                }
                return model;
            });
            return result;
        }

        public async Task<int> CheckExistsIdsAsync(string sql, object parameters = null)
        {
            int result = await Policy.Handle<Exception>()
            .WaitAndRetryAsync(POLICY_NUMBER_OF_RETRIES, i => PAUSE_BETWEEN_FAILURES)
            .ExecuteAsync(async () =>
            {
                using (sqlConnection = await GetSqlConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("@ids", parameters);
                    int count = await sqlConnection.QuerySingleOrDefaultAsync<int>(sql, param);
                    return count;
                }
            });
            return result;
        }

        public async Task<int> CheckExistsValueAsync(string sql, object parameters = null)
        {
            int result = await Policy.Handle<Exception>()
            .WaitAndRetryAsync(POLICY_NUMBER_OF_RETRIES, i => PAUSE_BETWEEN_FAILURES)
            .ExecuteAsync(async () =>
            {
                using (sqlConnection = await GetSqlConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("@value", parameters);
                    int count = await sqlConnection.QuerySingleOrDefaultAsync<int>(sql, param);
                    return count;
                }
            });
            return result;
        }

        public async Task<int> CheckExistsAsync(string sql, object parameters = null)
        {
            int result = await Policy.Handle<Exception>()
            .WaitAndRetryAsync(POLICY_NUMBER_OF_RETRIES, i => PAUSE_BETWEEN_FAILURES)
            .ExecuteAsync(async () =>
            {
                using (sqlConnection = await GetSqlConnection())
                {
                    int count = await sqlConnection.QuerySingleOrDefaultAsync<int>(sql, parameters);
                    return count;
                }
            });
            return result;
        }

        public async Task<int> EditAsync(string sql, object parameters = null)
        {
            int result = await Policy.Handle<Exception>()
            .WaitAndRetryAsync(POLICY_NUMBER_OF_RETRIES, i => PAUSE_BETWEEN_FAILURES)
            .ExecuteAsync(async () =>
            {
                using (sqlConnection = await GetSqlConnection())
                {
                    int affectedRows = await sqlConnection.ExecuteAsync(sql, parameters);
                    return affectedRows;
                }
            });
            return result;
        }

        public async Task<TResult> EditAsync<TResult>(string sql, object parameters = null)
        {
            TResult result = await Policy.Handle<Exception>()
                .WaitAndRetryAsync(POLICY_NUMBER_OF_RETRIES, i => PAUSE_BETWEEN_FAILURES)
                .ExecuteAsync(async () =>
                {
                    using (sqlConnection = await GetSqlConnection())
                    {
                        TResult editResult = await sqlConnection.QuerySingleOrDefaultAsync<TResult>(sql, parameters);
                        return editResult;
                    }
                });

            return result;
        }

        public async Task<int> ChangeStatusAsync(string sql, object parameters = null)
        {
            int result = await Policy.Handle<Exception>()
            .WaitAndRetryAsync(POLICY_NUMBER_OF_RETRIES, i => PAUSE_BETWEEN_FAILURES)
            .ExecuteAsync(async () =>
            {
                using (sqlConnection = await GetSqlConnection())
                {
                    int affectedRows = await sqlConnection.ExecuteAsync(sql, parameters);
                    return affectedRows;
                }
            });
            return result;
        }

        public async Task<IEnumerable<T>> GetByIdWithRelationsAsync<TRelation1, TRelation2, TRelation3>(string sql, Func<T, TRelation1, TRelation2, TRelation3, IDictionary<int, T>, T> mapperBody, string splitOn, object parameters = null)
        {
            IEnumerable<T> result = await Policy.Handle<Exception>()
            .WaitAndRetryAsync(POLICY_NUMBER_OF_RETRIES, i => PAUSE_BETWEEN_FAILURES)
            .ExecuteAsync(async () =>
            {
                IEnumerable<T> model = null;
                using (SqlConnection sqlConnection = await GetSqlConnection())
                {
                    var modelRelations = new Dictionary<int, T>();
                    var response = await sqlConnection.QueryAsync<T, TRelation1, TRelation2, TRelation3, T>(sql,
                       (mainModel, relation1Model, relation2Model, relation3Model) =>
                       {
                           var modelRow = mapperBody(mainModel, relation1Model, relation2Model, relation3Model, modelRelations);
                           return modelRow;
                       }, splitOn: splitOn, param: parameters);
                    model = modelRelations.Values.AsEnumerable();
                }
                return model;
            });
            return result;
        }

        public async Task<IEnumerable<DateTime>> GetDatesAsync(string sql, object parameters = null)
        {
            IEnumerable<DateTime> result = await Policy.Handle<Exception>()
            .WaitAndRetryAsync(POLICY_NUMBER_OF_RETRIES, i => PAUSE_BETWEEN_FAILURES)
            .ExecuteAsync(async () =>
            {
                using (sqlConnection = await GetSqlConnection())
                {
                    IEnumerable<DateTime> objects = await sqlConnection.QueryAsync<DateTime>(sql, parameters);
                    return objects;
                }
            });
            return result;
        }

        public async Task<IEnumerable<T>> QueryAllAsync(string sql, object parameters)
        {
            using (sqlConnection = await GetSqlConnection())
            {
                IEnumerable<T> objects = await sqlConnection.QueryAsync<T>(sql, parameters);
                return objects;
            }
        }

        public async Task<TResult> GetAsync<TResult>(string sql, object parameters = null)
        {
            TResult result = await Policy.Handle<Exception>()
            .WaitAndRetryAsync(POLICY_NUMBER_OF_RETRIES, i => PAUSE_BETWEEN_FAILURES)
            .ExecuteAsync(async () =>
            {
                using (sqlConnection = await GetSqlConnection())
                {
                    TResult objects = await sqlConnection.QueryFirstOrDefaultAsync<TResult>(sql, parameters).ConfigureAwait(false);

                    return objects;
                }
            })
            .ConfigureAwait(false);

            return result;
        }

        public async Task<IEnumerable<T>> QueryAllWithRelationsAsync<TRelation1>(string sql, Func<T, TRelation1, IDictionary<int, T>, T> mapperBody, string splitOn, object parameters = null)
        {
            IEnumerable<T> result = await Policy.Handle<Exception>()
            .WaitAndRetryAsync(POLICY_NUMBER_OF_RETRIES, i => PAUSE_BETWEEN_FAILURES)
            .ExecuteAsync(async () =>
            {
                IEnumerable<T> model = null;
                using (SqlConnection sqlConnection = await GetSqlConnection())
                {
                    var modelRelations = new Dictionary<int, T>();
                    var response = await sqlConnection.QueryAsync<T, TRelation1, T>(sql,
                       (pd, pp) =>
                       {
                           var modelRow = mapperBody(pd, pp, modelRelations);
                           return modelRow;
                       }, splitOn: splitOn, param: parameters);
                    model = modelRelations.Values.AsEnumerable();
                }
                return model;
            });

            return result;
        }
    }
}
