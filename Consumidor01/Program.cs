using RabbitMQ.Client;
using RabbitMQ.Client.Events;
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

                    canal.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                    var consumidor = new EventingBasicConsumer(canal);

                    consumidor.Received += (model, ea) =>
                    {
                        var corpo = ea.Body.ToArray();

                        var mensagem = Encoding.UTF8.GetString(corpo);

                        Console.WriteLine(" [x] Enviou {0} ", mensagem);

                        canal.BasicAck(deliveryTag:ea.DeliveryTag,multiple:false);

                    };

                    canal.BasicConsume(queue: "fila_de_tarefas", autoAck: true, consumer: consumidor);

                }
                Console.WriteLine("precione enter para sair");
                Console.ReadLine();
            }
        }
    }
}
