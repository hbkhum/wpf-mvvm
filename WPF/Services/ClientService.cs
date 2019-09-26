using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WPF.Services.Interface;

namespace WPF.Services
{
    public class ClientService<T> : IClientService<T> where T : class
    {
        private HttpClient _client;

        public HttpClient Client
        {
            get => _client;
            set => _client = value;
        }

        public ClientService()
        {

        }

        public void Initialize()
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /*public void Token()
        {
            var u = new Uri(Directory.GetCurrentDirectory());
            var token = Application.GetCookie(u);
            var jsonToken = JObject.Parse(token)["token"];
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", jsonToken.ToString());
        }*/

        public async Task<HttpResponseMessage> CreateEntityAsync(T entity, string url = "")
        {
            if (url == "") url = typeof(T).Name;
            Initialize();
            //Token();
            var myContent = JsonConvert.SerializeObject(entity, Formatting.Indented);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await _client.PostAsync(url + "/", byteContent);
            return response;
        }
        public async Task<HttpResponseMessage> UpdateEntityAsync(T entity, string url = "")
        {
            if (url == "") url = typeof(T).Name;
            Initialize();
            //Token();
            var myContent = JsonConvert.SerializeObject(entity, Formatting.Indented);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await _client.PutAsync(url + "/", byteContent);
            return response;
        }

        public async Task<HttpResponseMessage> LoginAsync(T entity, string url = "")
        {
            if (url == "") url = typeof(T).Name;
            Initialize();
            var myContent = JsonConvert.SerializeObject(entity, Formatting.Indented);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await _client.PostAsync(url + "/", byteContent);
            return response;
        }
        public async Task<HttpResponseMessage> RoleAsync(string url = "")
        {
            if (url == "") url = typeof(T).Name;
            Initialize();
            //Token();
            var response = await _client.GetAsync(url + "/");
            return response;
        }

        public async Task<HttpResponseMessage> GetAll(string whereValue, string orderBy, string url = "")
        {
            if (url == "") url = typeof(T).Name;
            Initialize();
            //Token();
            var response = await _client.GetAsync(url + "/");
            return response;
        }
        public async Task<HttpResponseMessage> GetById(Guid? id, string url = "")
        {
            if (url == "") url = typeof(T).Name;
            Initialize();
            //Token();
            url += "/" + id.ToString();
            var response = await _client.GetAsync(url);
            return response;
        }


    }
}