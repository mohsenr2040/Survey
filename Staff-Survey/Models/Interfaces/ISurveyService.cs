using Microsoft.EntityFrameworkCore;
using Staff_Survey.Common;
using Staff_Survey.Models.Dtos;
using Staff_Survey.Models.Entities;
using Staff_Survey.Utilities;
using System.Reflection.Metadata.Ecma335;
using System.Security.AccessControl;
using System.Security.Claims;


namespace Staff_Survey.Models.Interfaces
{
    public interface ISurveyService
    {
        ResultDto CreateSurvey(string name);
        ResultDto CreateQuestios(QuestionDto surveyQuestionDto);
        List<SurveyDto> GetAllSurveys(string ViewName);
        List<SurveyDto> GetAllSurveys();
        List<QuestionDto> GetQuestionsWithItems(int? surveyId);
        ResultDto DeleteQuestion(int questionId);
        ResultDto DeleteSurvey(int surveyId);
        ResultDto DisplaySurvey(int surveyId);
        SurveyDto GetSurveyById(int surveyId , string ViewName);
        SurveyDto GetSurveyById(int surveyId);
        ResultDto RegisterUserSurvey(List<int> itemIds,string UserId, int surveyId);

        float GetItemRank(int itemId);

    }

    public class SurveyService : ISurveyService
    {
        private readonly IDbContext _dbContext;
        public SurveyService(IDbContext dbContext)
        {
            _dbContext= dbContext;
        }

        public ResultDto CreateQuestios(QuestionDto request)
        {
            Survey survey = _dbContext.Surveys.Single(s=>s.xSurveyId==request.surveyId);
            SurveyQuestion surveyQuestion = new SurveyQuestion() 
            {
                //xSurveyId= request.surveyId,
                xQuestion= request.Question,
                Survey=survey,
            };
            _dbContext.SurveyQuestions.Add(surveyQuestion);

            foreach (var item in request.Items)
            {
                Entities.QuestionItems questionItem = new Entities.QuestionItems()
                {
                   xItem=   item.ItemValue,
                  // xQuestionId= request.QuestionId,
                    SurveyQuestion= surveyQuestion
                };
                _dbContext.QuestionItems.Add(questionItem);
            }
            _dbContext.SaveChanges();
            return new ResultDto
            {
                Message = "! سوال ثبت گردید",
                IsSuccess = true
            };
        }

        public ResultDto CreateSurvey(string name)
        {
            Survey survey=new Survey() { xSurveyName = name };
            _dbContext.Surveys.Add(survey);
            _dbContext.SaveChanges();
            return new ResultDto
            {
                Message = "! عنوان نظرسنجی ثبت گردید",
                IsSuccess = true
            };
        }

        public ResultDto DeleteQuestion(int questionId)
        {
            var _question=_dbContext.SurveyQuestions.SingleOrDefault(s => s.xQuestionId == questionId);
            var _items = _dbContext.QuestionItems.Where(i => i.xQuestionId_fk == questionId);
            _dbContext.QuestionItems.RemoveRange(_items);
            _dbContext.SurveyQuestions.Remove(_question);
            _dbContext.SaveChanges();
            return new ResultDto
            {
                IsSuccess = true,
                Message = "! سوال حذف شد"
            };
        }

        public ResultDto DeleteSurvey(int surveyId)
        {
            Survey survey = _dbContext.Surveys.Single(s => s.xSurveyId == surveyId);
            survey.IsDeleted = true;
            _dbContext.SaveChanges();
            return new ResultDto
            {
                IsSuccess = true,
                Message = "! نظرسنجی حذف شد"
            };
        }

        public ResultDto DisplaySurvey(int surveyId)
        {
            Survey survey = _dbContext.Surveys.Single(s => s.xSurveyId == surveyId);
            if (survey.IsDisplay)
                survey.IsDisplay = false;
            else
                survey.IsDisplay = true;

            _dbContext.SaveChanges();
            return new ResultDto
            {
                IsSuccess = true,
                Message = "! عملیات با موفقیت انجام شد"
            };
        }

        public List<SurveyDto> GetAllSurveys()
        {
           return _dbContext.Surveys.Select(s => new SurveyDto()
            {
                Display = s.IsDisplay,
                Id = s.xSurveyId,
                name = s.xSurveyName
            }).OrderByDescending(s=>s.Id).ToList();
            
        }
        public List<SurveyDto> GetAllSurveys(string viewname)
        {
                return _dbContext.Surveys.Select(s => new SurveyDto()
                {
                    Display = s.IsDisplay,
                    Id = s.xSurveyId,
                    name = s.xSurveyName
                }).Where(s => s.Display).OrderByDescending(s => s.Id).ToList();
        }

        public List<QuestionDto> GetQuestionsWithItems(int? surveyId)
        {
            if(surveyId==null)
            {
                return null;
            }
             var questions= _dbContext.SurveyQuestions.Include(q=>q.QuestionItems)
                                        .Where(q => q.xSurveyId_fk == surveyId)
                                        .ToList();
            List<QuestionDto> Lst = new List<QuestionDto>();
            if (questions != null)
            {
                foreach (var question in questions)
                {
                    Lst.Add(new QuestionDto()
                    {
                        QuestionId = question.xQuestionId,
                        Question = question.xQuestion,
                        Items = question.QuestionItems.Select(q => new ItemsDto() { ItemValue = q.xItem }).ToList()
                    });
                }
            }
            else
                Lst = null;


          return Lst;
              
        }

