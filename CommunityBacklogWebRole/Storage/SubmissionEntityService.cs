using System.ComponentModel.DataAnnotations;
using CommunityBacklogWebRole.Models;
using Microsoft.WindowsAzure.Storage.Table;

namespace CommunityBacklogWebRole.Storage
{
    public class SubmissionEntityService : TableStorageServiceBase
    {
        private CloudTable _table;
        private const string EntryTableName = "BacklogEntries";

        public SubmissionEntityService()
            : base(EntryTableName)
        {
            var tableClient = GetTableClient();
            _table = tableClient.GetTableReference(EntryTableName);
        }

        public SubmissionTableEntity Add(string urlKey, string oauthRefreshToken, SubmissionModel model)
        {
            
            _table.CreateIfNotExists();

            var submission = new SubmissionTableEntity(urlKey, oauthRefreshToken, model);

            var insertOperation = TableOperation.Insert(submission);
            _table.Execute(insertOperation);
            return submission;
        }

        public SubmissionTableEntity IncrementVote(ItemNumber whichItem, string key)
        {
            var submission = GetSubmission(key);
            if (submission != null)
            {
                if (whichItem == ItemNumber.Item1)
                    submission.Item1Votes++;
                else if (whichItem == ItemNumber.Item2)
                    submission.Item2Votes++;

                var update = TableOperation.Replace(submission);

                _table.Execute(update);
            }

            return submission;
        }

        public SubmissionTableEntity SetCreatedWorkItemFlag(string key)
        {
            var submission = GetSubmission(key);
            if (submission != null)
            {
                submission.WorkItemCreated = true;
                var update = TableOperation.Replace(submission);

                _table.Execute(update);
            }

            return submission;
        }

        public SubmissionTableEntity GetSubmission(string key)
        {
            var query = TableOperation.Retrieve<SubmissionTableEntity>("Submission", key);
            var result = _table.Execute(query);

            var submission = result.Result as SubmissionTableEntity;
            return submission;
        }

    }

    public enum ItemNumber
    {
        Item1,
        Item2
    }
}