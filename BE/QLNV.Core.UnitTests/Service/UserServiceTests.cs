using AutoMapper;
using MISA.AMIS.Core.Entities;
using MISA.AMIS.Core.Exceptions;
using MISA.AMIS.Core.Interfaces.Infrastructures;
using MISA.AMIS.Core.Interfaces.Services;
using MISA.AMIS.Core.Interfaces.UnitOfWork;
using MISA.AMIS.Core.Services;
using MISA.AMIS.Infrastructure.Repository;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.UnitTests.Service
{
    [TestFixture]
    public class UserServiceTests
    {
        public IUserRepository UserRepository { get; set; }
        public IUserService UserService { get; set; }

        /// <summary>
        /// Hàm thiết lập cho class test
        /// </summary>
        /// Created By: LQHUY(27/02/2023)
        [SetUp]
        public void SetUp()
        {
            UserRepository = Substitute.For<IUserRepository>();
            UserService = new UserService(UserRepository);
        }

        /// <summary>
        /// Hàm unit test khi đăng nhập (đầu vào là tài khoản hợp lệ)
        /// </summary>
        /// <returns></returns>
        /// Created BY: LQHUY(26/02/2024)
        [Test]
        public async Task LoginAsync_ValidInfoUserLogin_InfoUser()
        {
            //Arrange
            UserLogin userLogin = new UserLogin();
            var user = new User();
            user.UserId = Guid.NewGuid();
            UserRepository.GetUser(userLogin).Returns(user);

            //Action
            await UserService.LoginAsync(userLogin);

            //Assert
            await UserRepository.Received(1).GetUser(userLogin);
        }

        /// <summary>
        /// Hàm unit test khi đăng nhập (đầu vào là tài khoản không hợp lệ)
        /// </summary>
        /// <returns></returns>
        /// Created BY: LQHUY(26/02/2024)
        [Test]
        public async Task LoginAsync_FailureInfoUserLogin_ThrowException()
        {
            //Arrange
            UserLogin userLogin = new UserLogin();
            var user = new User();
            user = null;
            UserRepository.GetUser(userLogin).Returns(user);

            //Action & Assert
            var exception = Assert.ThrowsAsync<ValidateException>(async () => await UserService.LoginAsync(userLogin));

            Assert.That(exception.status, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
            Assert.That(exception.Message, Is.EqualTo(Resources.ResourceVN.AccountNotCorrect));
            await UserRepository.Received(1).GetUser(userLogin);

        }
    }
}
