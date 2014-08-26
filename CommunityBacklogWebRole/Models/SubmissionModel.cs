namespace CommunityBacklogWebRole.Models
{
    public class SubmissionModel
    {
        public SubmissionModel()
        {
            ItemTitle1 = string.Empty;
            ItemDesc1 = string.Empty;
            ItemTitle2 = string.Empty;
            ItemDesc2 = string.Empty;
            Threshold = 0;
            TimeLimit = TimeLimit.OneDay;
            Email = string.Empty;
            VsoAccount = string.Empty;
            TeamProject = string.Empty;
            WorkItemType = string.Empty;
        }

        public string ItemTitle1 { get; set; }
        public string ItemDesc1 { get; set; }
        public string ItemTitle2 { get; set; }
        public string ItemDesc2 { get; set; }

        public int Threshold { get; set; }
        public TimeLimit TimeLimit { get; set; }
        public string Email { get; set; }

        public string VsoAccount { get; set; }
        public string TeamProject { get; set; }
        public string WorkItemType { get; set; }
    }

    public enum TimeLimit
    {
        OneDay = 0,
        OneWeek = 1,
        FourWeeks = 2,
        ThreeMonths = 3,
        NoTimeLimit = 4
    }
}