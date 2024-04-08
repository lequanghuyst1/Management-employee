using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.AMIS.Core.Entities;
using MISA.AMIS.Core.Interfaces.Infrastructures;
using MISA.AMIS.Infrastructure.Interfaces;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace MISA.AMIS.Infrastructure.Repository
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        #region Constructor
        public CustomerRepository(IMISADbContext dbContext) : base(dbContext)
        {
        }
        #endregion

        #region Methods
        public async Task<bool> CheckCustomerCodeDuplicate(string customerCode)
        {
            var sqlCommand = $"select CustomerCode from Customer where CustomerCode = @CustomerCode";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"@CustomerCode", customerCode);
            var res = await _dbContext.Connection.QueryFirstOrDefaultAsync<string>(sql: sqlCommand, param: parameters, transaction: _dbContext.Transaction);
            if (res != null)
            {
                return true;
            }
            return false;
        }


        public async Task<PagingEntity<Customer>> Paging(int pageSize, int pageNumber, string? searchString)
        {
            var sql = $"Proc_Customer_FilterPaging";

            PagingEntity<Customer> pagingEntity = new PagingEntity<Customer>();
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@pageSize", pageSize);
            parameters.Add("@pageNumber", pageNumber);
            parameters.Add("@sreachString", searchString);

            parameters.Add("@totalRecord", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            parameters.Add("@totalPage", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

            var res = await _dbContext.Connection.QueryAsync<Customer>(sql, parameters, commandType: System.Data.CommandType.StoredProcedure);

            pagingEntity.Data = res;
            pagingEntity.TotalRecord = parameters.Get<int>("@totalRecord");
            pagingEntity.TotalPage = parameters.Get<int>("@totalPage");

            return pagingEntity;
        }

        /// <summary>
        /// Hàm dispose khi thực hiện xong thao tác với cơ sở dữ liệu
        /// </summary>
        /// Created by: LQHUY(25/12/2023)
        #endregion
    }
}
