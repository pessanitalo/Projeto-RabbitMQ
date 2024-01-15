using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

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

        canal.ExchangeDeclare(exchange: "mensagem_empresa", type: ExchangeType.Direct);

        var nomeFila = canal.QueueDeclare().QueueName;

        canal.QueueBind(queue: nomeFila, exchange: "mensagem_empresa", routingKey: "Vendas");

        Console.WriteLine(" [*] Aguardando mensagens.");

        var consumidor = new EventingBasicConsumer(canal);

        consumidor.Received += (model, ea) =>
        {

            var corpo = ea.Body.ToArray();

            var mensagem = Encoding.UTF8.GetString(corpo);

            Console.WriteLine(" [x] Recebido {0}", mensagem);

        };

        canal.BasicConsume(queue: nomeFila, autoAck: true, consumer: consumidor);

        Console.WriteLine(" Pressione [enter] para finalizar.");
        Console.ReadLine();
    }

}