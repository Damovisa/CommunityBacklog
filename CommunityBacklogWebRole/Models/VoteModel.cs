namespace CommunityBacklogWebRole.Models
{
    public class VoteModel
    {
        public string Key { get; set; }
        public string ItemTitle1 { get; set; }
        public string ItemDesc1 { get; set; }
        public string ItemTitle2 { get; set; }
        public string ItemDesc2 { get; set; }
        public int Item1Votes { get; set; }
        public int Item2Votes { get; set; }
        public int Threshold { get; set; }
    }
}