using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Project.Models
{
    public class Direction
    {
        public Direction() { }
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "DirectionOrderMissing")]
        public int Order { get; set; }

        [Required(ErrorMessage = "DirectionDescriptionMissing")]
        [StringLength(120, MinimumLength = 5, ErrorMessage = "DirectionDescriptionWrongLength")]
        public string Description { get; set; }
        public int? RecipeId { get; set; }
        public virtual Recipe Recipe { get; set; } 

    }


}
