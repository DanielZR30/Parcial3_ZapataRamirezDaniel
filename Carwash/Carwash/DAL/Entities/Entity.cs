using System.ComponentModel.DataAnnotations;

namespace Carwash.DAL.Entities
{
    public class Entity
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
    }
}
