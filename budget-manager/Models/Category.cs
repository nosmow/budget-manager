using System.ComponentModel.DataAnnotations;

namespace budget_manager.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 50, ErrorMessage = "No puede ser mayor a {1} caracteres")]
        public string Name { get; set; }
        [Display(Name = "Tipo Operación")]
        public TypeOperation type_operation_id { get; set; }
        public int UserId { get; set; }
    }
}
