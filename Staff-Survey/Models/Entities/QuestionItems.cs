using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Staff_Survey.Models.Entities
{
    public class QuestionItems
    {
        [Key]
        public int xItemId { get; set; }
        [Required]
        public string? xItem { get; set; }
        [ForeignKey("xQuestionId_fk")]
        public virtual SurveyQuestion SurveyQuestion { get; set; }
        public int xQuestionId_fk { get; set; }
    }
}
