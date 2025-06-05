using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OnlineMCQ.Data;
using OnlineMCQ.Models;
using OnlineMCQ.ViewModels;
using X.PagedList.Extensions;

namespace OnlineMCQ.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]
   // [EnableRateLimiting("ExamLimiter")]
   //controller level
    public class MCQController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MCQController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult CreateQuestion()
        {
            ViewBag.Modules = new SelectList(_context.Subjects.ToList(), "SubjectId", "SubjectName");
            ViewBag.Classes = new SelectList(_context.Levels.ToList(), "LevelId", "LevelName");
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
            ViewBag.Modules = new SelectList(_context.Subjects.ToList(), "SubjectId", "SubjectName");
            ViewBag.Classes = new SelectList(_context.Levels.ToList(), "LevelId", "LevelName");
            return View(question);
        }

        // GET: All Questions
        //[HttpGet]
        //public IActionResult QuestionList(int? subjectId, int? levelId, int page = 1, int pageSize = 5)
        //{
        //   // int pageSize = 2; // 

        //    var questions = _context.Questions
        //        .Include(q => q.Subject)
        //        .Include(q => q.Level)
        //        .AsQueryable();

        //    if (subjectId.HasValue && subjectId.Value != 0)
        //        questions = questions.Where(q => q.SubjectId == subjectId.Value);

        //    if (levelId.HasValue && levelId.Value != 0)
        //        questions = questions.Where(q => q.LevelId == levelId.Value);

        //    ViewBag.SelectedSubjectId = subjectId ?? 0;
        //    ViewBag.SelectedLevelId = levelId ?? 0;

        //    ViewBag.Subjects = _context.Subjects
        //        .Select(s => new { s.SubjectId, s.SubjectName })
        //        .ToList();

        //    ViewBag.Levels = _context.Levels
        //        .Select(l => new { l.LevelId, l.LevelName })
        //        .ToList();

        //    var pagedQuestions = questions.OrderByDescending(q => q.QuestionId).ToPagedList(page, pageSize);

        //    return View(pagedQuestions);

        //}


        [HttpGet]
        public IActionResult QuestionList(int? subjectId, int? levelId, int page = 1, int pageSize = 5)
        {
            var questions = _context.Questions
                .Include(q => q.Subject)
                .Include(q => q.Level)
                .AsQueryable();

            // ফিল্টার থাকলে apply করবো
            if (subjectId.HasValue && subjectId.Value != 0)
                questions = questions.Where(q => q.SubjectId == subjectId.Value);

            if (levelId.HasValue && levelId.Value != 0)
                questions = questions.Where(q => q.LevelId == levelId.Value);

            // ViewBag ডেটা ফিল্টার ড্রপডাউনের জন্য
            ViewBag.Subjects = _context.Subjects.Select(s => new { s.SubjectId, s.SubjectName }).ToList();
            ViewBag.Levels = _context.Levels.Select(l => new { l.LevelId, l.LevelName }).ToList();

            ViewBag.SelectedSubjectId = subjectId ?? 0;
            ViewBag.SelectedLevelId = levelId ?? 0;

            var pagedQuestions = questions.OrderByDescending(q => q.QuestionId).ToPagedList(page, pageSize);

            return View(pagedQuestions);
        }

        //// For Full Page Load
        //[HttpGet]
        //public IActionResult QuestionList()
        //{
        //    ViewBag.Subjects = _context.Subjects.Select(s => new { s.SubjectId, s.SubjectName }).ToList();
        //    ViewBag.Levels = _context.Levels.Select(l => new { l.LevelId, l.LevelName }).ToList();

        //    return View();
        //}

        // For AJAX Filter Request
        [HttpGet]
        public IActionResult FilteredQuestions(int? subjectId, int? levelId, int page = 1, int pageSize = 5)
        {
            var questions = _context.Questions
                .Include(q => q.Subject)
                .Include(q => q.Level)
                .AsQueryable();

            if (subjectId.HasValue && subjectId.Value != 0)
                questions = questions.Where(q => q.SubjectId == subjectId.Value);

            if (levelId.HasValue && levelId.Value != 0)
                questions = questions.Where(q => q.LevelId == levelId.Value);

            var pagedQuestions = questions
                .OrderByDescending(q => q.QuestionId)
                .ToPagedList(page, pageSize);

            ViewBag.SelectedSubjectId = subjectId ?? 0;
            ViewBag.SelectedLevelId = levelId ?? 0;

            return PartialView("_QuestionTablePartial", pagedQuestions);
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
        public IActionResult StartTestSelection()
        {
            var combinations = _context.Questions
                .Include(q => q.Subject)
                .Include(q => q.Level)
                .Where(q => q.Subject != null && q.Level != null)
                .GroupBy(q => new { q.SubjectId, q.LevelId, q.Subject.SubjectName, q.Level.LevelName })
                .Select(g => new SubjectLevelComboViewModel
                {
                    SubjectId = g.Key.SubjectId,
                    LevelId = g.Key.LevelId,
                    SubjectName = g.Key.SubjectName,
                    LevelName = g.Key.LevelName
                }).ToList();

            return View(combinations);
        }


        [AllowAnonymous]
        //[EnableRateLimiting("ExamLimiter")]
        public IActionResult StartTest(int subjectId, int levelId)
        {

            var questions = _context.Questions
                           .Where(q => q.SubjectId == subjectId && q.LevelId == levelId)
                           .OrderBy(q => Guid.NewGuid()) // Random order
                           .ToList();

            var setting = _context.ExamSettings.FirstOrDefault();
            int examDuration = setting?.DurationInMinutes ?? 10;

            var viewModel = new StartTestViewModel
            {
                Questions = questions,
                ExamDuration = examDuration,
                SubjectId = subjectId,
                LevelId = levelId
            };

            return View(viewModel);

            //var questions = _context.Questions.OrderBy(q => Guid.NewGuid()).ToList();
            //var setting = _context.ExamSettings.FirstOrDefault();
            //int examDuration = setting?.DurationInMinutes ?? 10;

            //var viewModel = new StartTestViewModel
            //{
            //    Questions = questions,
            //    ExamDuration = examDuration
            //};

            //return View(viewModel);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken] //CSRF
      
        public IActionResult SubmitTest(Dictionary<int, int> selectedAnswers, string name, string contact, int subjectId, int levelId)
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

            // প্রশ্ন ফিল্টার করা ঐ সাবজেক্ট ও লেভেলের
            var questions = _context.Questions
                .Where(q => q.SubjectId == subjectId && q.LevelId == levelId)
                .ToList();

            int totalQuestions = questions.Count;
            int correctAnswers = 0;
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

            TempData["WrongQuestions"] = JsonConvert.SerializeObject(wrongQuestions);
            TempData["SelectedAnswers"] = JsonConvert.SerializeObject(selectedAnswers);

            return RedirectToAction("ViewResult", new { id = testResult.Id });
        }

        //public IActionResult SubmitTest(Dictionary<int, int> selectedAnswers, string name, string contact)
        //{
        //    // Save User Information
        //    var userTest = new UserTest
        //    {
        //        Name = name,
        //        Contact = contact,
        //        TestDate = DateTime.Now
        //    };
        //    _context.UserTests.Add(userTest);
        //    _context.SaveChanges();

        //    var questions = _context.Questions.ToList();
        //    int totalQuestions = questions.Count;
        //    int correctAnswers = 0;

        //    // Save for Only Wrong Answer
        //    List<Question> wrongQuestions = new List<Question>();

        //    foreach (var question in questions)
        //    {
        //        if (selectedAnswers.ContainsKey(question.QuestionId))
        //        {
        //            int selected = selectedAnswers[question.QuestionId];
        //            if (selected == question.CorrectAnswer)
        //            {
        //                correctAnswers++;
        //            }
        //            else
        //            {
        //                wrongQuestions.Add(question);
        //            }
        //        }
        //        else
        //        {
        //            wrongQuestions.Add(question); 
        //        }
        //    }

        //    // Saving Exam Result
        //    var testResult = new TestResult
        //    {
        //        UserTestId = userTest.Id,
        //        TotalQuestions = totalQuestions,
        //        CorrectAnswers = correctAnswers,
        //        WrongAnswers = totalQuestions - correctAnswers,
        //        SubmittedAt = DateTime.Now
        //    };

        //    _context.TestResults.Add(testResult);
        //    _context.SaveChanges();

        //    // Wrong Question Store and Showing
        //    TempData["WrongQuestions"] = JsonConvert.SerializeObject(wrongQuestions);
        //    TempData["SelectedAnswers"] = JsonConvert.SerializeObject(selectedAnswers);


        //    return RedirectToAction("ViewResult", new { id = testResult.Id });
        //}

        [AllowAnonymous]
        // Exam Result Showing
        public IActionResult ViewResult(int id)
        {
            var result = _context.TestResults.Include(tr => tr.UserTest).FirstOrDefault(tr => tr.Id == id);
            if (result == null) return NotFound();
            return View(result);
        }

        // View Result For Admin Only 
        public IActionResult AdminResults(int page = 1, DateTime? fromDate = null, DateTime? toDate = null)
        {
            int pageSize = 10;
            var query = _context.TestResults.Include(tr => tr.UserTest).AsQueryable();

            // Date Filter
            if (fromDate.HasValue && toDate.HasValue)
            {
                query = query.Where(x => x.SubmittedAt.Date >= fromDate.Value.Date && x.SubmittedAt.Date <= toDate.Value.Date);
            }

            int totalItems = query.Count();
            var results = query
                .OrderByDescending(r => r.SubmittedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");

            return View(results);
        }

        //Delete Multiple Result
        [HttpPost]
        public IActionResult DeleteSelectedResults(int[] selectedResultIds)
        {
            if (selectedResultIds != null && selectedResultIds.Length > 0)
            {
                var resultsToDelete = _context.TestResults
                                                .Where(r => selectedResultIds.Contains(r.Id))
                                                .ToList();

                _context.TestResults.RemoveRange(resultsToDelete);
                _context.SaveChanges();
            }

            TempData["Message"] = "Selected result deleted successfully.";
            return RedirectToAction("AdminResults");
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
