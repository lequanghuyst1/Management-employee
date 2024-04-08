using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.Core.Entities;
using MISA.AMIS.Core.Interfaces.Infrastructures;
using MISA.AMIS.Core.Interfaces.Services;

namespace MISA.AMIS.Api.Controllers
{
    [ApiController]
    public class PositionsController : MISABaseController<Position>
    {
        public PositionsController(IBaseRepository<Position> baseRepository, IBaseService<Position> baseService) : base(baseRepository, baseService)
        {
        }
    }
}