        public SurveyDto GetSurveyById(int surveyId )
        {
            if(surveyId==0)
                return null;
            var survey = _dbContext.Surveys
                                .Include(s => s.SurveyQuestions)
                                .ThenInclude(sq => sq.QuestionItems)
                               .SingleOrDefault(s => s.xSurveyId == surveyId) ;

            SurveyDto surveyDto= new SurveyDto()
            {
                Display = survey.IsDisplay,
                Id = survey.xSurveyId,
                name = survey.xSurveyName,
                Questions = survey.SurveyQuestions.
                                    Select(q => new QuestionDto()
                                    {
                                        Question = q.xQuestion,
                                        QuestionId = q.xQuestionId,
                                        Items = q.QuestionItems.
                                            Select(i => new ItemsDto()
                                            {
                                                ItemId = i.xItemId,
                                                ItemValue = i.xItem,
                                            }).ToList()
                                    }).ToList()
            };
            return surveyDto;
        }
        public SurveyDto GetSurveyById(int surveyId, string ViewName)
        {
            if (surveyId == 0)
                return null;
            var survey = _dbContext.Surveys
                                .Include(s => s.SurveyQuestions)
                                .ThenInclude(sq => sq.QuestionItems)
                               .SingleOrDefault(s => s.xSurveyId == surveyId);

            SurveyDto surveyDto = new SurveyDto()
            {
                Display = survey.IsDisplay,
                Id = survey.xSurveyId,
                name = survey.xSurveyName,
                Questions = survey.SurveyQuestions.
                                    Select(q => new QuestionDto()
                                    {
                                        Question = q.xQuestion,
                                        QuestionId = q.xQuestionId,
                                        Items = q.QuestionItems.
                                            Select(i => new ItemsDto()
                                            {
                                                ItemId = i.xItemId,
                                                ItemValue = i.xItem,
                                            }).ToList()
                                    }).ToList()
            };

            if (ViewName == "ShowSurveyResult")
            {
                foreach (var question in surveyDto.Questions)
                {
                    foreach (var item in question.Items)
                    {
                        item.ItemRannk = GetItemRank(item.ItemId);
                    }
                }
            }

            return surveyDto;
        }
        public ResultDto RegisterUserSurvey(List<int> ItemIds,string UserId, int surveyId)
        {
            var surveyDto = GetSurveyById(surveyId);
            foreach (var question in surveyDto.Questions)
            {
                bool IsAnswered = false;
                foreach (var item in question.Items)
                {

                    foreach (var id in ItemIds)
                    {
                        if(item.ItemId==id)
                        {
                            IsAnswered = true;
                            break;
                        }
                    }
                    if (IsAnswered)
                    {
                        break;
                    }

                }
                if (!IsAnswered)
                {
                    return new ResultDto()
                    {
                        IsSuccess = false,
                        Message = "همه سوال ها را باید پاسخ دهید"
                    };
                }
            }

            //UserAnswerItem answerItem = _dbContext.UserAnswerItems.FirstOrDefault(u => u.xUserId == UserId);

            int questionId = surveyDto.Questions.Select(q => q.QuestionId).First();

            List<int> Lst_items = _dbContext.QuestionItems.Where(q =>
                              q.xQuestionId_fk == questionId).Select(i => i.xItemId).ToList();
            var _lstanswer = _dbContext.UserAnswerItems.Where(u => u.xUserId == UserId).ToList();

            bool IsAnswered2 = false;
            foreach (var itemId in Lst_items)
            {
                foreach (var answer in _lstanswer)
                {
                    if(itemId==answer.xItemId)
                    {
                        IsAnswered2=true;
                        break;
                    }
                }
                if (IsAnswered2)
                {
                    break;
                }
            }
            if (IsAnswered2)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "شما قبلا این نظرسنجی را انجام داده اید"
                };
            }


            foreach (var id in ItemIds)
            {
                UserAnswerItem userItem = new UserAnswerItem()
                {
                    xItemId = id,
                    xUserId = UserId
                };
                _dbContext.UserAnswerItems.Add(userItem);
            }
            _dbContext.SaveChanges();

            return new ResultDto() { 
                IsSuccess = true,
                Message="نظرات شما ثبت گردید"
            };
        }


        public float GetItemRank(int itemId)
        {
            int rank = _dbContext.UserAnswerItems.Where(a => a.xItemId == itemId).Count();
            int questionId = _dbContext.QuestionItems.First(i => i.xItemId == itemId).xQuestionId_fk;
            List<int> Lst_Ids = _dbContext.QuestionItems.Where(q => q.xQuestionId_fk == questionId).Select(n => n.xItemId).ToList();

            int userCount = 0;
            foreach (var id in Lst_Ids)
            {
                userCount = userCount + _dbContext.UserAnswerItems.Where(a => a.xItemId == id).Count();
            }
            if (userCount == 0)
                return 0;
            else
                return ((float)rank / (float)userCount)*100;

        }
    }
}
