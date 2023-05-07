using Carwash.DAL.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Carwash.Models
{
    public class ServiceViewModel : Entity
    {

        [Display(Name = "Servicio")]
        [Required(ErrorMessage = "Por favor seleccione un servicio")]
        public Guid ServiceId { get; set; }

        public IEnumerable<SelectListItem> Services { get; set; }

        [Display(Name ="Nombre del propietario")]
        [Required(ErrorMessage ="Por favor digite su nombre.")]
        public string Owner { get; set; }

        [Display(Name = "Placa")]
        [Required(ErrorMessage ="Por favor ingrese la placa")]
        [MinLength(6)]
        [MaxLength(6)]
        public string NumbrePlate { get; set; }
    }
}
