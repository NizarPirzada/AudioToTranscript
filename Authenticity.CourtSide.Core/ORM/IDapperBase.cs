using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.ORM
{
    public interface IDapperBase<T>
    {
        Task<T> GetByIdAsync(string sql, object parameters = null);

        Task<IEnumerable<T>> GetAllAsync(string sql, object parameters = null);

        Task<int> SaveAsync(string sql, object parameters = null);

        Task<TResult> SaveAsync<TResult>(string sql, object parameters = null);

        Task<int> DeleteAsync(string sql, object parameters = null);

        Task<TResult> DeleteAsync<TResult>(string sql, object parameters = null);

        Task<int> ExecuteCommandAsync(string sql, object parameters = null);

        Task<int> CheckExistsIdsAsync(string sql, object parameters = null);

        Task<int> CheckExistsValueAsync(string sql, object parameters = null);

        Task<IEnumerable<T>> GetByIdWithRelationsAsync<TRelation1>(string sql, Func<T, TRelation1, IDictionary<int, T>, T> mapperBody, string splitOn, object parameters = null);

        Task<IEnumerable<T>> GetByIdWithRelationsAsync<TRelation1, TRelation2>(string sql, Func<T, TRelation1, TRelation2, IDictionary<int, T>, T> mapperBody, string splitOn, object parameters = null);

        Task<IEnumerable<T>> GetByIdWithRelationsAsync<TRelation1, TRelation2, TRelation3>(string sql, Func<T, TRelation1, TRelation2, TRelation3, IDictionary<int, T>, T> mapperBody, string splitOn, object parameters = null);

        Task<int> EditAsync(string sql, object parameters = null);

        Task<TResult> EditAsync<TResult>(string sql, object parameters = null);

        Task<int> ChangeStatusAsync(string sql, object parameters = null);

        Task<int> CheckExistsAsync(string sql, object parameters = null);

        Task<IEnumerable<DateTime>> GetDatesAsync(string sql, object parameters = null);

        Task<TResult> GetAsync<TResult>(string sql, object parameters = null);

        Task<IEnumerable<T>> QueryAllAsync(string sql, object parameters);

        Task<IEnumerable<T>> QueryAllWithRelationsAsync<TRelation1>(string sql, Func<T, TRelation1, IDictionary<int, T>, T> mapperBody, string splitOn, object parameters = null);
    }
}
