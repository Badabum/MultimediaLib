using System;
using System.Net;
using System.Runtime.Serialization.Json;

namespace CommonClasses
{
    public class ImdbAPI
    {
        private string _webrequest;
        public ImdbInformation Information { get; private set; }
        public ImdbAPI(string filmTitle)
        {
            _webrequest = String.Format("http://www.omdbapi.com/?t={0}&y=&plot=full&r=json", filmTitle);
            MakeRequest();
        }

        private void MakeRequest()
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(_webrequest) as HttpWebRequest;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                            "Server error (HTTP {0}: {1}).",
                            response.StatusCode,
                            response.StatusDescription));
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ImdbInformation));
                    object objResponce = serializer.ReadObject(response.GetResponseStream());
                    ImdbInformation info = objResponce as ImdbInformation;
                    Information = info;
                }
            }
            catch (Exception ex)
            {
                Information.Response = "Can't retrieve data.";
            }
        }
    }
}
