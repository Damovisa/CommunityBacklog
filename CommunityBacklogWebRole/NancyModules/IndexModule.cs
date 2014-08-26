using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommunityBacklogWebRole.Models;
using CommunityBacklogWebRole.Storage;
using CommunityBacklogWebRole.VSOHelpers;
using Nancy;
using Nancy.ModelBinding;

namespace CommunityBacklogWebRole.NancyModules
{
    public class IndexModule : Nancy.NancyModule
    {
        public IndexModule()
        {
            // Root
            Get["/"] = _ =>
            {
                // todo: redirect to init if we already have a cookie - refresh the token??
                var newModel = new SubmissionModel { Threshold = 5 };
                return View["Index.cshtml", newModel];
            };

            // About
            Get["/about"] = _ => View["About"];

            Post["/submit"] = _ =>
            {
                var saveData = this.Bind<SubmissionModel>();

                var service = new SubmissionEntityService();
                var urlKey = GetRandomString(6); // some random key
                if (VsoHelper.GetActiveUser() != null)
                {
                    var savedObject = service.Add(urlKey, VsoHelper.GetActiveUser().refreshToken, saveData);
                }
                else
                {
                    var savedObject = service.Add(urlKey, "", saveData);
                }

                return Response.AsRedirect("/v/"+urlKey);
            };

            Post["/refresh"] = _ =>
            {
                var saveData = this.Bind<SubmissionModel>();
                return View["Index", saveData];
            };
        }

        /// <summary>
        /// Random String generator from http://stackoverflow.com/questions/730268/unique-random-string-generation
        /// </summary>
        private string GetRandomString(int length, string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
        {
            if (length < 0) throw new ArgumentOutOfRangeException("length", "length cannot be less than zero.");
            if (string.IsNullOrEmpty(allowedChars)) throw new ArgumentException("allowedChars may not be empty.");

            const int byteSize = 0x100;
            var allowedCharSet = new HashSet<char>(allowedChars).ToArray();
            if (byteSize < allowedCharSet.Length) throw new ArgumentException(String.Format("allowedChars may contain no more than {0} characters.", byteSize));

            // Guid.NewGuid and System.Random are not particularly random. By using a
            // cryptographically-secure random number generator, the caller is always
            // protected, regardless of use.
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                var result = new StringBuilder();
                var buf = new byte[128];
                while (result.Length < length)
                {
                    rng.GetBytes(buf);
                    for (var i = 0; i < buf.Length && result.Length < length; ++i)
                    {
                        // Divide the byte into allowedCharSet-sized groups. If the
                        // random value falls into the last group and the last group is
                        // too small to choose from the entire allowedCharSet, ignore
                        // the value in order to avoid biasing the result.
                        var outOfRangeStart = byteSize - (byteSize % allowedCharSet.Length);
                        if (outOfRangeStart <= buf[i]) continue;
                        result.Append(allowedCharSet[buf[i] % allowedCharSet.Length]);
                    }
                }
                return result.ToString();
            }
        }
    }
}