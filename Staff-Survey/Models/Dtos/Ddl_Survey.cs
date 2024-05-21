using Microsoft.AspNetCore.Mvc.Rendering;

namespace Staff_Survey.Models.Dtos
{
    public class Ddl_Survey
    {
        public int Id { get; set; }
        public List<SelectListItem> Select { set; get; }
    }
}
