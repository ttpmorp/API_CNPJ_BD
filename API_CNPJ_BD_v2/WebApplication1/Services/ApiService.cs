// Buscar dadps dp CNPJ na API Brasil API
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CNPJApp.Models;

namespace CNPJApp.Services
{
    public class ApiService
    {
        public async Task<Empresa> ConsultarCNPJ(string cnpj)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = $"https://brasilapi.com.br/api/cnpj/v1/{cnpj}";
                    var response = await client.GetStringAsync(url);
                    return JsonSerializer.Deserialize<Empresa>(response);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching CNPJ data: {ex.Message}");
                    return null;
                }
            }
        }
    }
}