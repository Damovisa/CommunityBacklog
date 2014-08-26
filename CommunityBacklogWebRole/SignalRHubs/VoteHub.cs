using System;
using System.Linq;
using System.Threading.Tasks;
using CommunityBacklogWebRole.Storage;
using CommunityBacklogWebRole.VSOHelpers;
using Microsoft.AspNet.SignalR;

namespace CommunityBacklogWebRole.SignalRHubs
{
    public class VoteHub : Hub
    {
        private readonly SubmissionEntityService _submissionEntityService;

        public VoteHub()
        {
            _submissionEntityService = new SubmissionEntityService();
        }

        public Task JoinVote(string key)
        {
            return Groups.Add(Context.ConnectionId, key);
        }

        public bool Vote(string key, int item)
        {
            try
            {
                if (key != null && new[] { 1, 2 }.Contains(item))
                {
                    var itemNumber = item == 1 ? ItemNumber.Item1 : ItemNumber.Item2;
                    var submission = _submissionEntityService.IncrementVote(itemNumber, key);
                    // tell everyone the updated count
                    Clients.Group(key).UpdateVotes(submission.Item1Votes, submission.Item2Votes);

                    // is the updated count more than the threshold? If so, create a work item
                    if (!submission.WorkItemCreated &&
                        (submission.Item1Votes >= submission.Threshold || submission.Item2Votes >= submission.Threshold))
                    {
                        VsoHelper.CreateWorkItem(submission, submission.OAuthRefreshToken);
                        _submissionEntityService.SetCreatedWorkItemFlag(key);
                    }

                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}