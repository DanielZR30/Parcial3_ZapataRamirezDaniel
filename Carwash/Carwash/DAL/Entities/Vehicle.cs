using System.ComponentModel.DataAnnotations;

namespace Carwash.DAL.Entities
{
    public class Vehicle : Entity
    {
        [Required]
        public Service Service { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string Owner { get; set; }

        [Required]
        [MaxLength(6)]
        [Display(Name = "Placa")]
        public string NumbrePlate { get; set; }
    }
}
