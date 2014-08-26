using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using CommunityBacklogWebRole.Models;
using CommunityBacklogWebRole.Models.VsoModels;
using CommunityBacklogWebRole.Storage;
using Newtonsoft.Json;
using RestSharp;
using HttpCookie = System.Web.HttpCookie;

namespace CommunityBacklogWebRole.VSOHelpers
{
    public static class VsoHelper
    {
        private static ProfileDetails GetProfile(TokenModel token)
        {
            //https://app.vssps.visualstudio.com/_apis/profile/profiles/me
            var client = new RestClient("https://app.vssps.visualstudio.com/_apis");
            var request = new RestRequest("profile/profiles/me");
            request.AddHeader("content-type", "application/json");
            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(token.accessToken);

            var profile = client.Execute<ProfileDetails>(request);

            return profile.Data;
        }

        public static TokenModel DoAuthenticationPost(string code, bool isRenewal, bool addCookie = true)
        {
            var error = String.Empty;
            if (!String.IsNullOrEmpty(code))
            {
                var client = new RestClient(ConfigurationManager.AppSettings["TokenUrl"]);
                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddParameter("client_assertion_type", "urn:ietf:params:oauth:client-assertion-type:jwt-bearer");
                request.AddParameter("client_assertion", HttpUtility.UrlEncode(ConfigurationManager.AppSettings["AppSecret"]));
                request.AddParameter("grant_type",
                    isRenewal
                        ? "refresh_token"
                        : "urn:ietf:params:oauth:grant-type:jwt-bearer");
                request.AddParameter("assertion", HttpUtility.UrlEncode(code));
                request.AddParameter("redirect_uri", ConfigurationManager.AppSettings["RedirectUrl"]);

                try
                {

                    var res = client.Execute(request);
                    if (res.StatusCode == HttpStatusCode.OK)
                    {
                        var token = JsonConvert.DeserializeObject<TokenModel>(res.Content);

                        token.Profile = GetProfile(token);

                        if (addCookie)
                        {
                            var cookie = new HttpCookie("VsoToken")
                            {
                                Value = JsonConvert.SerializeObject(token),
                                Expires = DateTime.Now.AddYears(1),
                                Secure = true
                            };
                            HttpContext.Current.Response.Cookies.Add(cookie);
                        }
                        return token;
                    }
                    error = "<strong>Request Issue:</strong> " + (int)res.StatusCode + ": " + res.StatusDescription + "<br/>" + res.Content;
                }
                catch (WebException wex)
                {
                    error = "<strong>Request Issue:</strong> " + wex.Message + "<br/>" + wex;
                }
                catch (Exception ex)
                {
                    error = "<strong>Issue:</strong> " + ex.Message + "<br/>" + ex;
                }
            }
            else
            {
                error = "<strong>Issue:</strong> Empty authorization code";
            }

            var emptyToken = new TokenModel { Error = error };
            return emptyToken;
        }

        public static string GenerateAuthorizeUrl()
        {
            return String.Format("{0}?client_id={1}&response_type=Assertion&state={2}&scope=preview_api_all%20preview_msdn_licensing&redirect_uri={3}",
                ConfigurationManager.AppSettings["AuthUrl"],
                ConfigurationManager.AppSettings["AppId"],
                "state",
                ConfigurationManager.AppSettings["RedirectUrl"]
                );
        }

        public static TokenModel GetActiveUser()
        {
            if (HttpContext.Current.Request.Cookies.AllKeys.Contains("VsoToken"))
            {
                var code = HttpContext.Current.Request.Cookies["VsoToken"].Value;
                var token = JsonConvert.DeserializeObject<TokenModel>(code);
                return token;
            }
            return null;
        }

        /// <summary>
        /// Get all accounts for the current user
        /// </summary>
        public static Accounts GetAccounts(string accessToken)
        {
            var url = ConfigurationManager.AppSettings["AccountsUrl"];
            return GetWithBearer<Accounts>(url, accessToken).Result;
        }

        /// <summary>
        /// Get all projects for an account
        /// </summary>
        public static Projects GetProjects(string account, string accessToken)
        {
            var url = string.Format(ConfigurationManager.AppSettings["ProjectsUrl"], account);
            return GetWithBearer<Projects>(url, accessToken).Result;
        }

