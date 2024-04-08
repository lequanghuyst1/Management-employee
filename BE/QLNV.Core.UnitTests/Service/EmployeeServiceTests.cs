using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using MISA.AMIS.Core.DTOs.EmployeeDTO;
using MISA.AMIS.Core.Entities;
using MISA.AMIS.Core.Exceptions;
using MISA.AMIS.Core.Interfaces.Infrastructures;
using MISA.AMIS.Core.Interfaces.Services;
using MISA.AMIS.Core.Interfaces.UnitOfWork;
using MISA.AMIS.Core.Services;
using MISA.AMIS.Infrastructure.Repository;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.UnitTests.Service
{
    [TestFixture]
    public class EmployeeServiceTests
    {
        public IEmployeeRepository EmployeeRepository { get; set; }
        public EmployeeService EmployeeService { get; set; }
        public IUnitOfWork UnitOfWork { get; set; }
        public IMapper Mapper { get; set; }
        public IMemoryCache MemoryCache { get; set; }

        /// <summary>
        /// Hàm thiết lập cho class test
        /// </summary>
        /// Created By; LQHUY(27/02/2023)
        [SetUp]
        public void SetUp()
        {
            EmployeeRepository = Substitute.For<IEmployeeRepository>();
            UnitOfWork = Substitute.For<IUnitOfWork>();
            Mapper = Substitute.For<IMapper>();
            MemoryCache = Substitute.For<IMemoryCache>();
            EmployeeService = Substitute.For<EmployeeService>(EmployeeRepository, UnitOfWork, Mapper, MemoryCache);
        }


        /// <summary>
        /// Hàm unit test thêm mới nhân viên (đầu vào là employee hợp lệ)
        /// </summary>
        /// <returns>số dòng thêm mới thành công</returns>
        /// Created BY: LQHUY(26/02/2024)
        [Test]
        public async Task InsertServiceAsync_ValidEmployee_RowSuccess()
        {
            //arrange
            var employee = new Employee();
            EmployeeRepository.InsertAsync(employee).Returns(1);
            EmployeeService.When(substituteCall: x => x.ValidateInsertAsync(Arg.Any<Employee>())).CallBase();

            //act 
            var actualReslut = await EmployeeService.InsertServiceAsync(employee);

            // assert
            Assert.That(actualReslut, Is.EqualTo(1));
            await EmployeeService.Received(1).ValidateInsertAsync(employee);
            await EmployeeRepository.Received(1).InsertAsync(employee);
        }

        /// <summary>
        /// Hàm unit test thêm mới nhân viên (đầu vào là employee hợp lệ)
        /// </summary>
        /// <returns>exception</returns>
        /// <exception cref="ValidateException"></exception>
        /// Created BY: LQHUY(26/02/2024)
        [Test]
        public async Task InsertServiceAsync_InValidEmployee_ThrowException()
        {
            //arrange
            var employee = new Employee();
            EmployeeService.When(substituteCall: x => x.ValidateInsertAsync(Arg.Any<Employee>())).CallBase();
            EmployeeRepository.InsertAsync(employee).Returns(0);

            //act & assert
            var exception = Assert.ThrowsAsync<ValidateException>(async () => await EmployeeService.InsertServiceAsync(employee));
            Assert.That(exception.status, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
            Assert.That(exception.Message, Is.EqualTo(Resources.ResourceVN.InsertFailure));
            await EmployeeService.Received(1).ValidateInsertAsync(employee);
            await EmployeeRepository.Received(1).InsertAsync(employee);
        }


        /// <summary>
        /// Hàm unit test validate khi thêm mới (đầu vào là EmployeeCode bị trùng)
        /// </summary>
        /// <exception cref="ValidateException"></exception>
        /// Created BY: LQHUY(26/02/2024)
        [Test]
        public async Task ValidateInsertAsync_DuplicateEmployeeCode_ThrowException()
        {
            //arrange
            var employee = new Employee();
            EmployeeRepository.CheckDuplicateCodeAsync(employee.EmployeeCode).Returns(true);
            EmployeeService.When(substituteCall: x => x.ValidateInsertAsync(Arg.Any<Employee>())).CallBase();

            //act & assert 
            var exception = Assert.ThrowsAsync<ValidateException>(async () => await EmployeeService.ValidateInsertAsync(employee));

            Assert.That(exception.status, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
            await EmployeeRepository.Received(1).CheckDuplicateCodeAsync(employee.EmployeeCode);
        }

        /// <summary>
        /// Hàm unit test sửa thông tin nhân viên (đầu vào là EmployeeCode hợp lệ)
        /// </summary>
        /// <returns>số dòng cập nhật thành công</returns>
        /// Created BY: LQHUY(26/02/2024)
        [Test]
        public async Task UpdateServiceAsync_ValidEmployee_RowSuccess()
        {
            //arrange
            var employee = new Employee();
            EmployeeService.When(substituteCall: x => x.ValidateUpdateAsync(Arg.Any<Employee>())).CallBase();
            EmployeeRepository.UpdateAsync(employee, employee.EmployeeId).Returns(1);

            //act 
            var actualReslut = await EmployeeService.UpdateServiceAsync(employee, employee.EmployeeId);

            // assert
            Assert.That(actualReslut, Is.EqualTo(1));
            await EmployeeRepository.Received(1).UpdateAsync(employee, employee.EmployeeId);
            await EmployeeService.Received(1).ValidateUpdateAsync(employee);

        }

        /// <summary>
        /// Hàm unit test sửa thông tin nhân viên (đầu vào là EmployeeCode hợp lệ)
        /// </summary>
        /// <exception cref="ValidateException"></exception>
        /// Created BY: LQHUY(26/02/2024)
        [Test]
        public async Task UpdateServiceAsync_ValidEmployee_ThrowException()
        {
            //arrange
            var employee = new Employee();
            EmployeeService.When(substituteCall: x => x.ValidateUpdateAsync(Arg.Any<Employee>())).CallBase();
            EmployeeRepository.UpdateAsync(employee, employee.EmployeeId).Returns(0);

            //act $ assert
            var exception = Assert.ThrowsAsync<ValidateException>(async () => await EmployeeService.UpdateServiceAsync(employee, employee.EmployeeId));
            Assert.That(exception.status, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
            Assert.That(exception.Message, Is.EqualTo(Resources.ResourceVN.UpdateFailure));
            await EmployeeService.Received(1).ValidateUpdateAsync(employee);
            await EmployeeRepository.Received(1).UpdateAsync(employee, employee.EmployeeId);
        }

        /// <summary>
        /// Hàm unit test validate khi cập nhật (đầu vào là EmployeeCode trùng với thông tin cần sửa)
        /// </summary>
        /// <returns></returns>
        /// Created BY: LQHUY(26/02/2024)
        [Test]
        public async Task ValidateUpdateAsync_DuplicateInfoEmployeeCode_Success()
        {
            //arrange
            var employee = new Employee();
            EmployeeRepository.CheckEnityCodeEqualCodeByIdAsync(employee.EmployeeCode, employee.EmployeeId).Returns(true);
            EmployeeService.When(substituteCall: x => x.ValidateUpdateAsync(Arg.Any<Employee>())).CallBase();

            //act 
            await EmployeeService.ValidateUpdateAsync(employee);

            // assert
            await EmployeeRepository.Received(1).CheckEnityCodeEqualCodeByIdAsync(employee.EmployeeCode, employee.EmployeeId);
        }

        /// <summary>
        /// Hàm unit test unit test validate khi cập nhật (đầu vào là EmployeeCode bị trùng)
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ValidateException"></exception>
        /// Created BY: LQHUY(26/02/2024)
        [Test]
        public async Task ValidateUpdateAsync_DuplicateEmployeeCode_ThrowException()
        {
            //arrange
            var employee = new Employee();
            EmployeeRepository.CheckEnityCodeEqualCodeByIdAsync(employee.EmployeeCode, employee.EmployeeId).Returns(false);
            EmployeeRepository.CheckDuplicateCodeAsync(employee.EmployeeCode).Returns(true);
            EmployeeService.When(substituteCall: x => x.ValidateUpdateAsync(Arg.Any<Employee>())).CallBase();

            // act & assert
            var exception = Assert.ThrowsAsync<ValidateException>(async () => await EmployeeService.ValidateUpdateAsync(employee));

            Assert.That(exception.status, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
            await EmployeeRepository.Received(1).CheckEnityCodeEqualCodeByIdAsync(employee.EmployeeCode, employee.EmployeeId);
            await EmployeeRepository.Received(1).CheckDuplicateCodeAsync(employee.EmployeeCode);
        }

        /// <summary>
        /// Hàm unit test xóa 1 nhân viên (đầu vào là EmployeeId đã có trong DB)
        /// </summary>
        /// <returns></returns>
        /// Created BY: LQHUY(27/02/2024)
        [Test]
        public async Task DeleteServiceAsync_ValidEmppoyeeId_CountDeleteSucces()
        {
            //arrange
            var employee = new Employee();
            EmployeeRepository.GetByIdAsync(employee.EmployeeId).Returns(employee);
            EmployeeRepository.DeleteAsync(employee.EmployeeId).Returns(1);

            //act
            var actualReslut = await EmployeeService.DeleteServiceAsync(employee.EmployeeId);

            //assert
            Assert.That(actualReslut, Is.EqualTo(1));
            await EmployeeRepository.Received(1).GetByIdAsync(employee.EmployeeId);
            await EmployeeRepository.Received(1).DeleteAsync(employee.EmployeeId);
        }

        /// <summary>
        /// Hàm unit test xóa 1 nhân viên (đầu vào là EmployeeId chưa có trong DB)
        /// </summary>
        /// <returns></returns>
        /// Created BY: LQHUY(27/02/2024)
        [Test]
        public async Task DeleteServiceAsync_NotExitEmppoyeeId_ThrowException()
        {
            //arrange
            var employee = new Employee();
            var id = employee.EmployeeId;
            employee = null;
            EmployeeRepository.GetByIdAsync(id).Returns(employee);

            //act & assert
            var exception = Assert.ThrowsAsync<ValidateException>(async () => await EmployeeService.DeleteServiceAsync(id));
            Assert.That(exception.status, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
            await EmployeeRepository.Received(1).GetByIdAsync(id);
        }

        /// <summary>
        /// Hàm unit test xóa nhiều nhân viên (đầu vào là danh sách EmployeeId đã có trong DB)
        /// </summary>
        /// <returns></returns>
        /// Created BY: LQHUY(27/02/2024)
        [Test]
        public async Task DeleteManyServiceAsync_ValidListEmployeeId_CountDeleteSucces()
        {
            //arrange
            var ids = new List<Guid>();
            var employee1 = new Employee();
            var employee2 = new Employee();
            ids.Add(employee1.EmployeeId);
            ids.Add(employee2.EmployeeId);

            var enitites = new List<Employee> { employee1, employee2 };

            EmployeeRepository.GetByIdsAsync(ids).Returns(enitites);
            EmployeeRepository.DeleteManyAsync(ids).Returns(ids.Count);

            var expectResult = ids.Count;

            //act
            var actualReslut = await EmployeeService.DeleteManyServiceAsync(ids);

            //assert
            Assert.That(actualReslut, Is.EqualTo(expectResult));
            Assert.That(enitites.Count, Is.EqualTo(expectResult));
            await EmployeeRepository.Received(1).GetByIdsAsync(ids);
            await EmployeeRepository.Received(1).DeleteManyAsync(ids);
        }

        /// <summary>
        /// Hàm unit test xóa nhiều nhân viên (đầu vào là có EmployeeId chưa có trong DB)
        /// </summary>
        /// <returns></returns>
        /// Created BY: LQHUY(27/02/2024)
        [Test]
        public async Task DeleteManyServiceAsync_NotExitsListEmployeeId_CountDeleteSucces()
        {
            //arrange
            var ids = new List<Guid>();
            var employee1 = new Employee();
            var employee2 = new Employee();
            ids.Add(employee1.EmployeeId);
            ids.Add(employee2.EmployeeId);
            var enitites = new List<Employee> { employee1 };

            EmployeeRepository.GetByIdsAsync(ids).Returns(enitites);

            //act & assert

            var exception = Assert.ThrowsAsync<ValidateException>(async () => await EmployeeService.DeleteManyServiceAsync(ids));
            Assert.That(exception.status, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
            await EmployeeRepository.Received(1).GetByIdsAsync(ids);

        }

        //[Test]
        //public  async Task ValidateDataImport_EmployeeCodeEmpty_MessageErorr()
        //{
        //    //arange
        //    var employeeImport = new EmployeeImportDto();
        //    employeeImport.EmployeeCode = "";
        //    //ation
        //   await EmployeeService.
        //    //assert
        //}
    }
}
