using Staff_Survey.Models.Entities;

namespace Staff_Survey.Models.Dtos
{

    public class SurveyDto
    {
        public int Id { get; set; }
        public string name { get; set; }
        public bool Display { get; set; }
        public List<QuestionDto> Questions { get; set; }
    }
 
}
