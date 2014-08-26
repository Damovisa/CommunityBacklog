using Newtonsoft.Json;

namespace CommunityBacklogWebRole.Models.VsoModels
{
    /// <summary>
    /// Version 1.2 of the Work Item Types API - not yet released
    /// </summary>
    public class WorkItemTypes
    {
        public int count { get; set; }
        public WorkItemType[] value { get; set; }
    }

    public class WorkItemType
    {
        public string name { get; set; }
        public string description { get; set; }
        public string xmlForm { get; set; }
        public Fieldinstance[] fieldInstances { get; set; }
        public _Links[] _links { get; set; }
        public string url { get; set; }
    }

    public class Fieldinstance
    {
        public WorkItemTypeField field { get; set; }
        public string helpText { get; set; }
    }

    public class WorkItemTypeField
    {
        public string referenceName { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

    public class _Links
    {
        public string rel { get; set; }
        public string url { get; set; }
    }

}