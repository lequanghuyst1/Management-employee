using AutoMapper;
using MISA.AMIS.Core.Entities;
using MISA.AMIS.Core.Exceptions;
using MISA.AMIS.Core.Interfaces.Infrastructures;
using MISA.AMIS.Core.Interfaces.Services;
using MISA.AMIS.Core.Interfaces.UnitOfWork;
using MISA.AMIS.Core.Services;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.UnitTests.Service
{
    [TestFixture]
    public class DepartmentServiceTests
    {
        //public IDepartmentRepository DepartmentRepository { get; set; }
        //public  IDepartmentService DepartmentService { get; set; }

        ///// <summary>
        ///// Hàm thiết lập cho class test
        ///// </summary>
        ///// Created By: LQHUY(28/02/2023)
        //[SetUp]
        //public void SetUp()
        //{
        //    DepartmentRepository = Substitute.For<IDepartmentRepository>();
        //    DepartmentService = new DepartmentService(DepartmentRepository);
        //}

        ///// <summary>
        ///// Hàm unit test thêm mới phòng ban (đầu vào là DepartmentCode bị trùng)
        ///// </summary>
        ///// Created BY: LQHUY(28/02/2024)
        //[Test]
        //public void InsertServiceAsync_DuplicateDepartmentCode_ThrowException()
        //{
        //    //arrange
        //    var department = new Department();
        //    DepartmentRepository.CheckDuplicateCodeAsync(department.DepartmentCode).Returns(true);

        //    //act & assert 
        //    var exception = Assert.ThrowsAsync<ValidateException>(async () => await DepartmentService.InsertServiceAsync(department));

        //    Assert.That(exception.status, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
        //}

        ///// <summary>
        ///// Hàm unit test thêm mới phòng ban (đầu vào là DepartmentCode hợp lệ)
        ///// </summary>
        ///// Created BY: LQHUY(28/02/2024)
        //[Test]
        //public async Task InsertServiceAsync_ValidDepartmentCode_RowSuccess()
        //{
        //    //arrange
        //    var department = new Department();
        //    DepartmentRepository.CheckDuplicateCodeAsync(department.DepartmentCode).Returns(false);
        //    DepartmentRepository.InsertAsync(department).Returns(1);

        //    //act 
        //    var expectResult = await DepartmentService.InsertServiceAsync(department);

        //    // assert
        //    Assert.That(expectResult, Is.EqualTo(1));
        //    await DepartmentRepository.Received(1).InsertAsync(department);

        //}

        ///// <summary>
        ///// Hàm unit test sửa thông tin phòng ban (đầu vào là DepartmentCode hợp lệ)
        ///// </summary>
        ///// <returns></returns>
        ///// Created BY: LQHUY(28/02/2024)
        //[Test]
        //public async Task UpdateServiceAsync_ValidDepartmentCode_RowSuccess()
        //{
        //    //arrange
        //    var department = new Department();
        //    DepartmentRepository.CheckDuplicateCodeAsync(department.DepartmentCode).Returns(false);
        //    DepartmentRepository.UpdateAsync(department, department.DepartmentId).Returns(1);

        //    //act 
        //    var expectResult = await DepartmentService.UpdateServiceAsync(department, department.DepartmentId);

        //    // assert
        //    Assert.That(expectResult, Is.EqualTo(1));
        //}

        ///// <summary>
        ///// Hàm unit test sửa thông tin phòng ban (đầu vào là DepartmentCode trùng với thông tin cần sửa)
        ///// </summary>
        ///// <returns></returns>
        ///// Created BY: LQHUY(28/02/2024)
        //[Test]
        //public async Task UpdateServiceAsync_DuplicateInfoDepartmentCode_RowSuccess()
        //{
        //    //arrange
        //    var department = new Department();
        //    DepartmentRepository.CheckEnityCodeEqualCodeByIdAsync(department.DepartmentCode, department.DepartmentId).Returns(true);
        //    DepartmentRepository.UpdateAsync(department, department.DepartmentId).Returns(1);

        //    //act 
        //    var expectResult = await DepartmentService.UpdateServiceAsync(department, department.DepartmentId);

        //    // assert
        //    Assert.That(expectResult, Is.EqualTo(1));
        //}

        ///// <summary>
        ///// Hàm unit test sửa thông tin phòng ban (đầu vào là DepartmentCode bị trùng)
        ///// </summary>
        ///// <returns></returns>
        ///// Created BY: LQHUY(28/02/2024)
        //[Test]
        //public void UpdateServiceAsync_DuplicateDepartmentCode_ThrowExceoption()
        //{
        //    //arrange
        //    var department = new Department();
        //    DepartmentRepository.CheckDuplicateCodeAsync(department.DepartmentCode).Returns(true);

        //    // act & assert
        //    var exception = Assert.ThrowsAsync<ValidateException>(async () => await DepartmentService.UpdateServiceAsync(department, department.DepartmentId));

        //    Assert.That(exception.status, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));

        //}
    }
}
