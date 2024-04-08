using MISA.AMIS.Core.Entities;
using MISA.AMIS.Core.Interfaces.Infrastructures;
using MISA.AMIS.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.Services
{
    public class PositionService : BaseService<Position>, IPositonService
    {
        public PositionService(IBaseRepository<Position> baseRepository) : base(baseRepository)
        {
        }
    }
}
