using System.ComponentModel.DataAnnotations;

namespace Carwash.DAL.Entities
{
    public class Vehicle : Entity
    {
        [Required]
        public Service Service { get; set; }

        [Required]
        public string Owner { get; set; }

        [Required]
        [MaxLength(6)]
        public string NumbrePlate { get; set; }
    }
}
