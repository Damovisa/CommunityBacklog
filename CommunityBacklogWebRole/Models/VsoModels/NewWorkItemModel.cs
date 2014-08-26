using System;

namespace CommunityBacklogWebRole.Models.VsoModels
{

    // post object
    public class NewWorkItem
    {
        public Field[] fields { get; set; }
    }

    public class Field
    {
        public FieldName field { get; set; }
        public object value { get; set; }
    }

    public class FieldName
    {
        public string refName { get; set; }
    }


    // response
    public class CreatedWorkItem
    {
        public string updatesUrl { get; set; }
        public int updateId { get; set; }
        public Field[] fields { get; set; }
        public Link[] links { get; set; }
        public int id { get; set; }
        public int rev { get; set; }
        public string url { get; set; }
        public string webUrl { get; set; }
    }

    public class Field1
    {
        public int id { get; set; }
        public string name { get; set; }
        public string refName { get; set; }
    }

    public class Link
    {
        public Target target { get; set; }
        public string linkType { get; set; }
        public string updateTypeExecuted { get; set; }
        public Source source { get; set; }
        public string updateType { get; set; }
        public DateTime changedDate { get; set; }
        public Changeby changeBy { get; set; }
    }

    public class Target
    {
        public int id { get; set; }
        public string url { get; set; }
        public string webUrl { get; set; }
    }

    public class Source
    {
        public int id { get; set; }
        public string url { get; set; }
        public string webUrl { get; set; }
    }

    public class Changeby
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public string uniqueName { get; set; }
        public string url { get; set; }
        public string imageUrl { get; set; }
    }

}