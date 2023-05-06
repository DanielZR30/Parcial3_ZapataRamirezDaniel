using System.Data.SqlTypes;

namespace Carwash.DAL.Entities
{
    public class Service : Entity
    {

        public ICollection<Vehicle> Vehicles { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
