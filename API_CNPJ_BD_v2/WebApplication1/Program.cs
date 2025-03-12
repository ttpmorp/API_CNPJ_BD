// Program.cs
using System;
using System.Threading.Tasks;
using CNPJApp.Controllers;
using CNPJApp.Services;

class Program
{
    static async Task Main()
    {
        var apiService = new ApiService();
        // Endereço do banco de dados pode ser modificado conforme necessário
        var databaseService = new DatabaseService("Data Source=DLX-13;Initial Catalog=APIDLX;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        var cnpjController = new CnpjController(apiService, databaseService);

        bool continuar = true;

        while (continuar)
        {
            Console.Write("Digite o CNPJ (ou 'sair' para encerrar): ");
            string cnpj = Console.ReadLine();

            if (cnpj.ToLower() == "sair")
            {
                continuar = false;
                Console.WriteLine("Encerrando o programa...");
            }
            else
            {
                try
                {
                    await cnpjController.ConsultarESalvarCNPJ(cnpj);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro: {ex.Message}");
                }

                Console.WriteLine();
                Console.Write("Deseja consultar outro CNPJ?(s/n): ");
                string resposta = Console.ReadLine();

                if (resposta.ToLower() == "n")
                {
                    continuar = false;
                    Console.WriteLine("Encerrando o programa...");
                }


            }
        }
    }
}