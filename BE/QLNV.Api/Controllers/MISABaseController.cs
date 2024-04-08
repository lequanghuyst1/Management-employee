using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.Core.Entities;
using MISA.AMIS.Core.Interfaces.Infrastructures;
using MISA.AMIS.Core.Interfaces.Services;
using MISA.AMIS.Infrastructure.Repository;

namespace MISA.AMIS.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MISABaseController<T> : ControllerBase where T : class
    {
        #region Fields
        IBaseRepository<T> _baseRepository;
        IBaseService<T> _baseService;
        #endregion

        #region Contructor
        public MISABaseController(IBaseRepository<T> baseRepository, IBaseService<T> baseService)
        {
            _baseRepository = baseRepository;
            _baseService = baseService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy toàn bộ các bản ghi
        /// </summary>
        /// <returns>
        /// StatusCode: 200 - danh sách các đối tượng
        /// StausCode: 500 - có vấn đề trên service
        /// </returns>
        /// Created by: LQHUY(25/12/2023)
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var res = await _baseRepository.GetAllAsync();
            return Ok(res);
        }

        /// <summary>
        /// Lấy ra 1 đối tượng theo mã định danh(id)
        /// </summary>
        /// <param name="id">mã định danh của đối tượng</param>
        /// <returns>
        /// - StatusCode: 200 và chi tiết đối tượng 
        /// - StatusCode: 400 khi có vấn đề phía client
        /// - StatusCode: 500 có vấn đề trên service
        /// </returns>
        /// Created by: LQHUY(25/12/2023)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var res = await _baseRepository.GetByIdAsync(id);
            return Ok(res);
        }

        /// <summary>
        /// Lấy ra mã code mới
        /// </summary>
        /// <returns>
        /// - StatusCode: 200 và mã code mới
        /// - StatusCode: 500 có vấn đề trên service
        /// </returns>
        [HttpGet("NewCode")]
        public async Task<IActionResult> GetNewCodeAsync()
        {
            var res = await _baseRepository.GetNewCodeAsync();
            return Ok(res);
        }

        /// <summary>
        /// Thêm mới 1 đối tượng 
        /// </summary>
        /// <param name="entity">thông tin của đối tượng</param>
        /// <returns>
        /// - StatusCode: 201 và trả về số lượng bản ghi khi thêm thành công
        /// - StatusCode: 400 khi có vấn đề phía client (giá trị nhập vào không hợp lệ)
        /// - StatusCode: 409 khi một trường đã tồn tại trong hệ thống (mã của đối tượng)
        /// - StatusCode: 500 có vấn đề trên service
        /// </returns>
        /// Created by: LQHUY(25/12/2023)
        [HttpPost]
        public async Task<IActionResult> Insert(T entity)
        {
            var res = await _baseService.InsertServiceAsync(entity);

            if (res > 0)
            {
                return StatusCode(201, res);
            }
            return NoContent();
        }

        /// <summary>
        /// Sửa thông tin một đối tượng theo mã định danh(id)
        /// </summary>
        /// <param name="entity">thông tin của đối tượng đã được chỉnh sửa</param>
        /// <param name="id">mã định danh của đối tượng</param>
        /// <returns>
        /// - StatusCode: 200 và trả về số lượng bản ghi khi sửa thành công
        /// - StatusCode: 400 khi có vấn đề phía client (giá trị nhập vào không hợp lệ)
        /// - StatusCode: 409 khi một trường đã tồn tại trong hệ thống (mã của đối tượng)
        /// - StatusCode: 500 có vấn đề trên service
        /// </returns>
        /// Created by: LQHUY(25/12/2023)
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(T entity, Guid id)
        {
            var res = await _baseService.UpdateServiceAsync(entity, id);
            if (res > 0)
            {
                return StatusCode(200, res);
            }
            return NoContent();
        }

        /// <summary>
        /// Xóa 1 đối tượng theo mã định danh(id)
        /// </summary>
        /// <param name="id">mã định danh của đối tượng</param>
        /// <returns>
        /// - StatusCode: 200 và trả về số lượng bản ghi đã bị xóa
        /// - StatusCode: 400 khi có vấn đề phía client
        /// - StatusCode: 500 có vấn đề trên service
        /// </returns>
        /// Created by: LQHUY(25/12/2023)
        [HttpDelete("{entityId}")]
        public async Task<IActionResult> Delete(Guid entityId)
        {
            var res = await _baseService.DeleteServiceAsync(entityId);
            return StatusCode(200, res);

        }

        /// <summary>
        /// Xóa nhiều đối tượng theo list mã định danh(id)
        /// </summary>
        /// <param name="id">mã định danh của đối tượng</param>
        /// <returns>
        /// - StatusCode: 200 và trả về số lượng bản ghi đã bị xóa
        /// - StatusCode: 400 khi có vấn đề phía client
        /// - StatusCode: 500 có vấn đề trên service
        /// </returns>
        /// Created by: LQHUY(25/12/2023)
        [HttpDelete("DeleteMany")]
        public async Task<IActionResult> DeleteMany([FromBody] List<Guid> ids)
        {
            var res = await _baseService.DeleteManyServiceAsync(ids);
            return Ok(res);
        }
        #endregion
    }
}
