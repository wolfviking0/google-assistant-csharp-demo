using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace googleassistantcsharpdemo.device
{
    public class DeviceInterface
    {
        private HttpWebRequest request;

        public DeviceInterface(string apiEndpoint, string accessToken)
        {
            request = (HttpWebRequest)WebRequest.Create(apiEndpoint);
            request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + accessToken);
        }
    
        DeviceModel registerModel() {
            try
            {
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
                return JsonConvert.DeserializeObject<DeviceModel>(responseString);
            }

            catch (WebException ex)
            {
                Logger.Get().Error("Error during registerModel" + ex);
                return null;
            }
        }

        //@POST("v1alpha2/projects/{project_id}/deviceModels/")
        //Call<DeviceModel> registerModel(@Path("project_id") String projectId, @Body DeviceModel deviceModel);

        //@POST("v1alpha2/projects/{project_id}/devices/")
        //Call<Device> registerDevice(@Path("project_id") String projectId, @Body Device device);
    }
}
