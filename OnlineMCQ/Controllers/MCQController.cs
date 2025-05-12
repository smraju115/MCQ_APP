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

        // প্রশ্ন তৈরি করার জন্য GET Method
        public IActionResult CreateQuestion()
        {
            return View();
        }

        // প্রশ্ন তৈরি করার জন্য POST Method
        [HttpPost]
        public IActionResult CreateQuestion(Question question)
        {
            if (ModelState.IsValid)
            {
                _context.Questions.Add(question);
                _context.SaveChanges();
                return RedirectToAction("QuestionList");
            }
            return View(question);
        }

        // প্রশ্নের তালিকা দেখানোর জন্য
        public IActionResult QuestionList()
        {
            var questions = _context.Questions.ToList();
            return View(questions);
        }

        // পরীক্ষার প্রশ্ন দেখানোর জন্য
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

        // পরীক্ষার ফলাফল সংরক্ষণের জন্য
        [HttpPost]
        public IActionResult SubmitTest(Dictionary<int, int> selectedAnswers, string name, string contact)
        {
            // ইউজারের তথ্য সেভ করা
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

            // ভুল প্রশ্ন সেভ করার জন্য লিস্ট
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
                    wrongQuestions.Add(question); // উত্তরই দেয়নি
                }
            }

            // পরীক্ষার ফলাফল সেভ করা
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

            // ভুল প্রশ্ন ও উত্তর TempData তে পাঠানো
            TempData["WrongQuestions"] = JsonConvert.SerializeObject(wrongQuestions);
            TempData["SelectedAnswers"] = JsonConvert.SerializeObject(selectedAnswers);

            // ফলাফল দেখানোর পেজে রিডাইরেক্ট
            return RedirectToAction("ViewResult", new { id = testResult.Id });
        }

        // পরীক্ষার ফলাফল দেখানোর জন্য
        public IActionResult ViewResult(int id)
        {
            var result = _context.TestResults.Include(tr => tr.UserTest).FirstOrDefault(tr => tr.Id == id);
            if (result == null) return NotFound();
            return View(result);
        }

        //  অ্যাডমিন প্যানেলে পরীক্ষার ফলাফল দেখানোর জন্য
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
