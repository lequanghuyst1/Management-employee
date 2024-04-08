using MISA.AMIS.Core.DTOs.CustomerDTO;
using MISA.AMIS.Core.Interfaces.Excels;
using MISA.AMIS.Core.Interfaces.Infrastructures;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MISA.AMIS.Core.Resources;
using System.ComponentModel.DataAnnotations;

namespace MISA.AMIS.Core.Excels
{
    public class BaseExcelService<T> : ExportExcelService<T>, IBaseExcelService<T> where T : class
    {
        IBaseRepository<T> _baseRepository;
        public BaseExcelService(IBaseRepository<T> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task<byte[]> ExportAllAsync()
        {
            var data = await _baseRepository.GetAllAsync();
            var bytes = ExportExcelAsync(data);
            return bytes;
        }

        public async Task<byte[]> ExportListAsync(List<Guid> ids)
        {
            var data = await _baseRepository.GetByIdsAsync(ids);
            var bytes = ExportExcelAsync(data);
            return bytes;
        }
    }
}

