
using Microsoft.Extensions.Configuration;
using MISA.AMIS.Core.Entities;
using MISA.AMIS.Core.Exceptions;
using MISA.AMIS.Core.Interfaces.Infrastructures;
using MISA.AMIS.Core.Resources;
using MISA.AMIS.Infrastructure.Interfaces;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Infrastructure.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        #region Field
        protected IMISADbContext _dbContext;
        #endregion

        #region Constructor 
        public BaseRepository(IMISADbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Methods
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var res = await _dbContext.GetAllAsync<T>();
            return res;
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            var res = await _dbContext.GetByIdAsync<T>(id);
            return res;
        }

        public async Task<int> InsertAsync(T entity)
        {
            var res = await _dbContext.InsertAsync<T>(entity);
            return res;
        }

        public async Task<int> UpdateAsync(T entity, Guid id)
        {
            var res = await _dbContext.UpdateAsync<T>(entity, id);
            return res;
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var res = await _dbContext.DeleteAsync<T>(id);
            return res;
        }

        public async Task<int> DeleteManyAsync(List<Guid> ids)
        {
            var res = await _dbContext.DeleteManyAsync<T>(ids);
            return res;
        }

        public async Task<IEnumerable<T>> GetByIdsAsync(List<Guid> ids)
        {
            var res = await _dbContext.GetByIdsAsync<T>(ids);
            return res;
        }

        public async Task<bool> CheckDuplicateCodeAsync(string entityCode)
        {
            var res = await _dbContext.CheckDuplicateCodeAsync<T>(entityCode);
            return res;
        }

        public async Task<string?> GetCodeById(Guid id)
        {
            var res = await _dbContext.GetCodeById<T>(id);
            return res;
        }

        public async Task<bool> CheckEnityCodeEqualCodeByIdAsync(string entityCode, Guid id)
        {
            var res = await _dbContext.CheckEnityCodeEqualCodeByIdAsync<T>(entityCode, id);
            return res;
        }

        public async Task<T?> GetByNameAsync(string entityName)
        {
            var res = await _dbContext.GetByNameAsync<T>(entityName);
            return res;
        }

        public async Task<string> GetNewCodeAsync()
        {
            var res = await _dbContext.GetNewCodeAsync<T>();
            return res;
        }
        #endregion
    }
}
