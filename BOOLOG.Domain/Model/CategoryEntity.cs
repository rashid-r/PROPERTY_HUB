
namespace BOOLOG.Domain.Model
{
    public class CategoryEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string CategoryName { get; set; }
        public ICollection<PropertyEntity> propertyEntities { get; set; }

    }
}
