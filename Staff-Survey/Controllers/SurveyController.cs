using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Staff_Survey.Models.Dtos;
using Staff_Survey.Models.Entities;
using Staff_Survey.Models.Interfaces;
using Staff_Survey.ViewModel;

namespace Staff_Survey.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SurveyController : Controller
    {
        private readonly ISurveyService _surveyService;
        public SurveyController(ISurveyService surveyService)
        {
                _surveyService = surveyService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
		public IActionResult CreateSurvey(SurveyViewModel model )
		{
            model = new SurveyViewModel()
            {
                Surveys = _surveyService.GetAllSurveys()
            };
			return View(model);
		}

		[HttpPost]
        public IActionResult CreateSurvey(string SurveyName)
        {
            return Json( _surveyService.CreateSurvey(SurveyName));
        }


        [HttpGet]
        public IActionResult CreateQuestions(QuestionViewModel model,int? surveyId)
        {
            if(TempData["SurveyId"] != null)
            {
                surveyId = int.Parse(TempData["SurveyId"].ToString());
            }
             var serveies = _surveyService.GetAllSurveys();

            List<SelectListItem> SelectList = new List<SelectListItem>();

            foreach (var item in serveies)
            {
                SelectList.Add(new SelectListItem { Text = item.name, Value = item.Id.ToString() }) ;
            }

            Ddl_Survey ddl_Survey = new Ddl_Survey()
            {
                Select = SelectList
            };

            if (surveyId == null)
            {
                ViewData["vm.Select"] = new SelectList(ddl_Survey.Select, "Value", "Text");
            }
            else
            {
                ViewData["vm.Select"] = new SelectList(ddl_Survey.Select, "Value", "Text", surveyId);
            }

            model = new QuestionViewModel()
            {
                _surveyId = surveyId,
                Questions = _surveyService.GetQuestionsWithItems(surveyId)
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult CreateQuestions(QuestionDto request , List<ItemsDto> Items)
        {
            ViewBag.SurveyId = request.surveyId;
            TempData["SurveyId"] = request.surveyId;
            request.Items = Items;
            return Json( _surveyService.CreateQuestios(request));
        }

        [HttpPost]
        public IActionResult DeleteQuestion(int questionId)
        {
            return Json(_surveyService.DeleteQuestion(questionId));
        }
        [HttpPost]
        public IActionResult DeleteSurvey(int surveyId)
        {
            return Json(_surveyService.DeleteSurvey(surveyId));
        }

        [HttpPost]
        public IActionResult DisplaySurvey(int surveyId)
        {
            return Json(_surveyService.DisplaySurvey(surveyId));
        }

        public IActionResult ShowSurveyResult(SurveyViewModel model, int surveyId)
        {
            model = new SurveyViewModel()
            {
                Survey = _surveyService.GetSurveyById(surveyId, "ShowSurveyResult"),
                Surveys = null,
            };
            return View(model);
        }
    }
}
