using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Recipe
    {
        public Recipe()
        {
            Directions = new List<Direction>();
            Comments = new List<Comment>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        //[Required(ErrorMessage = "NameMissing", AllowEmptyStrings = false)]
        //[StringLength(70, MinimumLength = 5, ErrorMessage = "NameWrongLength")]
        public string Name { get; set; }


        //[Required(ErrorMessage = "MissingDescription", AllowEmptyStrings = false)]
        //[StringLength(300, MinimumLength = 10, ErrorMessage = "DescriptionWrongLength")]
        public string Description { get; set; }

        //[StringLength(200, MinimumLength = 10)]
        public string Image { get; set; }
        public int Created { get; set; }

        
        public string CreatorId {get;set;}

        public virtual ICollection<Comment> Comments { get; set; }

        //[MinLength(1, ErrorMessage = "DirectionsMissing")]
        public virtual ICollection<Direction> Directions { get; set; } 
        public virtual ICollection<Favorites> Favorites { get; set; }

        public virtual AccountIdentity AccountIdentity { get; set; } 
    }
}
