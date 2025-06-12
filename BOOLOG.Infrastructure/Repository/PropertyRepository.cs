using BOOLOG.Application.Dto.PropertyDto;
using BOOLOG.Application.Interfaces.RepositoryInterfaces;
using BOOLOG.Domain.Model;
using BOOLOGAM.Infrastructure.Db_Context;
using Microsoft.EntityFrameworkCore;

namespace BOOLOG.Infrastructure.Repository
{
    public class PropertyRepository(AppDbContext DbContext) : IPropertyRepository
    {
        private readonly AppDbContext _DbContext = DbContext;

        public async Task<List<Property>> PropertyFilter(PropertyQueryDto query)
        {
            var properties = _DbContext.Properties.AsQueryable();

            if (query.Locations != null && query.Locations.Any())
            {
                properties = properties.Where(p => query.Locations.Contains(p.LocationId));
            }

            if (query.Categories != null && query.Categories.Any())
            {
                properties = properties.Where(p => query.Categories.Contains(p.CategoryId));
            }

            if (query.MaxPrice.HasValue)
            {
                properties = properties.Where(p => p.Price <= query.MaxPrice.Value);
            }

            if (query.MinPrice.HasValue)
            {
                properties = properties.Where(p => p.Price >= query.MinPrice.Value);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchText))
            {
                properties = properties.Where(p => p.Title.Contains(query.SearchText) || p.Description.Contains(query.SearchText));
            }

            return await properties.ToListAsync();
        }


    }
}
