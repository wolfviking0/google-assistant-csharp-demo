using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace GAssistant.Authentication
{
    public class OAuthClient
    {
        private string googleOAuthEndpoint;

        public OAuthClient(string googleOAuthEndpoint)
        {
            this.googleOAuthEndpoint = googleOAuthEndpoint;
        }

        public OAuthCredentials GetAccessToken(string code, string client_id, string client_secret, string redirect_uri, string grant_type)
        {
            try
            {
                string url = string.Format("{0}token", googleOAuthEndpoint);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                string postData = string.Format("code={0}&client_id={1}&client_secret={2}&redirect_uri={3}&grant_type={4}", HttpUtility.UrlEncode(code), client_id, client_secret, redirect_uri, grant_type);
                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return JsonConvert.DeserializeObject<OAuthCredentials>(responseString);
            }

            catch (WebException ex)
            {
                Logger.Get().Error("Error during GetAccessToken : " + ex);
                return null;
            }
        }

        public OAuthCredentials RefreshAccessToken(string refresh_token, string client_id, string client_secret, string grant_type)
        {
            try
            {
                string url = string.Format("{0}token", googleOAuthEndpoint);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                string postData = string.Format("refresh_token={0}&client_id={1}&client_secret={2}&grant_type={3}", refresh_token, client_id, client_secret, grant_type);
                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return JsonConvert.DeserializeObject<OAuthCredentials>(responseString);
            }

            catch (WebException ex)
            {
                Logger.Get().Error("Error during RefreshAccessToken : " + ex);
                return null;
            }
        }
    }
}
