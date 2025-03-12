// Salvar dados no SQL Server
using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using CNPJApp.Models;

namespace CNPJApp.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task SalvarNoBanco(Empresa empresa)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // Abrir a conexão com o banco de dados
                // Colunas da tabela CNPJDLX: CNPJ, RazaoSocial, NomeFantasia, Situacao, CNAE, Endereco
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
}