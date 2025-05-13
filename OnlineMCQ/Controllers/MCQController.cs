using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OnlineMCQ.Data;
using OnlineMCQ.Models;
using OnlineMCQ.ViewModels;

namespace OnlineMCQ.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class MCQController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MCQController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult CreateQuestion()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateQuestion(Question question)
        {
            if (ModelState.IsValid)
            {
                _context.Questions.Add(question);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Create Question Successfully.";

                return RedirectToAction("QuestionList");
            }
            return View(question);
        }

        //Get All Questions
        public IActionResult QuestionList()
        {
            var questions = _context.Questions.ToList();
            return View(questions);
        }

        //Delete Multiple Question
        [HttpPost]
        public IActionResult DeleteSelectedQuestions(int[] selectedQuestionIds)
        {
            if (selectedQuestionIds != null && selectedQuestionIds.Length > 0)
            {
                var questionsToDelete = _context.Questions
                                                .Where(q => selectedQuestionIds.Contains(q.QuestionId))
                                                .ToList();

                _context.Questions.RemoveRange(questionsToDelete);
                _context.SaveChanges();
            }

            TempData["Message"] = "Selected questions deleted successfully.";
            return RedirectToAction("QuestionList");
        }


        //Optional: Individual Delete Method
        //[HttpPost]
        //public IActionResult DeleteQuestion(int id)
        //{
        //    var question = _context.Questions.Find(id);
        //    if (question != null)
        //    {
        //        _context.Questions.Remove(question);
        //        _context.SaveChanges();
        //    }

        //    return RedirectToAction("QuestionList");
        //}



        [AllowAnonymous]
        public IActionResult StartTest()
        {
            var questions = _context.Questions.OrderBy(q => Guid.NewGuid()).ToList();
            var setting = _context.ExamSettings.FirstOrDefault();
            int examDuration = setting?.DurationInMinutes ?? 10;

            var viewModel = new StartTestViewModel
            {
                Questions = questions,
                ExamDuration = examDuration
            };

            return View(viewModel);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult SubmitTest(Dictionary<int, int> selectedAnswers, string name, string contact)
        {
            // Save User Information
            var userTest = new UserTest
            {
                Name = name,
                Contact = contact,
                TestDate = DateTime.Now
            };
            _context.UserTests.Add(userTest);
            _context.SaveChanges();

            var questions = _context.Questions.ToList();
            int totalQuestions = questions.Count;
            int correctAnswers = 0;

            // Save for Only Wrong Answer
            List<Question> wrongQuestions = new List<Question>();

            foreach (var question in questions)
            {
                if (selectedAnswers.ContainsKey(question.QuestionId))
                {
                    int selected = selectedAnswers[question.QuestionId];
                    if (selected == question.CorrectAnswer)
                    {
                        correctAnswers++;
                    }
                    else
                    {
                        wrongQuestions.Add(question);
                    }
                }
                else
                {
                    wrongQuestions.Add(question); 
                }
            }

            // Saving Exam Result
            var testResult = new TestResult
            {
                UserTestId = userTest.Id,
                TotalQuestions = totalQuestions,
                CorrectAnswers = correctAnswers,
                WrongAnswers = totalQuestions - correctAnswers,
                SubmittedAt = DateTime.Now
            };

            _context.TestResults.Add(testResult);
            _context.SaveChanges();

            // Wrong Question Store and Showing
            TempData["WrongQuestions"] = JsonConvert.SerializeObject(wrongQuestions);
            TempData["SelectedAnswers"] = JsonConvert.SerializeObject(selectedAnswers);

            
            return RedirectToAction("ViewResult", new { id = testResult.Id });
        }

        [AllowAnonymous]
        // Exam Result Showing
        public IActionResult ViewResult(int id)
        {
            var result = _context.TestResults.Include(tr => tr.UserTest).FirstOrDefault(tr => tr.Id == id);
            if (result == null) return NotFound();
            return View(result);
        }

        // View For Admin Only Result
        public IActionResult AdminResults()
        {
            var results = _context.TestResults.Include(tr => tr.UserTest).ToList();
            return View(results);
        }


        //ExamTimeChangeMethod
        public IActionResult Index()
        {
            var setting = _context.ExamSettings.FirstOrDefault() ?? new ExamSetting { DurationInMinutes = 10 };
            return View(setting);
        }

        [HttpPost]
        public IActionResult Index(ExamSetting model)
        {
            var setting = _context.ExamSettings.FirstOrDefault();
            if (setting == null)
            {
                _context.ExamSettings.Add(model);
            }
            else
            {
                setting.DurationInMinutes = model.DurationInMinutes;
            }

            _context.SaveChanges();
            ViewBag.Message = "Exam time updated successfully.";
            return View(model);
        }


        




    }
}
