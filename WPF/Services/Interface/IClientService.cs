using System.Net.Http;
using System.Threading.Tasks;

namespace WPF.Services.Interface
{
    public interface IClientService<in T>
    {
        void Initialize();
        //void Token();
        Task<HttpResponseMessage> CreateEntityAsync(T entity, string url = "");
        Task<HttpResponseMessage> UpdateEntityAsync(T entity, string url = "");
        Task<HttpResponseMessage> LoginAsync(T entity, string url = "");
    }
}