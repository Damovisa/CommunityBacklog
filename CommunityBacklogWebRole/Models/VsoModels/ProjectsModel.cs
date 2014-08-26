namespace CommunityBacklogWebRole.Models.VsoModels
{

    public class Projects
    {
        public Project[] value { get; set; }
        public int count { get; set; }
    }

    public class Project
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string description { get; set; }
        public ProjectCollection collection { get; set; }
        public DefaultTeam defaultTeam { get; set; }
    }

    public class ProjectCollection
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string collectionUrl { get; set; }
    }

    public class DefaultTeam
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

}