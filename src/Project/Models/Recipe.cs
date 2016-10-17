using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace Project.Models
{
    public class Recipe
    {
        public Recipe()
        {
            Directions = new HashSet<Direction>();
            Comments = new HashSet<Comment>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "NameMissing")]
        [StringLength(70, MinimumLength = 5, ErrorMessage = "NameWrongLength")]
        public string Name { get; set; }

        
        [Required(ErrorMessage = "MissingDescription")]
        [StringLength(300, MinimumLength = 10, ErrorMessage = "DescriptionWrongLength")]
        public string Description { get; set; }

        [StringLength(200, MinimumLength = 10)]
        public string Image { get; set; }
        public int Created { get; set; }

        
        public string CreatorId {get;set;}

        public virtual ICollection<Comment> Comments { get; set; }
        [Required(ErrorMessage = "DirectionsMissing")]
        public virtual ICollection<Direction> Directions { get; set; } 
        public virtual ICollection<Favorites> Favorites { get; set; }

        public virtual AccountIdentity AccountIdentity { get; set; } 
    }
}
