using API.Helpers;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace API.Extensions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response, PaginationHeader header)
        {
            var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
            response.Headers.Add("Pagination", JsonSerializer.Serialize(header, jsonOptions));       //this is a custom header
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");                     //for access and expose custom headers
        }
    }
}
