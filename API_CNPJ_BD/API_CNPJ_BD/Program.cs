using System;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.Write("Digite o CNPJ: ");
        string cnpj = Console.ReadLine();

        // Consulta a API
        Empresa empresa = await ConsultarCNPJ(cnpj);
        if (empresa == null || string.IsNullOrEmpty(empresa.cnpj))
        {
            throw new Exception("Erro: Empresa ou CNPJ está vazio!");
        }


        // Criando o endereço concatenado
        string endereco = $"{empresa.descricao_tipo_de_logradouro} {empresa.logradouro}, {empresa.numero} {empresa.complemento}, {empresa.bairro}, {empresa.municipio} - {empresa.uf}";
        empresa.endereco = endereco;

        // Salva no Banco
        await SalvarNoBanco(empresa);

        // Exibe as informações na tela
        Console.WriteLine("\n🔹 Informações da Empresa 🔹");
        Console.WriteLine($"CNPJ: {empresa.cnpj}");
        Console.WriteLine($"Razão Social: {empresa.razao_social}");
        Console.WriteLine($"Nome Fantasia: {empresa.nome_fantasia}");
        Console.WriteLine($"Endereço: {empresa.endereco}");
        Console.WriteLine($"CNAE: {empresa.cnae_fiscal_descricao}");
        Console.WriteLine($"Situação: {empresa.descricao_situacao_cadastral}");
    }

    // Buscar dados do CNPJ na API Brasil API
    static async Task<Empresa> ConsultarCNPJ(string cnpj)
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


    // Salvar dados no SQL Server
    static async Task SalvarNoBanco(Empresa empresa)
    {
        string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=APIDLX;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            await conn.OpenAsync();
            string query = @"INSERT INTO CNPJDLX (CNPJ, RazaoSocial, NomeFantasia, Situacao, CNAE, Endereco)
                 VALUES (@CNPJ, @RazaoSocial, @NomeFantasia, @Situacao, @CNAE, @Endereco)";


            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@cnpj", empresa.cnpj ?? ""); // CNPJ NÃO PODE SER NULL
                cmd.Parameters.AddWithValue("@RazaoSocial", empresa.razao_social ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@NomeFantasia", empresa.nome_fantasia ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Situacao", empresa.descricao_situacao_cadastral ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@CNAE", empresa.cnae_fiscal_descricao ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Endereco", empresa.endereco ?? (object)DBNull.Value);

                await cmd.ExecuteNonQueryAsync();
            }

        }
    }

}

// Classe para armazenar os dados do CNPJ
public class Empresa
{
    public string cnpj { get; set; }
    public string razao_social { get; set; }
    public string nome_fantasia { get; set; }
    public string descricao_situacao_cadastral { get; set; }
    public string cnae_fiscal_descricao { get; set; }
    public string descricao_tipo_de_logradouro { get; set; }
    public string logradouro { get; set; }
    public string numero { get; set; }
    public string complemento { get; set; }
    public string bairro { get; set; }
    public string municipio { get; set; }
    public string uf { get; set; }
    public string endereco { get; set; }
}