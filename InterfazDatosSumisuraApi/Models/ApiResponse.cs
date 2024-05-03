using System.Net;

namespace InterfazDatosSumisuraApi.Models
{
    public class ApiResponse
    {
        public ApiResponse()
        {
            IsError = new List<string>();
        }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> IsError { get; set; }
        public object Resultado { get; set; }
    }
}
