using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.Core.Entities;
using MISA.AMIS.Core.Interfaces.Infrastructures;
using MISA.AMIS.Core.Interfaces.Services;

namespace MISA.AMIS.Api.Controllers
{
    [ApiController]
    public class DepartmentsController : MISABaseController<Department>
    {
        public DepartmentsController(IBaseRepository<Department> baseRepository, IBaseService<Department> baseService) : base(baseRepository, baseService)
        {
        }
    }
}
