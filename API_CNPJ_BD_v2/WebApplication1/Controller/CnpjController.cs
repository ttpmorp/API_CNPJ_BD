// Controller para realizar a consulta de um CNPJ na API da Receita Federal e salvar no banco de dados
using System;
using System.Threading.Tasks;
using CNPJApp.Models;
using CNPJApp.Services;

namespace CNPJApp.Controllers
{
    public class CnpjController
    {
        private readonly ApiService _apiService;
        private readonly DatabaseService _databaseService;

        public CnpjController(ApiService apiService, DatabaseService databaseService)
        {
            _apiService = apiService;
            _databaseService = databaseService;
        }

        public async Task ConsultarESalvarCNPJ(string cnpj)
        {
            Empresa empresa = await _apiService.ConsultarCNPJ(cnpj);
            if (empresa == null || string.IsNullOrEmpty(empresa.cnpj))
            {
                throw new Exception("Erro: Empresa ou CNPJ está vazio!");
            }

            string endereco = $"{empresa.descricao_tipo_de_logradouro} {empresa.logradouro}, {empresa.numero} {empresa.complemento}, {empresa.bairro}, {empresa.municipio} - {empresa.uf}";
            empresa.endereco = endereco;

            await _databaseService.SalvarNoBanco(empresa);

            Console.WriteLine("\n -------------------------- INFORMAÇÕES DA EMPRESA -------------------------- ");
            Console.WriteLine($"CNPJ: {empresa.cnpj}");
            Console.WriteLine($"Razão Social: {empresa.razao_social}");
            Console.WriteLine($"Nome Fantasia: {empresa.nome_fantasia}");
            Console.WriteLine($"Endereço: {empresa.endereco}");
            Console.WriteLine($"CNAE: {empresa.cnae_fiscal_descricao}");
            Console.WriteLine($"Situação: {empresa.descricao_situacao_cadastral}");
            Console.WriteLine("------------------------------------------------------------------------------");
        }
    }
}