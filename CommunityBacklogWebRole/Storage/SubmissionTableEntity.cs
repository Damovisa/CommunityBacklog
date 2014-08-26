using CommunityBacklogWebRole.Models;
using Microsoft.WindowsAzure.Storage.Table;

namespace CommunityBacklogWebRole.Storage
{
    public class SubmissionTableEntity : TableEntity
    {
        public SubmissionTableEntity() { }

        public SubmissionTableEntity(string urlKey, string oauthRefreshToken, SubmissionModel model)
        {
            PartitionKey = "Submission";
            RowKey = urlKey;
            ItemTitle1 = model.ItemTitle1;
            ItemDesc1 = model.ItemDesc1;
            ItemTitle2 = model.ItemTitle2;
            ItemDesc2 = model.ItemDesc2;
            Item1Votes = 0;
            Item2Votes = 0;
            Threshold = model.Threshold;
            TimeLimit = (int)model.TimeLimit;
            Email = model.Email;
            VsoAccount = model.VsoAccount;
            TeamProject = model.TeamProject;
            WorkItemType = model.WorkItemType;
            OAuthRefreshToken = oauthRefreshToken;
            WorkItemCreated = false;
        }

        public string ItemTitle1 { get; set; }
        public string ItemDesc1 { get; set; }
        public string ItemTitle2 { get; set; }
        public string ItemDesc2 { get; set; }

        public int Item1Votes { get; set; }
        public int Item2Votes { get; set; }

        public int Threshold { get; set; }
        public int TimeLimit { get; set; }
        public string Email { get; set; }

        public string VsoAccount { get; set; }
        public string TeamProject { get; set; }
        public string WorkItemType { get; set; }
        public string OAuthRefreshToken { get; set; }

        public bool WorkItemCreated { get; set; }

    }
}