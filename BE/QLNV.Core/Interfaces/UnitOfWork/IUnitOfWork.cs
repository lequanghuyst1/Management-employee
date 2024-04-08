using MISA.AMIS.Core.Interfaces.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Khai báo customerRepository
        /// </summary>
        /// Created By: LQHUY(15/01/2024)
        ICustomerRepository Customers{ get; }

        /// <summary>
        /// Khai báo customerRepository
        /// </summary>
        /// Created By: LQHUY(20/01/2024)
        IEmployeeRepository Employees { get; }

        IDepartmentRepository Departments { get; }
        IPositionRepository Positions { get; }
        /// <summary>
        /// Bắt đầu transition
        /// </summary>
        /// /// Created By: LQHUY(15/01/2024)
        void BeginTransaction();

        /// <summary>
        /// Commit vào database
        /// </summary>
        /// Created By: LQHUY(15/01/2024)
        void Commit();

        /// <summary>
        /// Quay trở lại trasition
        /// </summary>
        /// Created By: LQHUY(15/01/2024)
        void Rollback();
    }
}
