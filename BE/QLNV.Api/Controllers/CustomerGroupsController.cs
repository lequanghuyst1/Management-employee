using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.Core.Entities;
using MISA.AMIS.Core.Interfaces.Infrastructures;
using MISA.AMIS.Core.Interfaces.Services;

namespace MISA.AMIS.Api.Controllers
{
    [ApiController]
    public class CustomerGroupsController : MISABaseController<CustomerGroup>
    {
        #region Constructor
        public CustomerGroupsController(IBaseRepository<CustomerGroup> baseRepository, IBaseService<CustomerGroup> baseService) : base(baseRepository, baseService)
        {
        } 
        #endregion
    }
}
