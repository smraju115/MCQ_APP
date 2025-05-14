namespace OnlineMCQ.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalQuestions { get; set; }
        public int TotalTestTakers { get; set; }
        public List<DailyTestInfo> DailyTestInfos { get; set; }
    }
}
