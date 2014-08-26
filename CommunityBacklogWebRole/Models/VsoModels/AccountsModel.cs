using System;

namespace CommunityBacklogWebRole.Models.VsoModels
{

    public class Accounts
    {
        public int count { get; set; }
        public Account[] value { get; set; }
    }

    public class Account
    {
        public string accountId { get; set; }
        public string accountUri { get; set; }
        public string accountName { get; set; }
        public string organizationName { get; set; }
        public string accountType { get; set; }
        public string accountOwner { get; set; }
        public string createdBy { get; set; }
        public DateTime createdDate { get; set; }
        public string accountStatus { get; set; }
        public string lastUpdatedBy { get; set; }
        public DateTime lastUpdatedDate { get; set; }
    }

}