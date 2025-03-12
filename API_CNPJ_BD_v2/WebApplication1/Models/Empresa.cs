namespace CNPJApp.Models
{

    // Classe para armazenar os dados do CNPJ
    // Todos os campos são strings para facilitar a manipulação
    // Todos os campos se encontram no API da BrasilAPI
    /// <summary>
    /// Juntei os campos de endereço em um único campo para facilitar a visualização
    /// ex: "Rua 1, 123, apto 101, Bairro, Cidade - UF" sendo que os campos são separados por vírgula
    /// </summary>
    public class Empresa
    {
        public string? cnpj { get; set; }
        public string? razao_social { get; set; }
        public string? nome_fantasia { get; set; }
        public string? descricao_situacao_cadastral { get; set; }
        public string? cnae_fiscal_descricao { get; set; }
        public string? descricao_tipo_de_logradouro { get; set; }
        public string? logradouro { get; set; }
        public string? numero { get; set; }
        public string? complemento { get; set; }
        public string? bairro { get; set; }
        public string? municipio { get; set; }
        public string? uf { get; set; }
        public string? endereco { get; set; }
    }
}