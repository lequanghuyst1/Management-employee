using MISA.AMIS.Core.Entities;
using MISA.AMIS.Core.Exceptions;
using MISA.AMIS.Core.Interfaces.Infrastructures;
using MISA.AMIS.Core.Interfaces.Services;
using MISA.AMIS.Core.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.Services
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        #region Field
        IBaseRepository<T> _baseRepository;
        Dictionary<string, string[]>? erorrs = new Dictionary<string, string[]>();
        #endregion

        #region Constructor
        public BaseService(IBaseRepository<T> baseRepository)
        {
            _baseRepository = baseRepository;
        }


        #endregion

        #region Methods
        public async Task<int> InsertServiceAsync(T entity)
        {
            await ValidateInsertAsync(entity);
            var res = await _baseRepository.InsertAsync(entity);
            if (res > 0)
            {
                return res;
            }

            erorrs?.Add("Insert", new string[] { String.Format(ResourceVN.InsertFailure) });
            throw new ValidateException(HttpStatusCode.BadRequest, String.Format(ResourceVN.InsertFailure), erorrs);

        }

        public async Task<int> UpdateServiceAsync(T entity, Guid id)
        {
            await ValidateUpdateAsync(entity);
            var res = await _baseRepository.UpdateAsync(entity, id);
            if (res > 0)
            {
                return res;
            }

            erorrs?.Add("Update", new string[] { String.Format(ResourceVN.UpdateFailure) });
            throw new ValidateException(HttpStatusCode.BadRequest, String.Format(ResourceVN.UpdateFailure), erorrs);

        }

        public async Task<int> DeleteServiceAsync(Guid id)
        {
            var tableName = typeof(T).Name;
            var className = GetClassnameTable(tableName);

            var entity = await _baseRepository.GetByIdAsync(id);
            if (entity != null)
            {
                var res = await _baseRepository.DeleteAsync(id);
                return res;
            }
            else
            {
                erorrs?.Add(tableName, new string[] { String.Format(ResourceVN.NotExitSystem, className) });
                throw new ValidateException(HttpStatusCode.BadRequest, String.Format(ResourceVN.NotExitSystem, className), erorrs);
            }
        }

        /// <summary>
        /// Hàm lấy ra tên của class
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        /// Crated By: LQHUY(02/01/2024)
        private string GetClassnameTable(string tableName)
        {
            if (tableName == "Employee")
            {
                return ResourceVN.EmployeeClassname;
            }
            if (tableName == "Customer")
            {
                return ResourceVN.CustomerClassname;
            }
            return "";
        }

        public async Task<int> DeleteManyServiceAsync(List<Guid> ids)
        {
            var tableName = typeof(T).Name;
            var className = GetClassnameTable(tableName);

            var entities = await _baseRepository.GetByIdsAsync(ids);
            //int count = 0;
            //for (int i = 0; i < ids.Count; i++)
            //{
            //    var res = _baseRepository.GetByIdAsync(ids[i]);
            //    if (res != null)
            //    {
            //        count++;
            //    }
            //    else { continue; }
            //}
            //if (count == ids.Count)
            //{
            //    var res = await _baseRepository.DeleteManyAsync(ids);
            //    return res;
            //}
            if (entities.Count() == ids.Count)
            {
                var res = await _baseRepository.DeleteManyAsync(ids);
                return res;
            }
            else
            {
                erorrs?.Add(tableName, new string[] { String.Format(ResourceVN.NotExitSystem, className) });
                throw new ValidateException(HttpStatusCode.BadRequest, String.Format(ResourceVN.NotExitSystem, className), erorrs);
            }
        }

        /// <summary>
        /// Validate dữ liệu khi thêm mới đối tượng
        /// </summary>
        /// <param name="entity">thông tin đối tượng</param>
        /// <returns></returns>
        /// CreatedBy: LQHUY(22/02/2024)
        public virtual async Task ValidateInsertAsync(T entity)
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// Validte dữ liệu khi cập nhật thông tin đối tượng
        /// </summary>
        /// <param name="entity">thông tin đối tượng</param>
        /// <param name="id">mã định danh đối tượng</param>
        /// <returns></returns>
        /// CreatedBy: LQHUY(22/02/2024)
        public virtual async Task ValidateUpdateAsync(T entity)
        {
            await Task.CompletedTask;
        }
        #endregion
    }
}
