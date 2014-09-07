using CommunityBacklogWebRole.Models;
using CommunityBacklogWebRole.Storage;
using Nancy;
using Nancy.Responses;

namespace CommunityBacklogWebRole.NancyModules
{
    public class VotingModule : NancyModule
    {
        private readonly SubmissionEntityService _submissionEntityService;

        public VotingModule()
        {
            _submissionEntityService = new SubmissionEntityService();

            // get the voting page
            Get["/v/{key}"] = _ =>
            {
                var key = (string) _.key;
                var submission = _submissionEntityService.GetSubmission(key);
                if (submission == null)
                {
                    return new TextResponse(
                        "Can't find a submission with that key. Please check the spelling and try again" +
                        " (note that keys are case-sensitive).") { StatusCode = HttpStatusCode.NotFound };
                }
                var model = new VoteModel
                {
                    Key = submission.RowKey,
                    ItemTitle1 = submission.ItemTitle1,
                    ItemDesc1 = submission.ItemDesc1,
                    Item1Votes = submission.Item1Votes,
                    ItemTitle2 = submission.ItemTitle2,
                    ItemDesc2 = submission.ItemDesc2,
                    Item2Votes = submission.Item2Votes,
                    Threshold = submission.Threshold
                };
                return View["Vote", model];
            };

        }
    }
}