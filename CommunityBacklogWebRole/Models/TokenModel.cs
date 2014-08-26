using System;
using CommunityBacklogWebRole.VSOHelpers;
using Newtonsoft.Json;

namespace CommunityBacklogWebRole.Models
{
    public class TokenModel
    {

        public TokenModel()
        {

        }

        [JsonProperty(PropertyName = "access_token")]
        public string accessToken { get; set; }

        [JsonProperty(PropertyName = "token_type")]
        public string tokenType { get; set; }

        [JsonProperty(PropertyName = "expires_in")]
        public string expiresIn { get; set; }

        [JsonProperty(PropertyName = "refresh_token")]
        public string refreshToken { get; set; }

        public string Error { get; set; }

        public ProfileDetails Profile { get; set; }
    }

    public class ProfileDetails
    {
        public string displayName { get; set; }
        public string publicAlias { get; set; }
        public string emailAddress { get; set; }
        public int coreRevision { get; set; }
        public DateTime timeStamp { get; set; }
        public string id { get; set; }
        public int revision { get; set; }
    }
}