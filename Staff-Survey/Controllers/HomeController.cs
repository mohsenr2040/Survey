using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Staff_Survey.Common;
using Staff_Survey.Models.Dtos;
using Staff_Survey.Models.Entities;
using Staff_Survey.Models.Interfaces;
using Staff_Survey.Utilities;
using Staff_Survey.ViewModel;
using System.Diagnostics;

namespace Staff_Survey.Controllers
{
    [Authorize()]
    public class HomeController : Controller
    {
        private readonly ISurveyService _surveyService;
        public HomeController(ISurveyService surveyService)
        {
            _surveyService = surveyService;
        }
        public ActionResult Index(SurveyViewModel model)
        {
            model = new SurveyViewModel()
            {
                Surveys = _surveyService.GetAllSurveys("Index")
            };
           
            return View(model);
        }

        [HttpGet]
        public ActionResult Survey(SurveyViewModel model, int surveyId)
        {
            model = new SurveyViewModel()
            {
                Survey = _surveyService.GetSurveyById(surveyId),
                Surveys=null
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Survey(List<Item>  items ,int surveyId)
        {
            if(items.Count==0)
            {
                return Json(new ResultDto()
                {
                    IsSuccess = false,
                    Message = "همه سوال ها  را پاسخ دهید"
                }) ; 
            }
            var userId = ClaimUtility.GetUserId(User);
            return Json(_surveyService.RegisterUserSurvey(items.Select(n => int.Parse(n.itemId.ToString())).ToList(), userId, surveyId)) ;
        }

        public class Item
        {
            public string itemId { get; set; }
        }
    }
}