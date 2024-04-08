using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.AMIS.Core.Entities;
using MISA.AMIS.Core.Exceptions;
using MISA.AMIS.Core.Resources;
using MISA.AMIS.Infrastructure.Interfaces;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.AMIS.Infrastructure.MISADbContext
{
    /// <summary>
    /// Class MariaDbContext
    /// </summary>
    /// Created By: LQHUY(03/01/2024)
    public class MariaDbContext : IMISADbContext
    {
        #region Field
        readonly string ConnectionString;
        #endregion

        #region Property
        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; set; }
        #endregion

        #region Constructor
        public MariaDbContext(IConfiguration configuration)
        {
            try
            {
                ConnectionString = configuration.GetConnectionString("DefaultConnection");
                Connection = new MySqlConnection(ConnectionString);
            }
            catch (MySqlException)
            {
                Dictionary<string, string[]>? errors = new Dictionary<string, string[]>();
                errors.Add("Database", new string[] { ResourceVN.ConnectDbException });
                throw new ConnectDbException(System.Net.HttpStatusCode.InternalServerError, ResourceVN.ConnectDbException, errors);
            }
        }
        #endregion

        #region Methods
        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            var tableName = typeof(T).Name;
            var sql = $"Proc_{tableName}_GetAll";
            var res = await Connection.QueryAsync<T>(sql, commandType: CommandType.StoredProcedure, transaction: Transaction);
            return res;
        }

        public async Task<T?> GetByIdAsync<T>(Guid id)
        {
            var tableName = typeof(T).Name;
            var sql = $"SELECT * FROM {tableName} WHERE {tableName}Id = @{tableName}Id";
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add($"@{tableName}Id", id);
            var res = await Connection.QueryFirstOrDefaultAsync<T>(sql, dynamicParameters);
            return res;
        }

        public async Task<int> InsertAsync<T>(T entity)
        {
            var tableName = typeof(T).Name;
            var sql = $"Proc_{tableName}_Insert";
            var res = await Connection.ExecuteAsync(sql, entity, transaction: Transaction, commandType: CommandType.StoredProcedure);
            return res;
        }

        public async Task<int> UpdateAsync<T>(T entity, Guid id)
        {
            var tableName = typeof(T).Name;
            var sql = $"Proc_{tableName}_Update";
            entity?.GetType()?.GetProperty($"{tableName}Id")?.SetValue(entity, id);
            var res = await Connection.ExecuteAsync(sql, entity, commandType: CommandType.StoredProcedure);
            return res;
        }

        public async Task<int> DeleteAsync<T>(Guid id)
        {
            var tableName = typeof(T).Name;
            var sql = $"DELETE FROM {tableName} WHERE {tableName}Id = @{tableName}Id";
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add($"@{tableName}Id", id);
            var res = await Connection.ExecuteAsync(sql, dynamicParameters);
            return res;
        }

        public async Task<int> DeleteManyAsync<T>(List<Guid> ids)
        {
            var tableName = typeof(T).Name;
            var sql = $"DELETE FROM {tableName} WHERE {tableName}Id IN @Ids";
            var res = await Connection.ExecuteAsync(sql, new { Ids = ids });
            return res;
        }

        public async Task<IEnumerable<T>> GetByIdsAsync<T>(List<Guid> ids)
        {
            var tableName = typeof(T).Name;
            string viewTable = $"View_{tableName}";
            var sql = $"SELECT * FROM {viewTable} WHERE {tableName}Id IN @ids ORDER BY CreatedDate DESC";
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add($"@ids", ids);
            var res = await Connection.QueryAsync<T>(sql, dynamicParameters);
            return res;
        }

        public async Task<bool> CheckDuplicateCodeAsync<T>(string entityCode)
        {
            var tableName = typeof(T).Name;
            var sqlCommand = $"select {tableName}Code from {tableName} where {tableName}Code = @{tableName}Code";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"@{tableName}Code", entityCode);
            var res = await Connection.QueryFirstOrDefaultAsync<string>(sql: sqlCommand, param: parameters, transaction: Transaction);
            if (res != null)
            {
                return true;
            }
            return false;
        }

        public async Task<string?> GetCodeById<T>(Guid id)
        {
            var tableName = typeof(T).Name;
            var sql = $"SELECT {tableName}Code FROM {tableName} WHERE {tableName}Id = @{tableName}Id";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"@{tableName}Id", id);
            var code = await Connection.QueryFirstOrDefaultAsync<string>(sql, parameters);
            return code;
        }

        public async Task<bool> CheckEnityCodeEqualCodeByIdAsync<T>(string entityCode, Guid id)
        {
            var tableName = typeof(T).Name;
            var sql = $"SELECT {tableName}Code FROM {tableName} WHERE {tableName}Id = @{tableName}Id";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"@{tableName}Id", id);
            var code = await Connection.QueryFirstOrDefaultAsync<string>(sql, parameters);
            if (entityCode == code)
            {
                return true;
            }
            return false;
        }

        public async Task<T?> GetByNameAsync<T>(string entityName)
        {
            var tableName = typeof(T).Name;
            var sql = "";
            if (tableName == "Position")
            {
                sql = $"SELECT * FROM {tableName}s WHERE {tableName}Name = @{tableName}Name";
            }
            else
            {
                sql = $"SELECT * FROM {tableName} WHERE {tableName}Name = @{tableName}Name";
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"@{tableName}Name", entityName);
            var res = await Connection.QueryFirstOrDefaultAsync<T>(sql, parameters, Transaction);
            return res;
        }

        public async Task<string> GetNewCodeAsync<T>()
        {
            var tableName = typeof(T).Name;
            var sql = $"SELECT {tableName}Code FROM {tableName}";
            var listEntityCode = (await Connection.QueryAsync<string>(sql)).ToList();
            var listNumberCode = new List<int>();
            var prefixNewCode = GetPrefixNewCode(tableName);

            string pattern = @"\d+"; // \d+ sẽ tìm tất cả các chữ số liên tục
            Regex regex = new Regex(pattern);

            // Tìm và lấy phần số từ chuỗi
            for (var i = 0; i < listEntityCode.Count; i++)
            {
                Match match = regex.Match(listEntityCode[i]);
                if (match.Success)
                {
                    var number = int.Parse(match.Value);
                    listNumberCode.Add(number);
                }
                //listNumberCode.Add(int.Parse(listEntityCode[i].Replace($"{prefixNewCode}-", "")));
            }
            listNumberCode.Sort();
            var newCode = $"{prefixNewCode}-{listNumberCode[listNumberCode.Count - 1] + 1}";
            return newCode;
        }

        public string GetPrefixNewCode(string tableName)
        {
            switch (tableName)
            {
                case "Employee":
                    return "NV";
                case "Customer":
                    return "KH";
                case "Department":
                    return "PB";
                case "Postion":
                    return "VT";
                default:
                    return "";
            }
        }
        #endregion
    }
}
