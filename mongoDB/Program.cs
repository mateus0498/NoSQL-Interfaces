// Autor: Mateus Ferreira

using System;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace mongoDB
{
    class Program
    {
        static int Main(string[] args)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            int opcao;

            Console.WriteLine("MongoDB Interface (desenvolvido por Mateus Ferreira)\n");
            Console.WriteLine("Estabelecendo conexão com o servidor...");

            try
            {
                client.StartSession();
                Console.WriteLine("Conexão estabelecida com sucesso.");
            } catch (TimeoutException)
            {
                Console.WriteLine("ERRO: Falha na conexão com o servidor");
                return -1;
            }

            while (true)
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1 - Listar bancos");
                Console.WriteLine("2 - Deletar banco");
                Console.WriteLine("3 - Listar coleções");
                Console.WriteLine("4 - Criar coleção");
                Console.WriteLine("5 - Deletar coleção");
                Console.WriteLine("6 - Listar documentos");
                Console.WriteLine("7 - Inserir documento");
                Console.WriteLine("8 - Sair da aplicação");
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
                        using (var cursor = client.ListDatabases())
                        {
                            foreach (var document in cursor.ToEnumerable())
                            {
                                Console.WriteLine(document.ToString());
                            }
                        }
                        break;

                    case 2:
                        Console.Write("Insira o nome do banco a ser deletado: ");
                        client.DropDatabase(Console.ReadLine());
                        break;

                    case 3:
                        Console.Write("Insira o nome do banco: ");

                        using (var cursor = client.GetDatabase(Console.ReadLine()).ListCollections())
                        {
                            foreach (var document in cursor.ToEnumerable())
                            {
                                Console.WriteLine(document.ToString());
                            }
                        }
                        break;

                    case 4:
                        string colecao, banco;

                        Console.Write("Insira o nome da coleção a ser criada: ");
                        colecao = Console.ReadLine();

                        Console.Write("Insira o nome do banco onde será armazenada a coleção (caso o banco não exista, será criado automaticamente): ");
                        banco = Console.ReadLine();

                        client.GetDatabase(banco).CreateCollection(colecao, null);
                        break;

                    case 5:
                        Console.Write("Insira o nome do banco: ");
                        banco = Console.ReadLine();

                        Console.Write("Insira o nome da colecao: ");
                        colecao = Console.ReadLine();

                        client.GetDatabase(banco).DropCollection(colecao);
                        break;

                    case 6:
                        Console.Write("Insira o nome do banco: ");
                        banco = Console.ReadLine();

                        Console.Write("Insira o nome da colecao: ");
                        colecao = Console.ReadLine();

                        var percurso = client.GetDatabase(banco)
                            .GetCollection<BsonDocument>(colecao)
                            .Find(new BsonDocument())
                            .ToCursor();

                        foreach (var document in percurso.ToEnumerable())
                        {
                            Console.WriteLine(document);
                        }

                        break;

                    case 7:
                        string caminho;
                        Console.Write("Insira o nome do banco: ");
                        banco = Console.ReadLine();

                        Console.Write("Insira o nome da colecao: ");
                        colecao = Console.ReadLine();

                        Console.Write("Insira o caminho do documento (exemplo: C:/customer.json): ");
                        caminho = Console.ReadLine();

                        IMongoCollection<BsonDocument> collection = client.GetDatabase(banco)
                            .GetCollection<BsonDocument>(colecao);

                        try
                        {
                            var text = System.IO.File.ReadAllText(caminho);

                            var jsonReader = new JsonReader(text);
                            var context = BsonDeserializationContext.CreateRoot(jsonReader);
                            var document = collection.DocumentSerializer.Deserialize(context);
                            collection.InsertOneAsync(document);
                        }
                        catch (System.IO.FileNotFoundException)
                        {
                            Console.WriteLine("ERRO: arquivo inexistente");
                        }

                        break;

                    case 8:
                        return 0;

                    default:
                        Console.WriteLine("ERRO: opção indisponível");
                        break;
                }

            }

        }

    }
}