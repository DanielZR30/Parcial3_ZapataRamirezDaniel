using System.ComponentModel.DataAnnotations;

namespace Carwash.DAL.Entities
{
    public class VehicleDetail: Entity
    {
        public Vehicle Vehicle { get; set; }
        
        [Display(Name = "Fecha de creación")]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "Fecha de Entrega")]
        public DateTime? DeliveryDate { get; set; }
    }
}
