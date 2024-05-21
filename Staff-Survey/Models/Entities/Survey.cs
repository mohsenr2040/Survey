using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Staff_Survey.Models.Entities
{
    public class Survey
    {
        [Key]
        public int xSurveyId { get; set; }
        public string? xSurveyName { get; set; }
        public DateTime xDateInsert { get; set; } = DateTime.Now.Date;
        public bool IsDeleted { get; set; } = false;
        public bool IsDisplay { get; set; } = true;
        public List<SurveyQuestion> SurveyQuestions { get; set; }
    }
}
