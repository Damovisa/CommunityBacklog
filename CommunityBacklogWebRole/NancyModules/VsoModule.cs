using System.Linq;
using CommunityBacklogWebRole.VSOHelpers;
using Nancy;

namespace CommunityBacklogWebRole.NancyModules
{
    public class VsoModule : NancyModule
    {
        public VsoModule()
        {
            var token = VsoHelper.GetActiveUser();

            // get accounts
            Get["/vso/accounts/"] = _ =>
            {
                if (token != null)
                {
                    var accounts = VsoHelper.GetAccounts(token.accessToken);
                    if (accounts != null)
                        return Response.AsJson(accounts.value.Select(a => new { a.accountName }));
                }
                return Response.AsJson(new { });
            };

            // get projects for an account
            Get["/vso/projects/{account}"] = _ =>
            {
                if (token != null)
                {
                    var account = (string)_.account;
                    var projects = VsoHelper.GetProjects(account, token.accessToken);
                    if (projects != null)
                        return Response.AsJson(projects.value.Select(p => new { p.name }));
                }
                return Response.AsJson(new { });
            };

            // get work item types for an account and project
            Get["/vso/workitemtypes/{account}/{project}"] = _ =>
            {
                if (token != null)
                {
                    var account = (string)_.account;
                    var project = (string)_.project;
                    var witypes = VsoHelper.GetWorkItemTypes(account, project, token.accessToken);
                    if (witypes != null)
                    {
                        return Response.AsJson(
                            witypes.value.Select(t => new
                            {
                                t.name,
                                state = GetState(t.name)
                            }));
                    }
                }
                return Response.AsJson(new { });
            };



            Get["/vso/test/{account}/{project}"] = _ =>
            {
                if (token != null)
                {
                    var account = (string)_.account;
                    var project = (string)_.project;
                    var witypes = VsoHelper.GetWorkItemTypes(account, project, token.accessToken);
                    if (witypes != null)
                    {
                        return View["VsoTest", witypes];
                    }
                }
                return View["VsoTest", new {error = "Hmm, something bad happened :("}];
            };
            
        }

        private string GetState(string witype)
        {
            switch (witype)
            {
                case "Requirement":
                    return "Proposed";
                default:
                    return "New";
            }
        }

        
    }
}