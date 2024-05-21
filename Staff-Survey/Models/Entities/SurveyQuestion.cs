using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Staff_Survey.Models.Entities
{
    public class SurveyQuestion
    {
        [Key]
        public int xQuestionId { get; set; }
        public string? xQuestion { get; set; }

        [ForeignKey("xSurveyId_fk")]
        public virtual Survey Survey { get; set; }
        public int xSurveyId_fk { get; set; }
        public List<QuestionItems> QuestionItems { get; set; }

    }
}
