using Dapper;
using MISA.AMIS.Core.Entities;
using MISA.AMIS.Core.Interfaces.Infrastructures;
using MISA.AMIS.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Infrastructure.Repository
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(IMISADbContext dbContext) : base(dbContext)
        {
        }
        public async Task<bool> CheckDuplicateEmpoloyeeCode(string employeeCode)
        {
            var sqlCommand = $"select EmployeeCode from Employee where EmployeeCode = @EmployeeCode";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"@EmployeeCode", employeeCode);
            var res = await _dbContext.Connection.QueryFirstOrDefaultAsync<string>(sql: sqlCommand, param: parameters, transaction:_dbContext.Transaction);
            if (res != null)
            {
                return true;
            }
            return false;
        }

        

        public async Task<PagingEntity<Employee>> Paging(int pageSize, int pageNumber, string? searchString)
        {
            var sql = $"Proc_Employee_FilterPaging";

            PagingEntity<Employee> pagingEntity = new PagingEntity<Employee>();
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@pageSize", pageSize);
            parameters.Add("@pageNumber", pageNumber);
            parameters.Add("@sreachString", searchString);

            parameters.Add("@totalRecord", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            parameters.Add("@totalPage", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

            var res = await _dbContext.Connection.QueryAsync<Employee>(sql, parameters, commandType: System.Data.CommandType.StoredProcedure);

            pagingEntity.Data = res;
            pagingEntity.TotalRecord = parameters.Get<int>("@totalRecord");
            pagingEntity.TotalPage = parameters.Get<int>("@totalPage");

            return pagingEntity;
        }
    }
}
