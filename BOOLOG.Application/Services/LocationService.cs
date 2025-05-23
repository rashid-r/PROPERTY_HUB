using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BOOLOG.Application.Repository.RepositoryInterfaces;
using BOOLOG.Domain.Model;

namespace BOOLOG.Application.Services
{
    internal class LocationService
    {
        private readonly IRepository<LocationEntity> _locRepo;
        private readonly IMapper _mapper;

        public LocationService(IRepository<LocationEntity> locRepo, IMapper mapper)
        {
            _locRepo = locRepo;
            _mapper = mapper;
        }

        public async Task<>
    }
}
