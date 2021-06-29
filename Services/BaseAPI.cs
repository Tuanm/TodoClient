using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;

namespace Todo.Services {
    public class BaseAPI {
        private HttpClient httpClient;

        private string _baseAddress;
        
        public BaseAPI() {
            httpClient = new HttpClient();
        }
        
        public BaseAPI WithBaseAddress(string baseAddress) {
            _baseAddress = baseAddress;
            httpClient.BaseAddress = new System.Uri(_baseAddress);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
            );
            return this;
        }

        private System.Uri GetUri(string url, object param = null) {
            string longurl = _baseAddress + url;
            var uriBuilder = new System.UriBuilder(longurl);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            if (param != null) {
                foreach (var p in param.GetType().GetProperties()
                    .Where(p => !p.GetGetMethod().GetParameters().Any())) {
                    if (p.GetValue(param, null) != null)
                        query[p.Name] = p.GetValue(param, null).ToString();
                }
            }

            uriBuilder.Query = query.ToString();
            return new System.Uri(uriBuilder.ToString());
        }

        public T Get<T>(string url, object param = null) {
            var response = httpClient.GetAsync(GetUri(url, param)).Result;
            response.EnsureSuccessStatusCode();
            string strResponse = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(strResponse);
        }

        public T Post<T>(string url, object param = null, object body = null) {
            var json = JsonConvert.SerializeObject(body);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync(GetUri(url, param), data).Result;
            response.EnsureSuccessStatusCode();
            string strResponse = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(strResponse);
        }

        public T Put<T>(string url, object param = null, object body = null) {
            var json = JsonConvert.SerializeObject(body);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = httpClient.PutAsync(GetUri(url, param), data).Result;
            response.EnsureSuccessStatusCode();
            string strResponse = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(strResponse);
        }

        public T Delete<T>(string url, object param = null) {
            var response = httpClient.DeleteAsync(GetUri(url, param)).Result;
            response.EnsureSuccessStatusCode();
            string strResponse = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(strResponse);
        }
    }

    public class TaskParameter {

    }
}
