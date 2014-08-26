using System.Web;
using CommunityBacklogWebRole.Models;
using CommunityBacklogWebRole.VSOHelpers;
using Nancy;
using Nancy.ModelBinding;
using Newtonsoft.Json;

namespace CommunityBacklogWebRole.NancyModules
{
    public class AuthModule : NancyModule
    {

        public AuthModule()
        {
            // callback URL from the VSO OAuth page
            Get["/oauth"] = _ =>
            {
                var oauthData = this.Bind<OAuthPostData>();
                TokenModel tokenModel = VsoHelper.DoAuthenticationPost(oauthData.Code, false);
                return View["TokenView", tokenModel];
            };

            // URL to renew or initialize OAuth
            Get["/oauth/init"] = _ =>
            {
                TokenModel token = null;
                if (Request.Cookies.ContainsKey("VsoToken"))
                {
                    var code = Request.Cookies["VsoToken"];
                    token = JsonConvert.DeserializeObject<TokenModel>(code);
                }
                else
                {
                    Response.AsRedirect("/oauth/request");
                }

                TokenModel tokenModel = VsoHelper.DoAuthenticationPost(token.refreshToken, true);

                if (!string.IsNullOrEmpty(tokenModel.Error))
                    return View["TokenView"];

                if (HttpContext.Current.Request.UrlReferrer != null)
                {
                    return Response.AsRedirect(HttpContext.Current.Request.UrlReferrer.ToString());
                }
                else
                {
                    return Response.AsRedirect("/");
                }
            };

            // Start the OAuth request process
            Get["/oauth/request"] = _ => Response.AsRedirect(VsoHelper.GenerateAuthorizeUrl());
        }
        
    }
}