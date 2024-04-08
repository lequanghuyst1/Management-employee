using MISA.AMIS.Core.Interfaces.Infrastructures;
using MISA.AMIS.Core.Interfaces.UnitOfWork;
using MISA.AMIS.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        IMISADbContext _dbContext;
        public UnitOfWork(IMISADbContext dbContext, ICustomerRepository customerRepository, IEmployeeRepository employees, IDepartmentRepository departments, IPositionRepository positions)
        {
            _dbContext = dbContext;
            Customers = customerRepository;
            Employees = employees;
            Departments = departments;
            Positions = positions;
        }

        public ICustomerRepository Customers{ get; }

        public IEmployeeRepository Employees { get; }

        public IDepartmentRepository Departments { get; }

        public IPositionRepository Positions { get; }

        public void BeginTransaction()
        {
            _dbContext.Connection.Open();
            _dbContext.Transaction = _dbContext.Connection.BeginTransaction();
        }

        public void Commit()
        {
            _dbContext.Transaction.Commit();
        }

        public void Dispose()
        {
            if (_dbContext.Connection.State == ConnectionState.Open)
            {
                _dbContext.Connection.Close();
            }
            _dbContext.Connection?.Dispose();
        }

        public void Rollback()
        {
            _dbContext.Transaction.Rollback();
        }
    }
}
