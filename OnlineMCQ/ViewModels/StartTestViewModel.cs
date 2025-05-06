using OnlineMCQ.Models;

namespace OnlineMCQ.ViewModels
{
    public class StartTestViewModel
    {
        public List<Question> Questions { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public int ExamDuration { get; set; }

    }
}
