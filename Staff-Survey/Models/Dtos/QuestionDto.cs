namespace Staff_Survey.Models.Dtos
{
    public class QuestionDto
    {
        public int surveyId { get; set; }
        public int QuestionId { get; set; }
        public string Question { get; set; }
        public List<ItemsDto> Items { get; set; }
    }
 
}
