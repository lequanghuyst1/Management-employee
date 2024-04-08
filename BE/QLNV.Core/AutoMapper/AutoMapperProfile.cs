using AutoMapper;
using MISA.AMIS.Core.DTOs.CustomerDTO;
using MISA.AMIS.Core.DTOs.EmployeeDTO;
using MISA.AMIS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.AutoMapper
{
    /// <summary>
    /// Class AutoMapperProfile
    /// </summary>
    /// Created By: LQHUY(16/01/2024)
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CustomerImport, Customer>();
            CreateMap<EmployeeImportDto, Employee>();
        }
    }
}
