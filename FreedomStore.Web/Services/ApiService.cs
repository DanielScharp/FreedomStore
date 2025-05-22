using System.Text.Json;
using System.Text;
using FreedomStore.Web.Models;
using Newtonsoft.Json;
using System.Net;

namespace FreedomStore.Web.Services
{
    public class ApiService :IApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("ApiClient");
        }

        public async Task<ApiResponse> ExecuteRequestAsync(ApiRequest request)
        {
            try
            {
                // Monta a URL com os parâmetros de query string (se existirem)
                var url = request.Route;

                if(request.QueryParams != null)
                {
                    var queryString = BuildQueryString(request.QueryParams);
                    if(!string.IsNullOrEmpty(queryString))
                    {
                        url += "?" + queryString;
                    }
                }

                // Cria a requisição HTTP
                var httpRequest = new HttpRequestMessage(request.Method, url);

                var token = ClientSession.Logado.Token; // Obtém o token do cliente logado
                if(!string.IsNullOrEmpty(token))
                {
                    httpRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                // Adiciona o corpo, se houver
                if(request.Body != null)
                {
                    var json = System.Text.Json.JsonSerializer.Serialize(request.Body);
                    httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }

                // Envia a requisição
                var response = await _httpClient.SendAsync(httpRequest);


                var content = await response.Content.ReadAsStringAsync();

                if(response.StatusCode != HttpStatusCode.Unauthorized && response.StatusCode != HttpStatusCode.InternalServerError)
                {
                    var handledError = JsonConvert.DeserializeObject<ApiResponse>(content);
                    return handledError;
                }

                var apiResponse = System.Text.Json.JsonSerializer.Deserialize<ApiResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if(apiResponse == null)
                {
                    throw new InvalidOperationException("Falha ao desserializar a resposta da API.");
                }

                return apiResponse;
            }
            catch(Exception ex)
            {
                return new ApiResponse { Success = false, Message = "Erro ao tentar executar a operação: " + ex.Message };
            }
        }

        public static string BuildQueryString(object queryParams)
        {
            if(queryParams == null)
                return string.Empty;

            var properties = queryParams.GetType().GetProperties()
                .Where(p => p.GetValue(queryParams) != null)
                .Select(p => $"{Uri.EscapeDataString(p.Name)}={Uri.EscapeDataString(p.GetValue(queryParams)!.ToString()!)}");

            return string.Join("&", properties);
        }


    }
}
