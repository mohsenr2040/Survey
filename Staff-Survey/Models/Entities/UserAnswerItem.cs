
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Staff_Survey.Models.Entities
{
    public class UserAnswerItem
    {
        [Key]
        public int xUserAnswerId { get; set; }
        [ForeignKey("xItemId")]
        public virtual QuestionItems QuestionItems { get; set; }
        [ForeignKey("xUserId")]
        public virtual User User { get; set; }
        public string xUserId { get; set; }
        public int xItemId { get; set; }
    }
}
