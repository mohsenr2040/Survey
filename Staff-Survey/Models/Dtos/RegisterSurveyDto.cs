using Staff_Survey.Models.Entities;

namespace Staff_Survey.Models.Dtos
{
    public class RegisterSurveyDto
    {
        public string Name { get; set; }
        public List<SurveyQuestion> Questions { get; set; }

        public List<QuestionItems> Items { get; set; }
    }
}
