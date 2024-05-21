using Staff_Survey.Models.Dtos;

namespace Staff_Survey.ViewModel
{
    public class QuestionViewModel
    {
        public int? _surveyId { get; set; }
        public List<QuestionDto> Questions { get; set; }
    }
}
