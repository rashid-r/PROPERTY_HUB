using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOOLOG.Application.Dto.PropertyDto;
using BOOLOG.Domain.Model;

namespace BOOLOG.Application.Interfaces.RepositoryInterfaces
{
    public interface IPropertyRepository
    {
        Task<List<Property>> PropertyFilter(PropertyQueryDto query);
    }
}
