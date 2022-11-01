using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using Wishlist.Shared.Extensions;
using Wishlist.Shared.Models.User;

namespace Wishlist.Server.Models
{
    public static class Notifier
    {
        public static string DeveloperEmail() => Environment.GetEnvironmentVariable("WISHLIST_DEVELOPER_EMAIL");

        public static string UserBlock(ApplicationUser user)
        {
            return $"\r\n\r\nName: {user.FirstName + " " + user.LastName}\r\nUsername: {user.UserName}\r\n\r\nEmail: {user.Email}";
        }

        public static IRestResponse UserEmailUpdated(ApplicationUser user)
        {
            var userBlock = UserBlock(user);
            return SendMail(null,
                            "User Email Updated!",
                            $"UPDATED email..." + userBlock);
        }

        public static IRestResponse UserPasswordReset(ApplicationUser person, Guid guid, DateTime timeStamp)
        {
            var template_variable_password = new KeyValuePair<string, string>("TEMP_PASSWORD", guid.ToString());
            var template_variable_first_name = new KeyValuePair<string, string>("FIRST_NAME", person.FirstName);
            var template_variable_time = new KeyValuePair<string, string>("TIME_LIMIT", timeStamp.ToString());
            return SendMail(
                toEmail: person.Email,
                subject: "Wishlist: Password Reset",
                templateName: "password_reset",
                templateParameters: new List<KeyValuePair<string, string>> {
                    template_variable_password,
                    template_variable_first_name,
                    template_variable_time 
                }
            );
        }

        public static IRestResponse NewUserAdded(ApplicationUser user, string adminAdding = null)
        {
            var templateList = new List<KeyValuePair<string, string>>();

            var additionalDetails = "Added at " + DateTime.Now.SetKindUtc();
            if (!string.IsNullOrEmpty(adminAdding))
            {
                additionalDetails += " by admin [" + adminAdding + "]";
            }
            templateList.Add(new KeyValuePair<string, string>("ADDITIONAL_DETAILS", additionalDetails));
            templateList.Add(new KeyValuePair<string, string>("USER_FULL_NAME", user.DisplayFullName()));
            templateList.Add(new KeyValuePair<string, string>("USER_NAME", user.UserName));
            templateList.Add(new KeyValuePair<string, string>("USER_EMAIL", user.Email));

            var result = SendMail(
                subject: "Wishlist: New User",
                templateName: "new_user_added",
                templateParameters: templateList
             );
            return result;
        }
        public static IRestResponse SendMail(string toEmail = null, string subject = null, string body = null,
            string templateName = null, List<KeyValuePair<string, string>> templateParameters = null)
        {
            toEmail = string.IsNullOrEmpty(toEmail) ? DeveloperEmail() : toEmail;
            if (string.IsNullOrEmpty(toEmail))
            {
                return new RestResponse
                {
                    ErrorException = new ArgumentNullException(nameof(toEmail),
                    "Email requires a TO field to know where to send it or a developer email " +
                    "environment variable to notify this user of key developments.")
                };
            }
            try
            {
                RestClient client = new();
                client.BaseUrl = new Uri(ResetPassword.MailApiUrl);
                client.Authenticator = new HttpBasicAuthenticator("api", ResetPassword.MailApiKey());
                RestRequest request = new();
                request.AddParameter("domain", ResetPassword.MailDomainName(), ParameterType.UrlSegment);
                request.Resource = "{domain}/messages";
                request.AddParameter("from", $"Wishlist Web <app201235650@{ResetPassword.MailDomainName()}>");
                request.AddParameter("to", toEmail);
                request.AddParameter("subject", subject);
                if (!string.IsNullOrEmpty(body))
                {
                    request.AddParameter("text", body);
                }
                if (!string.IsNullOrEmpty(templateName))
                {
                    request.AddParameter("template", templateName);
                    if (templateParameters != null)
                    {
                        foreach (var param in templateParameters)
                        {
                            request.AddParameter($"v:{param.Key}", $"{param.Value}");
                        }
                    }
                }
                request.Method = Method.POST;
                var response = client.Execute(request);
                if (response.IsSuccessful)
                {
                    return response;
                }
                else
                {
                    return new RestResponse
                    {
                        ErrorException = new Exception(response.StatusDescription),
                        ErrorMessage = response.Content
                    };
                }
            }
            catch (Exception ex)
            {
                return new RestResponse
                {
                    ErrorException = ex,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}