        /// <summary>
        /// Get all work item types - currently not working
        /// </summary>
        /// <returns></returns>
        public static WorkItemTypes GetWorkItemTypes(string account, string projectName, string accessToken)
        {
            var url = string.Format(ConfigurationManager.AppSettings["WorkItemTypesUrl"], account, projectName);
            try
            {
                return GetWithBearer<WorkItemTypes>(url, accessToken).Result;
            }
            catch (Exception ex)
            {
                return new WorkItemTypes { value = new[] { new WorkItemType { name = url + "  ---  " + ex.ToString() } } };
            }
        }

        /// <summary>
        /// Create a work item given a submission object
        /// </summary>
        public static CreatedWorkItem CreateWorkItem(SubmissionTableEntity submission, string refreshToken)
        {
            var url = string.Format(ConfigurationManager.AppSettings["CreateWorkItemUrl"], submission.VsoAccount, submission.TeamProject);

            // refresh our access token
            var newToken = DoAuthenticationPost(refreshToken, true, false);

            var workItem = WorkItemFromSubmission(submission);
            return PostWithBearer<CreatedWorkItem, NewWorkItem>(url, newToken.accessToken, workItem).Result;
        }


        private static async Task<T> GetWithBearer<T>(string url, string accessToken)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                using (var response = client.GetAsync(url).Result)
                {
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsAsync<T>();
                }
            }
        }

        private static async Task<TR> PostWithBearer<TR, T>(string url, string accessToken, T workItem)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);


                using (var response = client.PostAsJsonAsync(url, workItem).Result)
                {
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsAsync<TR>();
                }
            }
        }

        /*
         * Requirements for each Work Item Type:
         * 
         * Product Backlog Item
            { refName: "System.State" }, value: "New" },
            { refName: "System.Reason" }, value: "New backlog item" }
         * User Story
            { refName: "System.State" }, value: "New" },
            { refName: "System.Reason" }, value: "New" }
         * Requirement
            { refName: "System.State" }, value: "Proposed" },
            { refName: "System.Reason" }, value: "New" },
            { refName: "Microsoft.VSTS.Common.Priority" }, value: "X" },
            { refName: "Microsoft.VSTS.Common.Triage" }, value: "Pending" },
            { refName: "Microsoft.VSTS.CMMI.RequirementType" }, value: "XXXXXX" },
            { refName: "Microsoft.VSTS.CMMI.Committed" }, value: "No" },
            { refName: "Microsoft.VSTS.CMMI.UserAcceptanceTest" }, value: "Not Ready" }
         */

        private static NewWorkItem WorkItemFromSubmission(SubmissionTableEntity submission)
        {
            var workItem = new NewWorkItem();
            var fields = new List<Field>()
            {
                new Field
                {
                    field = new FieldName {refName = "System.WorkItemType"},
                    value = submission.WorkItemType
                },
                new Field
                {
                    field = new FieldName {refName = "System.AreaPath"},
                    value = submission.TeamProject
                },
                new Field
                {
                    field = new FieldName {refName = "System.IterationPath"},
                    value = submission.TeamProject
                },
                new Field
                {
                    field = new FieldName {refName = "System.Title"},
                    value =
                        submission.Item1Votes >= submission.Item2Votes ? submission.ItemTitle1 : submission.ItemTitle2
                },
                new Field
                {
                    field = new FieldName {refName = "System.Description"},
                    value = submission.Item1Votes >= submission.Item2Votes ? submission.ItemDesc1 : submission.ItemDesc2
                }
            };
            switch (submission.WorkItemType)
            {
                case "Product Backlog Item":
                    fields.Add(new Field
                    {
                        field = new FieldName { refName = "System.State" },
                        value = "New"
                    });
                    fields.Add(new Field
                    {
                        field = new FieldName { refName = "System.Reason" },
                        value = "New backlog item"
                    });
                    break;
                case "User Story":
                    fields.Add(new Field
                    {
                        field = new FieldName { refName = "System.State" },
                        value = "New"
                    });
                    fields.Add(new Field
                    {
                        field = new FieldName { refName = "System.Reason" },
                        value = "New"
                    });
                    break;
                case "Requirements":
                    fields.Add(new Field
                    {
                        field = new FieldName { refName = "System.State" },
                        value = "New"
                    });
                    fields.Add(new Field
                    {
                        field = new FieldName { refName = "System.Reason" },
                        value = submission.Item1Votes >= submission.Item2Votes ? submission.ItemDesc1 : submission.ItemDesc2
                    });
                    //todo: more fields required for CMMI
                    break;
            }
            workItem.fields = fields.ToArray();
            return workItem;
        }
    }

    public class OAuthPostData
    {
        public string Code { get; set; }
        public string State { get; set; }
    }

}
