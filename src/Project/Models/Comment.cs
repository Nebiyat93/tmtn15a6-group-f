using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Comment
    {
        public Comment() { }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Required]
        [MaxLength(400)][MinLength(10)]
        public string Text { get; set; }
        [Required]
        [Range(1, 5)]
        public int Grade { get; set; }
        [MaxLength(200)][MinLength(0)]
        public string Image { get; set; }
        [Required]
        public int Created { get; set; } 
        [Required]
        public int AccountId { get; set; }
        [Required]
        public int RecipeId { get; set; }

        public virtual Account Account { get; set; } 
        public virtual Recipe Recipe { get; set; }
    }
}
