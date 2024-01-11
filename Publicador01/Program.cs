using RabbitMQ.Client;
using System.Text;

namespace Publicador
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var servidor = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "usuario",
                Password = "senha@123",
            };

            var conexao = servidor.CreateConnection();
            {
                using (var canal = conexao.CreateModel())
                {
                    canal.QueueDeclare(queue: "fila_de_tarefas",
                                       durable: true,
                                       exclusive: false,
                                       autoDelete: false,
                                       arguments: null);

                    Console.WriteLine("'Publicador: ' Digite uma mensagem");

                    while (true)
                    {
                        string mensagem = Console.ReadLine();

                        if (mensagem == "!finalizar") break;

                        var corpoMensagem = Encoding.UTF8.GetBytes(mensagem);

                        var propriedades = canal.CreateBasicProperties();
                        propriedades.Persistent = true;

                        canal.BasicPublish(exchange: "",
                                 routingKey: "fila_de_tarefas",
                                 basicProperties: propriedades,
                                 body: corpoMensagem);

                        Console.WriteLine(" [x] Enviou {0} ", mensagem);
                    }


                }
                Console.WriteLine("precione enter para sair");
                Console.ReadLine();
            }
        }
    }
}
