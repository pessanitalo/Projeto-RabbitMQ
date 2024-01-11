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
                    canal.QueueDeclare(queue: "ola",
                                       durable: false,
                                       exclusive: false,
                                       autoDelete: false,
                                       arguments: null);

                    string message = "Olá mundo";
                    var corpoMensagem = Encoding.UTF8.GetBytes(message);

                    canal.BasicPublish(exchange: "", routingKey: "ola", basicProperties: null, body: corpoMensagem);

                    Console.WriteLine(" [x] Enviou {0} ", message);

                }
                Console.WriteLine("precione enter para sair");
                Console.ReadLine();
            }
        }
    }
}
