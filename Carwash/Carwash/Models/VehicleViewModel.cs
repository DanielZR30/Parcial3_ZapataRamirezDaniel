using Carwash.DAL.Entities;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Carwash.Models
{
    public class VehicleViewModel : Vehicle
    {
        public Guid ServiceId { get; set; }
        [Display(Name = "Servicio")]
        public string ServiceName { get; set; }

        [Display(Name = "Precio")]
        public decimal Price { get; set; }
        public Guid VehicleDetailId { get; set; }

        [Display(Name = "Fecha de creación")]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "Fecha de Entrega")]
        public DateTime? DeliveryDate { get; set; }

    }
}
