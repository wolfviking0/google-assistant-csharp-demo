using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace GAssistant.Device
{
    public class DeviceInterface
    {
        private string apiEndpoint;
        private string accessToken;

        public DeviceInterface(string apiEndpoint, string accessToken)
        {
            this.accessToken = accessToken;
            this.apiEndpoint = apiEndpoint;
        }

        public DeviceModel RegisterModel(string projectId, DeviceModel deviceModel)
        {
            try
            {
                string url = string.Format("{0}v1alpha2/projects/{1}/deviceModels/", apiEndpoint, projectId);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + accessToken);

                string body = JsonConvert.SerializeObject(deviceModel);
                var data = Encoding.ASCII.GetBytes(body);

                request.Method = "POST";
                request.ContentType = "application/json";
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
                Logger.Get().Error("Error during RegisterModel : " + ex);
                return null;
            }
        }

        public DeviceDesc RegisterDevice(string projectId, DeviceDesc device)
        {
            try
            {
                string url = string.Format("{0}v1alpha2/projects/{1}/devices/", apiEndpoint, projectId);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + accessToken);

                string body = JsonConvert.SerializeObject(device);
                var data = Encoding.ASCII.GetBytes(body);

                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return JsonConvert.DeserializeObject<DeviceDesc>(responseString);
            }

            catch (WebException ex)
            {
                Logger.Get().Error("Error during RegisterDevice : " + ex);
                return null;
            }
        }
    }
}
