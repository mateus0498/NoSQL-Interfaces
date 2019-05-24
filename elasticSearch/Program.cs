// Autor: Mateus Ferreira

using System;
using elasticSearch.Services;

namespace elasticSearch
{
    class Program
    {
        static int Main(string[] args)
        {
            RestService service = new RestService();
            int opcao;
            bool result;

            Console.WriteLine("ElasticSearch Interface (desenvolvido por Mateus Ferreira)\n");

            Console.WriteLine("Estabelecendo conexão com o servidor...");

            try
            {
                result = service.TestarConexao().Result;
                Console.WriteLine("Conexão estabelecida com sucesso.");
            }
            catch (AggregateException)
            {
                Console.WriteLine("ERRO: Falha na conexão com o servidor");
                return -1;
            }

            while (true)
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1 - Listar índices");
                Console.WriteLine("2 - Criar índice");
                Console.WriteLine("3 - Deletar índice");
                Console.WriteLine("4 - Adicionar documento");
                Console.WriteLine("5 - Sair da aplicação");
                Console.Write("Insira a opção desejada: ");
                try
                {
                    opcao = Convert.ToInt32(Console.ReadLine());
                    Console.Write("\n");
                }
                catch (FormatException)
                {
                    Console.WriteLine("ERRO: Entrada inválida");
                    continue;
                }

                switch (opcao)
                {
                    case 1:
                        result = service.ListarIndices().Result;

                        if (!result)
                            Console.WriteLine("Nao foi possivel listar os índices.");

                        break;

                    case 2:
                        Console.Write("Insira o nome do índice a ser criado: ");

                        if (service.CriarIndice(Console.ReadLine()).Result)
                            Console.WriteLine("Índice criado com sucesso.");
                        else
                            Console.WriteLine("Nao foi possivel criar o índice.");

                        break;

                    case 3:
                        Console.Write("Insira o nome do índice a ser deletado: ");

                        if (service.DeletarIndice(Console.ReadLine()).Result)
                            Console.WriteLine("Índice deletado com sucesso.");
                        else
                            Console.WriteLine("Nao foi possivel deletar o índice.");

                        break;

                    case 4:
                        string indice, caminho;
                        int id;

                        Console.Write("Digite o nome do índice no qual será salvo o documento: ");
                        indice = Console.ReadLine();

                        Console.Write("Insira um número para ser usado como ID do documento: ");
                        id = Convert.ToInt32(Console.ReadLine());

                        Console.Write("Insira o caminho do documento (exemplo: C:/customer.json): ");
                        caminho = Console.ReadLine();

                        if (service.AdicionarDocumento(indice, id, caminho).Result)
                            Console.WriteLine("Arquivo adicionado ao banco com sucesso.");
                        else
                            Console.WriteLine("Nao foi possivel adicionar o documento.");

                        break;

                    case 5:
                        return 0;

                    default:
                        Console.WriteLine("ERRO: opção indisponível\n");
                        break;
                }
            }
        }

    }
}