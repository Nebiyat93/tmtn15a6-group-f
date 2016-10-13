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

        [StringLength(200, MinimumLength = 0, ErrorMessage = "TextWrongLength")]
        public string Text { get; set; }

        [Range(1, 5, ErrorMessage = "GradeWrongValue")]
        public int Grade { get; set; }

        [StringLength(200, MinimumLength = 0)]
        public string Image { get; set; }

        public int Created { get; set; }
        public string CommenterId { get; set; }
        public int RecipeId { get; set; }

        public virtual AccountIdentity AccountIdentity { get; set; }
        public virtual Recipe Recipe { get; set; }
    }
}
